using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Tests for the account detail class.
    /// </summary>
    [TestClass]
    public class AccountDetailTests
    {
        /// <summary>
        /// Test the constructor.
        /// </summary>
        [TestMethod]
        public void AccountDetail_ConstructorTest()
        {
            AccountDetail ad = new AccountDetail("testName", "testValue");
            Assert.AreEqual<string>("testName", ad.Name);
            Assert.AreEqual<string>("testValue", ad.Value);
        }

        /// <summary>
        /// Test the ToString method.
        /// </summary>
        [TestMethod]
        public void AccountDetail_ToStringTest()
        {
            AccountDetail ad = new AccountDetail("testName", "testValue");
            Assert.AreEqual<string>("testName : testValue", ad.ToString());
        }

        /// <summary>
        /// Test the static Equals method.
        /// </summary>
        [TestMethod]
        public void AccountDetail_StaticEqualsTest()
        {

            AccountDetail a = new AccountDetail("testName", "testValue1");
            AccountDetail b = new AccountDetail("testName", "testValue1");
            AccountDetail c = new AccountDetail("testNameDiff", "testValue2");
            AccountDetail d = null;

            Assert.IsTrue(AccountDetail.Equals(a, b));
            Assert.IsFalse(AccountDetail.Equals(a, c));
            Assert.IsFalse(AccountDetail.Equals(a, d));
        }

        /// <summary>
        /// Test the Equals method.
        /// </summary>
        [TestMethod]
        public void AccountDetail_EqualsTest()
        {
            AccountDetail a = new AccountDetail("testName", "testValue1");
            AccountDetail b = new AccountDetail("testName", "testValue1");
            AccountDetail c = new AccountDetail("testNameDiff", "testValue2");
            AccountDetail d = null;

            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(a.Equals(d));
        }

        /// <summary>
        /// Test the serialization methods.
        /// </summary>
        [TestMethod]
        public void AccountDetail_XmlSerializeTest()
        {
            AccountDetail ad = new AccountDetail("testName", "testValue");
            XmlSerializer ser = new XmlSerializer(typeof(AccountDetail));
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, ad);
                string xml = writer.ToString();
                Console.Write(xml);

                using (StringReader reader = new StringReader(xml))
                {
                    AccountDetail adReadIn = (AccountDetail)ser.Deserialize(reader);
                    Assert.AreEqual<string>("testName", adReadIn.Name);
                    Assert.AreEqual<string>("testValue", adReadIn.Value);
                }
            }
        }
    }
}
