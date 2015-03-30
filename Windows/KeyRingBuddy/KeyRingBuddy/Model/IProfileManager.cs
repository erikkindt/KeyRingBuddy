using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// Defines a profile manager.
    /// </summary>
    public interface IProfileManager
    {
        /// <summary>
        /// Get all of the profiles.
        /// </summary>
        /// <returns>All profiles.</returns>
        IProfile[] GetProfiles();

        /// <summary>
        /// Create a new profile.
        /// </summary>
        /// <returns>The newly created profile.</returns>
        IProfile CreateProfile();

        /// <summary>
        /// Delete a profile.
        /// </summary>
        /// <param name="profile">The profile to delete.</param>
        void DeleteProfile(IProfile profile);
    }
}
