using KeyRingBuddy.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// An implementation of IProfile that uses a local zip file as it's store.
    /// </summary>
    public class ZipProfile : IProfile
    {
        #region Fields

        /// <summary>
        /// The password used to read from and write to the profile with.
        /// </summary>
        private SecureString _password = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filePath">The zip file path.</param>
        public ZipProfile(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("filePath is null or whitespace", "filePath");

            FilePath = filePath;

            // create zip archive if it doesn't exist
            if (!File.Exists(FilePath))
            {
                using (Stream fileStream = File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Update))
                    {
                    }
                }

                Id = Guid.NewGuid();
                WriteEntryUnsecured<SerializableDetail>("Id", new SerializableDetail(Id.ToString()));
            }
            // read in some values from existing profile
            else
            {
                SerializableDetail idDetail = ReadEntryUnsecured<SerializableDetail>("Id");
                if (idDetail == null)
                    throw new Exception("The profile is missing an id.");
                Id = Guid.Parse(idDetail.Value);

                SerializableDetail nameDetail = ReadEntryUnsecured<SerializableDetail>("Name");
                if (nameDetail != null)
                    Name = nameDetail.Value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filePath">The zip file path.</param>
        /// <param name="password">The password used to read from and write to the profile with.</param>
        public ZipProfile(string filePath, SecureString password)
            : this(filePath)
        {
            if (!SetPassword(password))
                throw new Exception("The password provided is incorrect.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The path to the zip file.
        /// </summary>
        public string FilePath { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set the password to use for the profile.
        /// </summary>
        /// <param name="password">The password for the profile.</param>
        /// <returns>
        /// true if the password is set correctly.  
        /// false if the password is not set because there is an existing password that doesn't match.
        /// </returns>
        public bool SetPassword(SecureString password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            // check for existing password
            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = archive.GetEntry("PasswordCheck.secure");

                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<SerializableDetail>));
                            CryptoContainer<SerializableDetail> container = ser.Deserialize(stream) as CryptoContainer<SerializableDetail>;

                            try
                            {
                                SerializableDetail detail = container.GetData(password);
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            _password = password;
            WriteEntry<SerializableDetail>("PasswordCheck", new SerializableDetail("check"));

            return true;
        }

        /// <summary>
        /// Update the password for the profile.
        /// </summary>
        /// <param name="password">The new password for the profile.</param>
        public void UpdatePassword(SecureString password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (_password == null)
                throw new Exception("Password is not set.  Call SetPassword method first.");

            if (SecureStringUtility.GetPlainText(_password) == SecureStringUtility.GetPlainText(password))
                return;

            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    List<ZipArchiveEntry> entries = new List<ZipArchiveEntry>(archive.Entries);
                    foreach (ZipArchiveEntry entry in entries)
                    {
                        if (!entry.Name.EndsWith(".secure"))
                            continue;

                        switch (entry.Name)
                        {
                            case "headers.secure":
                                AccountHeaderCollection headers = null;

                                using (Stream stream = entry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<AccountHeaderCollection>));
                                    CryptoContainer<AccountHeaderCollection> container = ser.Deserialize(stream) as CryptoContainer<AccountHeaderCollection>;
                                    headers = container.GetData(_password);
                                }

                                string headersName = entry.FullName;
                                entry.Delete();
                                ZipArchiveEntry headersEntry = archive.CreateEntry(headersName);

                                using (Stream stream = headersEntry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<AccountHeaderCollection>));
                                    CryptoContainer<AccountHeaderCollection> container = new CryptoContainer<AccountHeaderCollection>(headers, password);
                                    ser.Serialize(stream, container);
                                }
                                break;

                            case "PasswordCheck.secure":
                                SerializableDetail check = null;

                                using (Stream stream = entry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<SerializableDetail>));
                                    CryptoContainer<SerializableDetail> container = ser.Deserialize(stream) as CryptoContainer<SerializableDetail>;
                                    check = container.GetData(_password);
                                }

                                string checkName = entry.FullName;
                                entry.Delete();
                                ZipArchiveEntry checkEntry = archive.CreateEntry(checkName);

                                using (Stream stream = checkEntry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<SerializableDetail>));
                                    CryptoContainer<SerializableDetail> container = new CryptoContainer<SerializableDetail>(check, password);
                                    ser.Serialize(stream, container);
                                }
                                break;

                            default:
                                Account account = null;

                                using (Stream stream = entry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<Account>));
                                    CryptoContainer<Account> container = ser.Deserialize(stream) as CryptoContainer<Account>;
                                    account = container.GetData(_password);
                                }

                                string accountName = entry.FullName;
                                entry.Delete();
                                ZipArchiveEntry accountEntry = archive.CreateEntry(accountName);

                                using (Stream stream = accountEntry.Open())
                                {
                                    XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<Account>));
                                    CryptoContainer<Account> container = new CryptoContainer<Account>(account, password);
                                    ser.Serialize(stream, container);
                                }
                                break;
                        }
                    }
                }
            }

            _password = password;
        }

        /// <summary>
        /// Helper method used to get an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to read in.</typeparam>
        /// <param name="id">The id of the object to read in.  If it's Guid.Empty the AccountHeader collection will be read in.</param>
        /// <returns>The object that was read in or null if the object wasn't found.</returns>
        private TType ReadEntry<TType>(Guid id) where TType : IXmlSerializable
        {
            return ReadEntry<TType>(id.ToString("N"));
        }

        /// <summary>
        /// Helper method used to get an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to read in.</typeparam>
        /// <param name="name">The name the object to read in.  
        /// <returns>The object that was read in or null if the object wasn't found.</returns>
        private TType ReadEntry<TType>(string name) where TType : IXmlSerializable
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name is null or whitespace", "name");

            if (_password == null)
                throw new Exception("Password is not set.  Call SetPassword method first.");

            TType result = default(TType);
            string entryName = String.Format("{0}.secure", name);

            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryName);

                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<TType>));
                            CryptoContainer<TType> container = ser.Deserialize(stream) as CryptoContainer<TType>;
                            result = container.GetData(_password);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Helper method used to write an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to write an entry for.</typeparam>
        /// <param name="id">The id for the object.</param>
        /// <param name="item">The item to write out.</param>
        private void WriteEntry<TType>(Guid id, TType item) where TType : IXmlSerializable
        {
            WriteEntry<TType>(id.ToString("N"), item);
        }

        /// <summary>
        /// Helper method used to write an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to write an entry for.</typeparam>
        /// <param name="name">The name for the entry.</param>
        /// <param name="item">The item to write out.</param>
        private void WriteEntry<TType>(string name, TType item) where TType : IXmlSerializable
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name is null or whitespace", "name");

            if (_password == null)
                throw new Exception("Password is not set.  Call SetPassword method first.");

            string entryName = String.Format("{0}.secure", name);
            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryName);
                    if (entry != null)
                        entry.Delete();

                    if (item != null)
                        entry = archive.CreateEntry(entryName);
                    else
                        entry = null;


                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<TType>));
                            CryptoContainer<TType> container = new CryptoContainer<TType>(item, _password);
                            ser.Serialize(stream, container);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method used to get an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to read in.</typeparam>
        /// <param name="name">The name the object to read in.  
        /// <returns>The object that was read in or null if the object wasn't found.</returns>
        private TType ReadEntryUnsecured<TType>(string name) where TType : IXmlSerializable
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name is null or whitespace", "name");

            TType result = default(TType);
            string entryName = String.Format("{0}.unsecure", name);

            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryName);

                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(TType));
                            result = (TType)ser.Deserialize(stream);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Helper method used to write an entry for the given object.
        /// </summary>
        /// <typeparam name="TType">The type of object to write an entry for.</typeparam>
        /// <param name="name">The name for the entry.</param>
        /// <param name="item">The item to write out.</param>
        private void WriteEntryUnsecured<TType>(string name, TType item) where TType : IXmlSerializable
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name is null or whitespace", "name");

            string entryName = String.Format("{0}.unsecure", name);
            using (Stream fileStream = File.Open(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(entryName);
                    if (entry != null)
                        entry.Delete();

                    if (item != null)
                        entry = archive.CreateEntry(entryName);
                    else
                        entry = null;


                    if (entry != null)
                    {
                        using (Stream stream = entry.Open())
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(TType));
                            ser.Serialize(stream, item);
                        }
                    }
                }
            }
        }

        #endregion

        #region IProfile Members

        /// <summary>
        /// The id for the profile.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The name for the profile.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Update the name for the profile.
        /// </summary>
        /// <param name="name">The new name for the profile.</param>
        public void UpdateName(string name)
        {
            WriteEntryUnsecured<SerializableDetail>("Name", new SerializableDetail(name));
            Name = name;
        }

        /// <summary>
        /// Get headers for all of the accounts in this profile.
        /// </summary>
        /// <returns>The headers for all of the accounts in this profile.</returns>
        public IEnumerable<AccountHeader> GetAccountHeaders()
        {
            IEnumerable<AccountHeader> result = ReadEntry<AccountHeaderCollection>("headers");
            if (result == null)
                return new List<AccountHeader>();
            else
                return result;
        }

        /// <summary>
        /// Get the account with the given id.
        /// </summary>
        /// <param name="id">The id of the account to get.</param>
        /// <returns>The id or null if there is no account with the given id.</returns>
        public Account GetAccount(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Account id's must be a non empty guid.", "id");

            return ReadEntry<Account>(id);
        }

        /// <summary>
        /// Add a new account.
        /// </summary>
        /// <param name="account">The account to add.</param>
        public void AddAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");
            if (account.Id == Guid.Empty)
                throw new ArgumentException("Account id's must be a non empty guid.", "id");

            Account a = GetAccount(account.Id);
            if (a != null)
                throw new ArgumentException("This account already exists in the profile and can't be added again.", "account");

            WriteEntry<Account>(account.Id, account);

            AccountHeaderCollection headers = ReadEntry<AccountHeaderCollection>("headers") ?? new AccountHeaderCollection();
            headers.Add(new AccountHeader(account.Category, account.Name, account.Id));
            WriteEntry<AccountHeaderCollection>("headers", headers);
        }

        /// <summary>
        /// Update the given account.
        /// </summary>
        /// <param name="account">The account to update.</param>
        public void UpdateAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");
            if (account.Id == Guid.Empty)
                throw new ArgumentException("Account id's must be a non empty guid.", "id");

            Account a = GetAccount(account.Id);
            if (a == null)
                throw new ArgumentException("This account doesn't exist in the profile and can't be updated.", "account");

            WriteEntry<Account>(account.Id, account);

            AccountHeaderCollection headers = ReadEntry<AccountHeaderCollection>("headers") ?? new AccountHeaderCollection();
            foreach (AccountHeader header in headers)
            {
                if (header.AccountId == account.Id)
                {
                    header.Category = account.Category;
                    header.AccountName = account.Name;
                }
            }
            WriteEntry<AccountHeaderCollection>("headers", headers);
        }

        /// <summary>
        /// Delete the given account.
        /// </summary>
        /// <param name="id">The id of the account to delete.</param>
        public void DeleteAccount(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Account id's must be a non empty guid.", "id");

            Account account = GetAccount(id);
            if (account == null)
                throw new ArgumentException("This account doesn't exist in the profile and can't be deleted.", "account");

            WriteEntry<Account>(account.Id, null);

            AccountHeaderCollection headers = ReadEntry<AccountHeaderCollection>("headers") ?? new AccountHeaderCollection();
            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i].AccountId == id)
                {
                    headers.RemoveAt(i);
                    i--;
                }
            }
            WriteEntry<AccountHeaderCollection>("headers", headers);
        }

        #endregion
    }
}
