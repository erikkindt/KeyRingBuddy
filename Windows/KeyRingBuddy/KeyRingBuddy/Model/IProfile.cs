using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Model
{
    /// <summary>
    /// A profile is a stored collection of accounts.
    /// </summary>
    public interface IProfile
    {
        #region Methods

        /// <summary>
        /// The id for the profile.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// The name for the profile.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Update the name for the profile.
        /// </summary>
        /// <param name="name"></param>
        void UpdateName(string name);

        /// <summary>
        /// Set the password to use for the profile.
        /// </summary>
        /// <param name="password">The password for the profile.</param>
        /// <returns>
        /// true if the password is set correctly.  
        /// false if the password is not set because there is an existing password that doesn't match.
        /// </returns>
        bool SetPassword(SecureString password);

        /// <summary>
        /// Update the password for the profile.
        /// </summary>
        /// <param name="password">The new password for the profile.</param>
        void UpdatePassword(SecureString password);

        /// <summary>
        /// Get headers for all of the accounts in this profile.
        /// </summary>
        /// <returns>The headers for all of the accounts in this profile.</returns>
        IEnumerable<AccountHeader> GetAccountHeaders();

        /// <summary>
        /// Get the account with the given id.
        /// </summary>
        /// <param name="id">The id of the account to get.</param>
        /// <returns>The id or null if there is no account with the given id.</returns>
        Account GetAccount(Guid id);

        /// <summary>
        /// Add a new account.
        /// </summary>
        /// <param name="account">The account to add.</param>
        void AddAccount(Account account);

        /// <summary>
        /// Update the given account.
        /// </summary>
        /// <param name="account">The account to update.</param>
        void UpdateAccount(Account account);

        /// <summary>
        /// Delete the given account.
        /// </summary>
        /// <param name="id">The id of the account to delete.</param>
        void DeleteAccount(Guid id);

        #endregion
    }
}
