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
using System.Windows.Controls;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Displays accounts for the user to select from to open.
    /// </summary>
    public class ProfileSelectController : DesktopControllerBase
    {
        #region Fields

        /// <summary>
        /// Holds the content that is displayed if there are profiles.
        /// </summary>
        private TileSelectControl _contentTileSelect = null;

        /// <summary>
        /// Holds the content displayed if there are no profiles.
        /// </summary>
        private CreateItemControl _contentCreateItem = null;

        /// <summary>
        /// A function to create a new profile.
        /// </summary>
        private const string CREATE_PROFILE_FUNCTION = "Create new Profile";

        #endregion

        #region Methods

        /// <summary>
        /// The name displayed for navigation.
        /// </summary>
        /// <returns>The display name.</returns>
        public override string GetDisplayName()
        {
            return "Profiles";
        }

        /// <summary>
        /// Register for events.
        /// </summary>
        /// <returns>The events to register for.</returns>
        public override IEnumerable<string> GetRegisteredEventNames()
        {
            return new string[] 
            { 
                ApplicationEventNames.STARTED
            };
        }

        /// <summary>
        /// Define functions.
        /// </summary>
        /// <param name="column">The column for the functions.</param>
        /// <returns>The functions for the given column.</returns>
        public override IEnumerable<string> GetFunctionNames(int column)
        {
            switch (column)
            {
                case 0:
                    return new string[]
                    {
                        CREATE_PROFILE_FUNCTION
                    };

                default:
                    return null;
            }
        }

        /// <summary>
        /// Execute a function.
        /// </summary>
        /// <param name="name">The function to execute.</param>
        public override void FunctionExecuted(string name)
        {
            switch (name)
            {
                case CREATE_PROFILE_FUNCTION:
                    App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_CREATE);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Get the content for the view.
        /// </summary>
        /// <returns>The content for the view.</returns>
        public override UIElement GetContent()
        {
            IProfile[] profiles = App.ProfileManager.GetProfiles();

            if (profiles.Length == 0)
            {
                if (_contentTileSelect != null)
                {
                    _contentTileSelect.TileClick -= TileSelectControl_TileClick;
                    _contentTileSelect = null;
                }

                if (_contentCreateItem == null)
                {
                    _contentCreateItem = new CreateItemControl();
                    _contentCreateItem.Caption = String.Format("Profiles are used to group together and store Account information.{0}Get started by creating a new Profile.", Environment.NewLine);
                    _contentCreateItem.CreateCaption = "Create Profile";
                    _contentCreateItem.CreateClick += CreateItemControl_CreateClick;
                }

                return _contentCreateItem;
            }
            else
            {
                if (_contentCreateItem != null)
                {
                    _contentCreateItem.CreateClick -= CreateItemControl_CreateClick;
                    _contentCreateItem = null;
                }

                if (_contentTileSelect == null)
                {
                    _contentTileSelect = new TileSelectControl();
                    _contentTileSelect.Caption = "Select a profile to open";
                }

                _contentTileSelect.ClearTiles();

                foreach (IProfile profile in profiles)
                    _contentTileSelect.AddTile(new TileControl(profile.Name, new DefaultProfileImageControl(), profile));
                _contentTileSelect.TileClick += TileSelectControl_TileClick;

                return _contentTileSelect;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Open the login for the profile.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void TileSelectControl_TileClick(object sender, TileControlEventArgs e)
        {
            if (e.Tile.Tag is IProfile)
                App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_OPEN, e.Tile.Tag as IProfile);
        }

        /// <summary>
        /// Open the profile create view.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void CreateItemControl_CreateClick(object sender, EventArgs e)
        {
            App.DesktopManager.RaiseEvent(ApplicationEventNames.PROFILE_CREATE);
        }

        #endregion
    }
}
