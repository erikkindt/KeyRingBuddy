using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Event arguments involving a TileControl.
    /// </summary>
    public class TileControlEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tile">Tile.</param>
        public TileControlEventArgs(TileControl tile)
        {
            if (tile == null)
                throw new ArgumentNullException("tile");

            Tile = tile;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The tile involved in the event.
        /// </summary>
        public TileControl Tile { get; private set; }

        #endregion
    }
}
