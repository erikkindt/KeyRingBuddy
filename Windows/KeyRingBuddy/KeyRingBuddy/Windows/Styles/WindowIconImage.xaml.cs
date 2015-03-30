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

namespace KeyRingBuddy.Windows.Styles
{
    /// <summary>
    /// Interaction logic for WindowIconImage.xaml
    /// </summary>
    public partial class WindowIconImage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WindowIconImage()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set icon displayed.
        /// </summary>
        /// <param name="oldParent">Not used.</param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                if (parentWindow.Icon != null)
                {
                    BitmapFrame frame = parentWindow.Icon as BitmapFrame;
                    if (frame != null)
                    {
                        BitmapFrame imageFrame = GetFrame(frame, 24, 24, 32);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 24, 24, 24);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 24, 24, -1);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 32, 32, 32);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 32, 32, 24);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 32, 32, -1);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 16, 16, 32);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 16, 16, 24);
                        if (imageFrame == null)
                            imageFrame = GetFrame(frame, 16, 16, -1);

                        imageIcon.Source = imageFrame;
                    }
                }
                else
                {
                    this.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Get the frame requested.
        /// </summary>
        /// <param name="image">The image to get the frame from.</param>
        /// <param name="height">The height o the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="bitsPerPixel">The resolution of the frame.</param>
        /// <returns>The requested frame or null if it doesn't exist.</returns>
        private BitmapFrame GetFrame(BitmapFrame image, int height, int width, int bitsPerPixel)
        {
            return image.Decoder.Frames.First(f => f.PixelHeight == height &&
                                                   f.PixelWidth == width &&
                                                   (bitsPerPixel == -1 || f.Format.BitsPerPixel == bitsPerPixel));
        }

        #endregion
    }
}
