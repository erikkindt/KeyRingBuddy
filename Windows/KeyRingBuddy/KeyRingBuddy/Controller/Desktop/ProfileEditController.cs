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
    /// Controller used to edit or create new profiles.
    /// </summary>
    public class ProfileEditController : DesktopControllerBase
    {
        #region Fields

        /// <summary>
        /// The content for this controller.
        /// </summary>
        private ProfileEditControl _content = null;

        /// <summary>
        /// Indicates if a profile has been saved.
        /// </summary>
        private bool _saved = false;

        /// <summary>
        /// Holds the profile when in edit mode.
        /// </summary>
        private IProfile _profile = null;

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
            return (_profile == null) ? "Create Profile" : "Edit Profile";
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
                ApplicationEventNames.PROFILE_CREATE,
                ApplicationEventNames.PROFILE_EDIT
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
            if (eventName == ApplicationEventNames.PROFILE_EDIT)
                _profile = GetArgument<IProfile>(arguments, 0);
            else
                _profile = null;

            return base.EventRaised(eventName, arguments);
        }

        /// <summary>
        /// Get the content to display.
        /// </summary>
        /// <returns>The content to display.</returns>
        public override System.Windows.UIElement GetContent()
        {
            if (_content == null)
            {
                _content = new ProfileEditControl();
                _content.SaveClick += ProfileCreateControl_SaveClick;
                _content.CancelClick += ProfileCreateControl_CancelClick;
            }

            _content.ClearInput();
            _saved = false;

            if (_profile != null)
            {
                _content.Caption = String.Format("Edit {0}", _profile.Name);
                _content.ProfileName = _profile.Name;
            }
            else
            {
                _content.Caption = "Create a new Profile";
            }

            _content.IsModified = false;

            return _content;
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
                    "You have unsaved Profile changes.  Are you sure you want to continue?",
                    "Unsaved Profile",
                    System.Windows.MessageBoxImage.Warning,
                    "Yes", "No") == "Yes";
            }

            _saved = false;
            return true;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Validate and create a new profile.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ProfileCreateControl_SaveClick(object sender, EventArgs e)
        {
            if (_content == null)
                return;

            bool isValid = true;
            _content.SetProfileNameErrorMessage(null);
            _content.SetProfilePasswordErrorMessage(null);
            _content.SetProfilePasswordReEnterErrorMessage(null);

            if (String.IsNullOrWhiteSpace(_content.ProfileName))
            {
                _content.SetProfileNameErrorMessage("! A profile name is required");
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(_content.ProfilePassword))
            {
                _content.SetProfilePasswordErrorMessage("! A password is required");
                isValid = false;
            }
            else if (_content.ProfilePassword != _content.ProfilePasswordReEnter)
            {
                _content.SetProfilePasswordReEnterErrorMessage("! The value you have entered doesn't match the password provided above");
                isValid = false;
            }

            if (isValid)
            {
                if (_profile == null)
                {
                    _profile = App.ProfileManager.CreateProfile();
                    _profile.UpdateName(_content.ProfileName);
                    _profile.SetPassword(SecureStringUtility.CreateSecureString(_content.ProfilePassword));

                    _saved = true;

                    App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_CREATED, _profile);
                }
                else
                {
                    _profile.UpdateName(_content.ProfileName);
                    _profile.UpdatePassword(SecureStringUtility.CreateSecureString(_content.ProfilePassword));

                    _saved = true;

                    App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_EDITED, _profile);
                }
            }
        }

        /// <summary>
        /// Go back to the previous screen.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ProfileCreateControl_CancelClick(object sender, EventArgs e)
        {
            App.DesktopManager.Back();
        }

        #endregion
    }
}
