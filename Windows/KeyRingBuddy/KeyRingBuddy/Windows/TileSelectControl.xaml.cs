using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for TileSelectControl.xaml
    /// </summary>
    public partial class TileSelectControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        public TileSelectControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The caption to display.
        /// </summary>
        public string Caption
        {
            get { return textBlockCaption.Text; }
            set { textBlockCaption.Text = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new tile to the display.
        /// </summary>
        /// <param name="tile">The tile to add.</param>
        public void AddTile(TileControl tile)
        {
            if (tile == null)
                throw new ArgumentNullException("tile");

            tile.Click += TileControl_Click;

            wrapPanelProfiles.Children.Add(tile);
        }

        /// <summary>
        /// Clear all of the tiles.
        /// </summary>
        public void ClearTiles()
        {
            while (wrapPanelProfiles.Children.Count > 0)
            {
                (wrapPanelProfiles.Children[0] as TileControl).Click -= TileControl_Click;
                wrapPanelProfiles.Children.RemoveAt(0);
            }
        }

        /// <summary>
        /// Raises the TileClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnTileClick(TileControlEventArgs e)
        {
            if (TileClick != null)
                TileClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raise the TileClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void TileControl_Click(object sender, EventArgs e)
        {
            TileControl tile = sender as TileControl;
            if (tile != null)
                OnTileClick(new TileControlEventArgs(tile));
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when a user clicks a tile.
        /// </summary>
        public event EventHandler<TileControlEventArgs> TileClick;

        #endregion
    }
}
