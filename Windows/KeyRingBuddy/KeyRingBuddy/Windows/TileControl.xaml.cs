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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Interaction logic for TileControl.xaml
    /// </summary>
    public partial class TileControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TileControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="tag">Tag.</param>
        public TileControl(string text, UIElement icon, object tag)
            : this()
        {
            Text = text;
            Icon = icon;
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="tag">Tag.</param>
        public TileControl(string text, object tag)
            : this(text, null, tag)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The icon for this tile.
        /// </summary>
        public UIElement Icon
        {
            get { return borderIcon.Child; }
            set { borderIcon.Child = value; }
        }

        /// <summary>
        /// The text displayed.
        /// </summary>
        public string Text
        {
            get { return textBoxText.Text; }
            set 
            { 
                textBoxText.Text = value;
                textBoxText.ToolTip = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raise the Click event on this object.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClick(EventArgs.Empty);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event that is raised when a tile is clicked.
        /// </summary>
        public event EventHandler Click;

        #endregion
    }
}
