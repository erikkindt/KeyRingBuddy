using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Defines the view for the desktop window.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Called when the controller is being closed.
        /// </summary>
        /// <returns>true if it's ok to close the controller, false if it is not.</returns>
        bool Closing();

        /// <summary>
        /// Called when the control is closed.
        /// </summary>
        void Closed();

        /// <summary>
        /// Called when the control is opened.
        /// </summary>
        void Opened();
    }
}
