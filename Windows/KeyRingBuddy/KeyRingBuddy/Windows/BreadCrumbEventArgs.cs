using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Event args involving a bread crumb.
    /// </summary>
    public class BreadCrumbEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="breadCrumb">BreadCrumb.</param>
        public BreadCrumbEventArgs(BreadCrumb breadCrumb)
        {
            if (breadCrumb == null)
                throw new ArgumentNullException("breadCrumb");

            BreadCrumb = breadCrumb;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The bread crumb involved in the event.
        /// </summary>
        public BreadCrumb BreadCrumb { get; private set; }

        #endregion
    }
}
