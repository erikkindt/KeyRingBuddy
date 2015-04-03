/*
 * Copyright (c) 2015 Nathaniel Wallace
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

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
