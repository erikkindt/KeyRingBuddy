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
