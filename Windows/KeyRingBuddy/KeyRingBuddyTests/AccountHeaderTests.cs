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
    /// Tests for AccountHeader class.
    /// </summary>
    [TestClass]
    public class AccountHeaderTests
    {
        /// <summary>
        /// Test the ToString method.
        /// </summary>
        [TestMethod]
        public void AccountHeader_ToStringTest()
        {
            AccountHeader ah = new AccountHeader("TestCategory", "TestAccount", Guid.NewGuid());
            Assert.AreEqual<string>("TestAccount", ah.ToString());
        }

        /// <summary>
        /// Test the Equals method.
        /// </summary>
        [TestMethod]
        public void AccountHeader_EqualsTest()
        {
            AccountHeader ah1 = new AccountHeader("TestCategory1", "TestAccount1", Guid.NewGuid());
            AccountHeader ah2 = new AccountHeader("TestCategory2", "TestAccount2", ah1.AccountId);

            Assert.AreEqual<AccountHeader>(ah1, ah2);
            Assert.AreNotEqual<AccountHeader>(null, ah1);
        }
    }
}
