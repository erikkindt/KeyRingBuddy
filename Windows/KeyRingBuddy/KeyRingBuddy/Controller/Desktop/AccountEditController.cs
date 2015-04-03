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

using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using KeyRingBuddy.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Controller used to create or edit accounts.
    /// </summary>
    public class AccountEditController : DesktopControllerBase
    {
        #region Fields

        /// <summary>
        /// The content for this controller.
        /// </summary>
        private AccountEditControl _content = null;

        /// <summary>
        /// The profile the account belongs to.
        /// </summary>
        private IProfile _profile;

        /// <summary>
        /// The account being edited or null if a new account is being created.
        /// </summary>
        private Account _account;

        /// <summary>
        /// Indicates if a profile has been saved.
        /// </summary>
        private bool _saved = false;

        /// <summary>
        /// Function to generate a new password.
        /// </summary>
        private const string GENERATE_PASSWORD = "Generate a new Password";

        #endregion

        #region Methods

        /// <summary>
        /// Get the display name.
        /// </summary>
        /// <returns>The display name.</returns>
        public override string GetDisplayName()
        {
            return (_account == null) ? "Create Account" : "Edit Account";
        }

        /// <summary>
        /// The controller can't be returned to.
        /// </summary>
        /// <returns>false.</returns>
        public override bool CanRestore()
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
                ApplicationEventNames.ACCOUNT_CREATE,
                ApplicationEventNames.ACCOUNT_EDIT
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
                        GENERATE_PASSWORD
                    };

                default:
                    return null;
            }
        }

        /// <summary>
        /// Handle the event.
        /// </summary>
        /// <param name="eventName">The name of the event that was fired.</param>
        /// <param name="arguments">Arguments for the event.</param>
        /// <returns>true if the event was processed ok, false if it wasn't.</returns>
        public override bool EventRaised(string eventName, object[] arguments)
        {
            _profile = GetArgument<IProfile>(arguments, 0);
            if (eventName == ApplicationEventNames.ACCOUNT_EDIT)
                _account = GetArgument<Account>(arguments, 1);
            else
                _account = null;

            return base.EventRaised(eventName, arguments);
        }

        /// <summary>
        /// Prompt user if they haven't saved changes.
        /// </summary>
        /// <returns>true if the it's ok to close this controller, false if it isn't.</returns>
        public override bool Closing()
        {
            if (!_saved && _content.IsModified)
            {
                return App.MessageUser(
                    "You have unsaved Account changes.  Are you sure you want to continue?",
                    "Unsaved Account",
                    System.Windows.MessageBoxImage.Warning,
                    "Yes", "No") == "Yes";
            }

            _saved = false;
            return true;
        }

        /// <summary>
        /// Get the content to display.
        /// </summary>
        /// <returns>The content to display.</returns>
        public override System.Windows.UIElement GetContent()
        {
            if (_content == null)
            {
                _content = new AccountEditControl();
                _content.SaveClick += AccountEditControl_SaveClick;
                _content.CancelClick += AccountEditControl_CancelClick;
                _content.RefreshIconClick += AccountEditControl_RefreshIconClick;
            }

            _content.ClearInput();
            _content.SetCategories(AccountHeader.GetSortedUniqueCategoryNames(_profile, true));

            if (_account == null)
            {
                _content.Caption = "Create a new Account";
                _content.AddDetail(new AccountDetail("Username", null));
                _content.AddDetail(new AccountDetail("Password", null));
                _content.AccountIcon = FavIcon.SmallDefaultIcon;
                _content.AccountIconTag = new FavIcon();
            }
            else
            {
                _content.Caption = String.Format("Edit {0}", _account.Name);
                _content.AccountName = _account.Name;
                _content.AccountCategory = _account.Category;
                _content.AccountSite = _account.Site;
                _content.AccountIcon = _account.Icon.SmallIconOrDefault;

                foreach (AccountDetail detail in _account.Details)
                    _content.AddDetail(detail);
            }
            
            _saved = false;
            _content.IsModified = false;

            return _content;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Save the modified account.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void AccountEditControl_SaveClick(object sender, EventArgs e)
        {
            if (_content == null)
                return;

            bool isValid = true;
            _content.SetAccountNameErrorMessage(null);
            _content.SetAccountSiteErrorMessage(null);
            _content.SetAccountCategoryErrorMessage(null);

            if (String.IsNullOrWhiteSpace(_content.AccountName))
            {
                _content.SetAccountNameErrorMessage("! An account name is required");
                isValid = false;
            }

            if (isValid)
            {
                if (_account == null)
                {
                    _account = new Account(
                        _content.AccountName,
                        _content.AccountCategory,
                        _content.AccountSite);

                    foreach (AccountDetail detail in _content.GetDetails())
                        _account.Details.Add(detail);

                    _account.Icon = _content.AccountIconTag as FavIcon;

                    _profile.AddAccount(_account);
                    _saved = true;

                    App.DesktopManager.RaiseEvent(ApplicationEventNames.ACCOUNT_CREATED, _profile, _account);
                }
                else
                {
                    _account.Name = _content.AccountName;
                    _account.Category = _content.AccountCategory;
                    _account.Site = _content.AccountSite;

                    _account.Details.Clear();
                    foreach (AccountDetail detail in _content.GetDetails())
                        _account.Details.Add(detail);

                    _account.Icon = _content.AccountIconTag as FavIcon;

                    _profile.UpdateAccount(_account);
                    _saved = true;

                    App.DesktopManager.RaiseEvent(ApplicationEventNames.ACCOUNT_EDITED, _profile, _account);
                }
            }
        }

        /// <summary>
        /// Go back to the previous screen.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void AccountEditControl_CancelClick(object sender, EventArgs e)
        {
            App.DesktopManager.Back();
        }

        /// <summary>
        /// Download a new icon for the account.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void AccountEditControl_RefreshIconClick(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(_content.AccountSite))
            {
                try
                {
                    System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    FavIcon icon = FavIcon.Download(_content.AccountSite);
                    _content.AccountIcon = icon.SmallIconOrDefault;
                    _content.AccountIconTag = icon;
                }
                finally
                {
                    System.Windows.Input.Mouse.OverrideCursor = null;
                }
            }
        }

        #endregion
    }
}
