using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Holds an object in an encrypted state.
    /// </summary>
    /// <typeparam name="TType">The type of data held in this container.</typeparam>
    public class CryptoContainer<TType> : IXmlSerializable where TType : IXmlSerializable
    {
        #region Fields

        /// <summary>
        /// The initialization vector used to encrypt the current secure data.
        /// </summary>
        private byte[] _iv;

        /// <summary>
        /// The data that has been encrypted in this container.
        /// </summary>
        private byte[] _secureData;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CryptoContainer()
        {
            _iv = null;
            _secureData = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">The data to secure in this object.</param>
        /// <param name="password">The password to secure the data with.</param>
        public CryptoContainer(TType data, SecureString password)
            : this()
        {
            SetData(data, password);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the data that is held in this class.
        /// </summary>
        /// <typeparam name="TType">The type of object to hold.</typeparam>
        /// <param name="password">The password that secures the data.</param>
        /// <returns>The data held in this container.</returns>
        public TType GetData(SecureString password)
        {
            if (_secureData == null)
                return default(TType);

            using (SymmetricAlgorithm crypto = CreateCryptoProvider(password, _iv))
            {
                using (MemoryStream ms = new MemoryStream(_secureData))
                {
                    using (CryptoStream cs = new CryptoStream(ms, crypto.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        XmlReader reader = XmlReader.Create(cs);

                        if (!reader.ReadToFollowing("data"))
                            throw new Exception("Unable to parse data.");

                        TType data = Activator.CreateInstance<TType>();
                        data.ReadXml(reader);

                        return data;
                    }
                }
            }
        }

        /// <summary>
        /// Set the data that is to be secured in this object.
        /// </summary>
        /// <param name="data">The data to be secured.</param>
        /// <param name="password">The password to secure the data with.</param>
        public void SetData(TType data, SecureString password)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (SymmetricAlgorithm crypto = CreateCryptoProvider(password, null))
            {
                _iv = crypto.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, crypto.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (XmlWriter xml = XmlWriter.Create(cs))
                        {
                            xml.WriteStartDocument();
                            xml.WriteStartElement("data");
                            data.WriteXml(xml);
                            xml.WriteEndElement();
                        }
                    }

                    _secureData = ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Create a crypto provider that is used to encrypt or decrypt data in this object.
        /// </summary>
        /// <param name="password">The password to use for encrypt or decrypt operations.</param>
        /// <param name="iv">The initialization vector to use.  If null a new IV will be generated.</param>
        /// <returns>A crypto provider to use for encrypt or decrypt operations.</returns>
        private SymmetricAlgorithm CreateCryptoProvider(SecureString password, byte[] iv)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (password.Length == 0)
                throw new ArgumentException("password length is zero.", "password");
            if (iv != null && iv.Length != 16)
                throw new ArgumentException("iv length is not 16.  It is " + iv.Length, "iv");

            AesManaged cryptoProvider = new AesManaged();

            // set iv
            if (iv == null)
                cryptoProvider.GenerateIV();
            else
                cryptoProvider.IV = iv;

            // set password
            string passwordText = SecureStringUtility.GetPlainText(password);
            cryptoProvider.Key = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(passwordText));

            return cryptoProvider;
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// Not used.
        /// </summary>
        /// <returns>null.</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Read in this object from xml.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            string ivText = reader["iv"];
            if (ivText != null)
            {
                _iv = Convert.FromBase64String(ivText);
                reader.Read();
                _secureData = Convert.FromBase64String(reader.ReadContentAsString());
            }

            reader.Read();
        }

        /// <summary>
        /// Write this object out to xml.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            if (_secureData != null)
            {
                writer.WriteAttributeString("iv", Convert.ToBase64String(_iv));
                writer.WriteString(Convert.ToBase64String(_secureData));
            }
        }

        #endregion
    }
}
