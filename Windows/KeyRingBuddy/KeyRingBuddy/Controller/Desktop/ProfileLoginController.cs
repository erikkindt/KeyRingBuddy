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
using System.Windows;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Log into an account.
    /// </summary>
    public class ProfileLoginController : DesktopControllerBase
    {
        #region Fields

        /// <summary>
        /// The profile being logged into.
        /// </summary>
        private IProfile _profile = null;

        /// <summary>
        /// The content for this controller.
        /// </summary>
        private LoginControl _content = null;

        #endregion

        #region Methods

        /// <summary>
        /// Get the display name.
        /// </summary>
        /// <returns>The display name.</returns>
        public override string GetDisplayName()
        {
            return "Login";
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
            return new string[] { ApplicationEventNames.PROFILE_OPEN };
        }

        /// <summary>
        /// Respond to the registered events.
        /// </summary>
        /// <param name="eventName">The name of the event raised.</param>
        /// <param name="arguments">Arguments for the event.</param>
        /// <returns>true if the event was processed successfully, false otherwise.</returns>
        public override bool EventRaised(string eventName, object[] arguments)
        {
            _profile = GetArgument<IProfile>(arguments, 0);
            return App.DesktopManager.LoadController(this);
        }

        /// <summary>
        /// Get the content for the view.
        /// </summary>
        /// <returns>The content for the view.</returns>
        public override UIElement GetContent()
        {
            if (_content == null)
            {
                _content = new LoginControl();
                _content.Icon = new DefaultProfileImageControl();
                _content.Caption = String.Format("Enter password for{0}{1}", Environment.NewLine, _profile.Name);
                _content.LoginClick += LoginControl_LoginClick;
            }

            _content.Password = null;

            return _content;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Login to the profile.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void LoginControl_LoginClick(object sender, EventArgs e)
        {
            if (!_profile.SetPassword(SecureStringUtility.CreateSecureString(_content.Password)))
            {
                _content.SetErrorMessage("Failed to unlock profile.  Please check that the password you entered is correct.");
            }
            else
            {
                App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_OPENED, _profile);
            }
        }

        #endregion
    }
}
