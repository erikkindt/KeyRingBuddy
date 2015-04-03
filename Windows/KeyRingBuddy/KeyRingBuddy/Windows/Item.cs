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
using System.Windows.Media;

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
        /// <param name="icon">Icon.</param>
        /// <param name="tag">Tag.</param>
        public Item(string name, ImageSource icon, object tag)
        {
            Name = name;
            Icon = icon;
            Tag = tag;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        public Item(string name)
            : this(name, null, null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The item name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The item icon.
        /// </summary>
        public ImageSource Icon { get; private set; }

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
