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
