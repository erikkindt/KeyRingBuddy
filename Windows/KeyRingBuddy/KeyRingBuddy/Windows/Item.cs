using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// An item that can be displayed.
    /// </summary>
    public class Item
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="tag">Tag.</param>
        public Item(string name, object tag)
        {
            Name = name;
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        public Item(string name)
            : this(name, null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The item name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// An object associated with the item.
        /// </summary>
        public object Tag { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the Name property.
        /// </summary>
        /// <returns>The Name property.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Base equality on the Tag property or the Name property if Tags are null.
        /// </summary>
        /// <param name="obj">The object to compare with this one.</param>
        /// <returns>true if the other object is logically equal to this one, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            Item other = obj as Item;
            if (other == null)
                return false;

            if (this.Tag != null || other.Tag != null)
                return Object.Equals(this.Tag, other.Tag);

            return String.Compare(other.Name, this.Name, true) == 0;
        }

        /// <summary>
        /// The hash code is based on the Tag property or the Name property if Tag is null.
        /// </summary>
        /// <returns>A hash code for this object.</returns>
        public override int GetHashCode()
        {
            if (Tag != null)
                return Tag.GetHashCode();

            if (Name != null)
                return Name.ToUpper().GetHashCode();

            return 0;
        }

        #endregion
    }
}
