using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Test the methods in the ZipProfile class.
    /// </summary>
    [TestClass]
    public class ZipProfileTests
    {
        /// <summary>
        /// Test the Id and name properties.
        /// </summary>
        [TestMethod]
        public void ZipProfile_IdAndNameTest()
        {
            if (System.IO.File.Exists("TestProfile.zip"))
                System.IO.File.Delete("TestProfile.zip");

            ZipProfile profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));
            Guid id = profile.Id;
            profile.UpdateName("MyTestName");

            profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));
            Assert.AreEqual<Guid>(id, profile.Id);
            Assert.AreEqual<string>("MyTestName", profile.Name);
        }

        /// <summary>
        /// Test the account header management.
        /// </summary>
        [TestMethod]
        public void ZipProfile_AccountHeaderTest()
        {
            if (System.IO.File.Exists("TestProfile.zip"))
                System.IO.File.Delete("TestProfile.zip");

            ZipProfile profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));
            IEnumerable<AccountHeader> headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(0, headers.Count());

            Account account1 = new Account("test account 1", "test category/sub", "google.com");
            Account account2 = new Account("test account 2", "test category/sub", "yahoo.com");

            profile.AddAccount(account1);
            profile.AddAccount(account2);

            headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(2, headers.Count());

            bool foundFirst = false;
            bool foundSecond = false;
            foreach (AccountHeader header in headers)
            {
                if (header.AccountId == account1.Id &&
                    header.AccountName == account1.Name &&
                    header.Category == account1.Category)
                    foundFirst = true;
                else if (header.AccountId == account2.Id &&
                         header.AccountName == account2.Name &&
                         header.Category == account2.Category)
                    foundSecond = true;
            }

            Assert.IsTrue(foundFirst, "First header not found.");
            Assert.IsTrue(foundSecond, "Second header not found.");

            profile.DeleteAccount(account1.Id);

            headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(1, headers.Count());

            AccountHeader h = headers.First();
            Assert.AreEqual<Guid>(account2.Id, h.AccountId);
            Assert.AreEqual<string>(account2.Name, h.AccountName);
            Assert.AreEqual<string>(account2.Category, h.Category);

            account2.Name = "new account name";
            account2.Category = "new category";
            profile.UpdateAccount(account2);

            headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(1, headers.Count());

            h = headers.First();
            Assert.AreEqual<Guid>(account2.Id, h.AccountId);
            Assert.AreEqual<string>(account2.Name, h.AccountName);
            Assert.AreEqual<string>(account2.Category, h.Category);
        }

        /// <summary>
        /// Test the account header management.
        /// </summary>
        [TestMethod]
        public void ZipProfile_AccountTest()
        {
            if (System.IO.File.Exists("TestProfile.zip"))
                System.IO.File.Delete("TestProfile.zip");

            ZipProfile profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));
            IEnumerable<AccountHeader> headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(0, headers.Count());

            Account account1 = new Account("test account 1", "test category/sub", "google.com");
            account1.Details.Add(new AccountDetail("user", "bob"));
            account1.Details.Add(new AccountDetail("pass", "secret"));
            Account account2 = new Account("test account 2", "test category/sub", "yahoo.com");

            profile.AddAccount(account1);
            profile.AddAccount(account2);

            Account testAccount = profile.GetAccount(account1.Id);

            Assert.AreEqual<Guid>(account1.Id, testAccount.Id);
            Assert.AreEqual<string>(account1.Name, testAccount.Name);
            Assert.AreEqual<string>(account1.Category, testAccount.Category);
            Assert.AreEqual<string>(account1.Site, testAccount.Site);
            Assert.AreEqual<int>(account1.Details.Count, testAccount.Details.Count);
            for (int i = 0; i < account1.Details.Count; i++)
            {
                Assert.AreEqual<string>(account1.Details[i].Name, testAccount.Details[i].Name);
                Assert.AreEqual<string>(account1.Details[i].Value, testAccount.Details[i].Value);
            }

            account1.Name = "new name";
            account1.Details[1].Value = "sue";
            profile.UpdateAccount(account1);
            testAccount = profile.GetAccount(account1.Id);

            Assert.AreEqual<Guid>(account1.Id, testAccount.Id);
            Assert.AreEqual<string>(account1.Name, testAccount.Name);
            Assert.AreEqual<string>(account1.Category, testAccount.Category);
            Assert.AreEqual<string>(account1.Site, testAccount.Site);
            Assert.AreEqual<int>(account1.Details.Count, testAccount.Details.Count);
            for (int i = 0; i < account1.Details.Count; i++)
            {
                Assert.AreEqual<string>(account1.Details[i].Name, testAccount.Details[i].Name);
                Assert.AreEqual<string>(account1.Details[i].Value, testAccount.Details[i].Value);
            }

            profile.DeleteAccount(account1.Id);
            headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(1, headers.Count());
            testAccount = profile.GetAccount(account1.Id);
            Assert.IsNull(testAccount);
        }

        /// <summary>
        /// Test the save functions.
        /// </summary>
        [TestMethod]
        public void ZipProfile_PersistenceTest()
        {
            if (System.IO.File.Exists("TestProfile.zip"))
                System.IO.File.Delete("TestProfile.zip");

            ZipProfile profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));

            Account account1 = new Account("test account 1", "test category/sub", "google.com");
            account1.Details.Add(new AccountDetail("user", "bob"));
            account1.Details.Add(new AccountDetail("pass", "secret"));
            Account account2 = new Account("test account 2", "test category/sub", "yahoo.com");

            profile.AddAccount(account1);
            profile.AddAccount(account2);

            profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));

            IEnumerable<AccountHeader> headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(2, headers.Count());

            Account testAccount = profile.GetAccount(account1.Id);
            Assert.AreEqual<string>("test account 1", testAccount.Name);
        }

        /// <summary>
        /// Test the UpdatePassword method.
        /// </summary>
        [TestMethod]
        public void ZipProfile_UpdatePasswordTest()
        {
            if (System.IO.File.Exists("TestProfile.zip"))
                System.IO.File.Delete("TestProfile.zip");

            ZipProfile profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypass"));

            Account account1 = new Account("test account 1", "test category/sub", "google.com");
            account1.Details.Add(new AccountDetail("user", "bob"));
            account1.Details.Add(new AccountDetail("pass", "secret"));
            Account account2 = new Account("test account 2", "test category/sub", "yahoo.com");

            profile.AddAccount(account1);
            profile.AddAccount(account2);

            profile.UpdatePassword(SecureStringUtility.CreateSecureString("mypassUPDATE"));

            profile = new ZipProfile("TestProfile.zip", SecureStringUtility.CreateSecureString("mypassUPDATE"));

            IEnumerable<AccountHeader> headers = profile.GetAccountHeaders();
            Assert.AreEqual<int>(2, headers.Count());

            Account testAccount = profile.GetAccount(account1.Id);
            Assert.AreEqual<string>("test account 1", testAccount.Name);
        }
    }
}
