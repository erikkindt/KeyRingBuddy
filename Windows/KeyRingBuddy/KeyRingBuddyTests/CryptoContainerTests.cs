using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Tests for the CryptoContainer class.
    /// </summary>
    [TestClass]
    public class CryptoContainerTests
    {
        /// <summary>
        /// Test the SetData and GetData methods.
        /// </summary>
        [TestMethod]
        public void CryptoContainer_SetAndGetDataTest()
        {
            CryptoContainer<Account> container = new CryptoContainer<Account>();
            SecureString password = SecureStringUtility.CreateSecureString("my test password");
            Account account = new Account("test name", "test category", "test site");
            container.SetData(account, password);

            Account otherAccount = container.GetData(password);
            Assert.AreEqual(account.Id, otherAccount.Id, "Account.Name doesn't match.");
            Assert.AreEqual(account.Name, otherAccount.Name, "Account.Name doesn't match.");
            Assert.AreEqual(account.Category, otherAccount.Category, "Account.Category doesn't match.");
            Assert.AreEqual(account.Site, otherAccount.Site, "Account.Site doesn't match.");
        }

        /// <summary>
        /// Test the serialization methods.
        /// </summary>
        [TestMethod]
        public void CryptoContainer_SerializationTest()
        {
            CryptoContainer<Account> container = new CryptoContainer<Account>();
            SecureString password = SecureStringUtility.CreateSecureString("my test password");
            Account account = new Account("test name", "test category", "test site");
            container.SetData(account, password);

            XmlSerializer ser = new XmlSerializer(typeof(CryptoContainer<Account>));

            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter xml = XmlWriter.Create(writer, settings);
                ser.Serialize(xml, container);
            }
            Console.WriteLine(sb.ToString());

            using (StringReader reader = new StringReader(sb.ToString()))
            {
                CryptoContainer<Account> otherContainer = ser.Deserialize(reader) as CryptoContainer<Account>;
                Account otherAccount = otherContainer.GetData(password);

                Assert.AreEqual(account.Id, otherAccount.Id, "Account.Name doesn't match.");
                Assert.AreEqual(account.Name, otherAccount.Name, "Account.Name doesn't match.");
                Assert.AreEqual(account.Category, otherAccount.Category, "Account.Category doesn't match.");
                Assert.AreEqual(account.Site, otherAccount.Site, "Account.Site doesn't match.");
            }
        }
    }
}
