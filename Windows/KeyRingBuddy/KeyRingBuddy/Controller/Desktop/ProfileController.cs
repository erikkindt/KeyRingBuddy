using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using KeyRingBuddy.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Controller for a profile.
    /// </summary>
    public class ProfileController : DesktopControllerBase
    {
        #region Fields

        /// <summary>
        /// The profile that is open.
        /// </summary>
        private IProfile _profile;

        /// <summary>
        /// An account that was just created or edited.
        /// </summary>
        private Account _account;

        /// <summary>
        /// The last category that was selected.
        /// </summary>
        private string _category;

        /// <summary>
        /// Control used when there are no controls.
        /// </summary>
        private CreateItemControl _contentCreateItem = null;

        /// <summary>
        /// Control used for profile.
        /// </summary>
        private ProfileControl _contentProfile = null;

        /// <summary>
        /// Create a new account.
        /// </summary>
        private const string CREATE_ACCOUNT_FUNCTION = "Create new Account";

        /// <summary>
        /// Edit the current profile.
        /// </summary>
        private const string EDIT_PROFILE_FUNCTION = "Edit this Profile";

        /// <summary>
        /// Delete the current profile.
        /// </summary>
        private const string DELETE_PROFILE_FUNCTION = "Delete this Profile";

        #endregion

        #region Methods

        /// <summary>
        /// Get the display name.
        /// </summary>
        /// <returns>The display name.</returns>
        public override string GetDisplayName()
        {
            return _profile.Name;
        }

        /// <summary>
        /// If true, focus is given to the controller when it's opened.  If false, focus will not start on the controller
        /// when it is opened.
        /// </summary>
        /// <returns>true to give focus to the controller when it's opened, false if focus should not be given.</returns>
        public override bool StartWithFocus()
        {
            return false;
        }

        /// <summary>
        /// Register for events.
        /// </summary>
        /// <returns>The events to register for.</returns>
        public override IEnumerable<string> GetRegisteredEventNames()
        {
            return new string[] 
            { 
                ApplicationEventNames.PROFILE_OPENED,
                ApplicationEventNames.PROFILE_CREATED,
                ApplicationEventNames.PROFILE_EDITED,
                ApplicationEventNames.ACCOUNT_EDITED,
                ApplicationEventNames.ACCOUNT_CREATED
            };
        }

        /// <summary>
        /// The functions that are available to this controller.
        /// </summary>
        /// <param name="column">The column to get functions for.</param>
        /// <returns>The functions for the requested column.</returns>
        public override IEnumerable<string> GetFunctionNames(int column)
        {
            switch (column)
            {
                case 0:
                    return new String[] 
                    {
                        CREATE_ACCOUNT_FUNCTION,
                        EDIT_PROFILE_FUNCTION,
                        DELETE_PROFILE_FUNCTION
                    };

                default:
                    return null;
            }
        }

        /// <summary>
        /// Handle function execution.
        /// </summary>
        /// <param name="name">The function that was executed.</param>
        public override void FunctionExecuted(string name)
        {
            switch (name)
            {
                case CREATE_ACCOUNT_FUNCTION:
                    App.DesktopManager.RaiseEvent(ApplicationEventNames.ACCOUNT_CREATE, _profile);
                    break;

                case EDIT_PROFILE_FUNCTION:
                    App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_EDIT, _profile);
                    break;

                case DELETE_PROFILE_FUNCTION:
                    if (App.MessageUser(
                            String.Format("Deleting a Profile is a permanent action that cannot be undone.  Are you sure you want to delete {0}?", _profile.Name),
                            "Delete Profile",
                            MessageBoxImage.Warning,
                            "DELETE", "Cancel") == "DELETE")
                    {
                        App.ProfileManager.DeleteProfile(_profile);
                        App.DesktopManager.RaiseEvent(ApplicationEventNames.STARTED);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Collect arguments.
        /// </summary>
        /// <param name="eventName">The name of the event that was raised.</param>
        /// <param name="arguments">Arguments passed with the event.</param>
        /// <returns>true if the event was processed successfully, false if it was not.</returns>
        public override bool EventRaised(string eventName, object[] arguments)
        {
            switch (eventName)
            {
                case ApplicationEventNames.PROFILE_OPENED:
                case ApplicationEventNames.PROFILE_CREATED:
                case ApplicationEventNames.PROFILE_EDITED:
                    _profile = GetArgument<IProfile>(arguments, 0);
                    break;

                case ApplicationEventNames.ACCOUNT_EDITED:
                case ApplicationEventNames.ACCOUNT_CREATED:
                    _account = GetArgument<Account>(arguments, 1);
                    break;

                default:
                    break;
            }

            bool result = base.EventRaised(eventName, arguments);

            return result;
        }

        /// <summary>
        /// The content to display.
        /// </summary>
        /// <returns>The content to display.</returns>
        public override UIElement GetContent()
        {
            if (_profile.GetAccountHeaders().Count() == 0)
            {
                if (_contentCreateItem == null)
                {
                    _contentCreateItem = new CreateItemControl();
                    _contentCreateItem.CreateCaption = "Create Account";
                    _contentCreateItem.Caption = "Information such as users names and passwords are stored in Accounts within your Profile.  Start creating Accounts to hold all of your sensitive information.";
                    _contentCreateItem.CreateClick += ContentCreateItem_CreateClick;
                }

                return _contentCreateItem;
            }
            else
            {
                if (_contentProfile == null)
                {
                    _contentProfile = new ProfileControl();
                    _contentProfile.SelectedCategoryChanged += ProfileControl_SelectedCategoryChanged;
                    _contentProfile.SelectedAccountChanged += ProfileControl_SelectedAccountChanged;
                    _contentProfile.AccountLaunchClick += ProfileControl_AccountLaunchClick;
                    _contentProfile.AccountEditClick += ProfileControl_AccountEditClick;
                    _contentProfile.AccountDeleteClick += ProfileControl_AccountDeleteClick;
                }

                Account tempAccount = _account;
                string tempCategory = _category;

                _contentProfile.Clear();

                _account = tempAccount;
                _category = tempCategory;

                // setup categories
                foreach (string categoryName in AccountHeader.GetSortedUniqueCategoryNames(_profile, true))
                    _contentProfile.AddCategory(new Item(categoryName, categoryName));

                if (_category != null)
                    _contentProfile.SelectedCategory = new Item(_category, _category);
                else
                    _contentProfile.SelectedCategory = new Item("All", "All");

                // select account
                if (_account != null)
                {
                    _contentProfile.SelectedAccount = new Item(
                        _account.Name,
                        new AccountHeader(_account.Category, _account.Name, _account.Id));
                }

                return _contentProfile;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raise the account create event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ContentCreateItem_CreateClick(object sender, EventArgs e)
        {
            App.DesktopManager.RaiseEvent(ApplicationEventNames.ACCOUNT_CREATE, _profile);
        }

        /// <summary>
        /// Update the accounts displayed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ProfileControl_SelectedCategoryChanged(object sender, EventArgs e)
        {
            _contentProfile.ClearAccounts();
            Item item = _contentProfile.SelectedCategory;
            if (item != null)
            {
                string categoryName = item.Tag as string;
                bool isAll = String.Compare(categoryName, "All", true) == 0;
                List<AccountHeader> headers = new List<AccountHeader>();

                foreach (AccountHeader header in _profile.GetAccountHeaders())
                {
                    if (isAll || String.Compare(categoryName, header.Category, true) == 0)
                        headers.Add(header);
                }

                foreach (AccountHeader header in headers.OrderBy(h => h.AccountName))
                    _contentProfile.AddAccount(new Item(header.AccountName, header));

                _category = categoryName;
            }
            else
            {
                _category = null;
            }
        }

        /// <summary>
        /// Update the account displayed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ProfileControl_SelectedAccountChanged(object sender, EventArgs e)
        {
            Item item = _contentProfile.SelectedAccount;
            if (item != null)
            {
                AccountHeader header = item.Tag as AccountHeader;
                _account = _profile.GetAccount(header.AccountId);

                _contentProfile.AccountName = _account.Name;
                _contentProfile.AccountCategory = _account.Category;
                _contentProfile.AccountSite = _account.Site;
                _contentProfile.AccountDetails = _account.Details;
            }
            else
            {
                _contentProfile.AccountName = null;
                _account = null;
            }
        }

        /// <summary>
        /// Launch the current account.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public void ProfileControl_AccountLaunchClick(object sender, EventArgs e)
        {
            Item item = _contentProfile.SelectedAccount;
            if (item != null)
            {
                AccountHeader header = item.Tag as AccountHeader;
                Account account = _profile.GetAccount(header.AccountId);

                if (!String.IsNullOrWhiteSpace(account.Site))
                    System.Diagnostics.Process.Start(account.Site);

                List<string> details = new List<string>();
                foreach (AccountDetail detail in account.Details)
                    if (!String.IsNullOrEmpty(detail.Value))
                        details.Add(detail.Value);

                if (details.Count > 0)
                    ClipboardStackInjector.Inject(details, TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Edit the current account.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public void ProfileControl_AccountEditClick(object sender, EventArgs e)
        {
            Item item = _contentProfile.SelectedAccount;
            if (item != null)
            {
                AccountHeader header = item.Tag as AccountHeader;
                Account account = _profile.GetAccount(header.AccountId);

                App.DesktopManager.RaiseEvent(ApplicationEventNames.ACCOUNT_EDIT, _profile, account);
            }
        }

        /// <summary>
        /// Delete the current account.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ProfileControl_AccountDeleteClick(object sender, EventArgs e)
        {
            Item item = _contentProfile.SelectedAccount;
            if (item != null)
            {
                AccountHeader header = item.Tag as AccountHeader;

                if (App.MessageUser(
                    String.Format("Deleting an Account is a permanent action that cannot be undone.  Are you sure you want to delete {0}?", header.AccountName),
                    "Delete Account",
                    MessageBoxImage.Warning,
                    "DELETE", "Cancel") == "DELETE")
                {
                    // remove account
                    _profile.DeleteAccount(header.AccountId);
                    _contentProfile.RemoveAccount(item);

                    // remove category if there are no accounts left in it
                    if (String.Compare(header.Category, "All", true) != 0)
                    {
                        bool foundHeader = false;
                        foreach (AccountHeader h in _profile.GetAccountHeaders())
                        {
                            if (String.Compare(header.Category, h.Category, true) == 0)
                            {
                                foundHeader = true;
                                break;
                            }
                        }

                        if (!foundHeader)
                        {
                            _contentProfile.RemoveCategory(new Item(header.Category, header.Category));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
