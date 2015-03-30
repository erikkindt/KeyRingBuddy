using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using KeyRingBuddy.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Test the SecureStringUtility class.
    /// </summary>
    [TestClass]
    public class SecureStringUtilityTests
    {
        /// <summary>
        /// Test the GetSecureString and CreateSecureString methods.
        /// </summary>
        [TestMethod]
        public void SecureStringUtility_GetAndCreateSecureStringTest()
        {
            string text = "my example password";
            SecureString secureString = SecureStringUtility.CreateSecureString(text);
            string newText = SecureStringUtility.GetPlainText(secureString);

            Assert.AreEqual(text, newText, "strings are not equal.");
        }
    }
}
