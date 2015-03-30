using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// A bread crumb.
    /// </summary>
    public class BreadCrumb
    {
        #region Properties

        /// <summary>
        /// The name for the bread crumb.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// An object linked to the bread crumb.
        /// </summary>
        public object Tag { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="tag">Tag.</param>
        public BreadCrumb(string name, object tag)
        {
            Name = name;
            Tag = tag;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Use the Tag property to check for equality.
        /// </summary>
        /// <param name="obj">The object to compare with this one.</param>
        /// <returns>true if the object is logically equal to this one, false if it isn't.</returns>
        public override bool Equals(object obj)
        {
            if (obj is BreadCrumb)
                return Object.Equals((obj as BreadCrumb).Tag, this.Tag);
            else
                return false;
        }

        /// <summary>
        /// Use the Tag properties hash code.
        /// </summary>
        /// <returns>The Tag properties hash code.</returns>
        public override int GetHashCode()
        {
            return (Tag == null) ? 0 : Tag.GetHashCode();
        }

        /// <summary>
        /// The Name property is returned.
        /// </summary>
        /// <returns>The Name property.</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
