using KeyRingBuddy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Test class for ZipProfileManager.
    /// </summary>
    [TestClass]
    public class ZipProfileManagerTests
    {
        /// <summary>
        /// Test the manager
        /// </summary>
        [TestMethod]
        public void ZipProfileManager_Test()
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "ZipProfileManagerTest");
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            ZipProfileManager manager = new ZipProfileManager(folder);
            Assert.AreEqual<int>(0, manager.GetProfiles().Length, "manager isn't empty when it should be.");

            IProfile profile = manager.CreateProfile();
            Assert.AreEqual<int>(1, manager.GetProfiles().Length, "manager should have the newly created profile.");

            profile.UpdateName("MyProfile");

            manager = new ZipProfileManager(folder);
            Assert.AreEqual<int>(1, manager.GetProfiles().Length, "manager should have the profile that was created earlier.");

            profile = manager.GetProfiles()[0];
            Assert.AreEqual<string>("MyProfile", profile.Name, "the profile name doesn't match.");
        }
    }
}
