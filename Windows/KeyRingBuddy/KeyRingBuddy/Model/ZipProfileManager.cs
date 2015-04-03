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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// An implementation of IProfileManager that uses a local folder as it's store.
    /// </summary>
    public class ZipProfileManager : IProfileManager
    {
        #region Fields

        /// <summary>
        /// The folder that profiles are stored in.
        /// </summary>
        public string _folder { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="folder">The folder that profiles are stored in.</param>
        public ZipProfileManager(string folder)
        {
            if (String.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("folder is null or whitespace", "folder");

            _folder = folder;

            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all profiles that are in the folder.
        /// </summary>
        /// <returns>All profiles that are in the folder.</returns>
        public IProfile[] GetProfiles()
        {
            List<IProfile> result = new List<IProfile>();
            foreach (string file in Directory.GetFiles(_folder, "*.profile"))
                result.Add(new ZipProfile(file));

            return result.ToArray();
        }

        /// <summary>
        /// Create a new profile.
        /// </summary>
        /// <returns>The newly created profile.</returns>
        public IProfile CreateProfile()
        {
            string file = null;
            do
            {
                file = Path.Combine(_folder, String.Format("{0}.profile", Guid.NewGuid().ToString("N")));
            }
            while (File.Exists(file));

            return new ZipProfile(file);
        }

        /// <summary>
        /// Delete a profile.
        /// </summary>
        /// <param name="profile">The profile to delete.</param>
        public void DeleteProfile(IProfile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            ZipProfile zipProfile = profile as ZipProfile;
            if (zipProfile == null)
                throw new ArgumentNullException("Only ZipProfile objects can be deleted using this method.");

            if (File.Exists(zipProfile.FilePath))
                File.Delete(zipProfile.FilePath);
        }

        #endregion
    }
}
