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
using System.Windows.Media.Imaging;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// This is a helper class for things like loading images.
    /// </summary>
    public class VisualHelper
    {
        #region Fields

        /// <summary>
        /// Holds images that have been loaded.
        /// </summary>
        private static Dictionary<string, BitmapImage> _imageMap = new Dictionary<string, BitmapImage>();

        #endregion

        #region Methods

        /// <summary>
        /// Load the given bitmap.
        /// </summary>
        /// <param name="name">The name of the bitmap to load.</param>
        /// <returns>The loaded bitmap.</returns>
        public static BitmapImage LoadBitmap(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            string key = name.Trim().ToLower();
            if (_imageMap.ContainsKey(key))
                return _imageMap[key];

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(String.Format(@"pack://application:,,,/Resources/{0}", name));
            bitmap.EndInit();

            _imageMap.Add(key, bitmap);

            return bitmap;
        }

        #endregion
    }
}
