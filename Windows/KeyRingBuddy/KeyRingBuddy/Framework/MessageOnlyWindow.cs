using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using KeyRingBuddy.Framework.Win32;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Window that is used only for message communication.
    /// </summary>
    public partial class MessageOnlyWindow : Window
    {
        #region Properties

        /// <summary>
        /// This windows source.
        /// </summary>
        public HwndSource WindowSource { get; private set; }

        /// <summary>
        /// This windows handle.
        /// </summary>
        public IntPtr WindowHandle { get; private set; }

        /// <summary>
        /// Holds the original parent of this window.
        /// </summary>
        private IntPtr? _originalParent;

        #endregion

        #region Methods

        /// <summary>
        /// Setup this window as a message only window.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            WindowSource = PresentationSource.FromVisual(this) as HwndSource;
            if (WindowSource != null)
            {
                WindowHandle = WindowSource.Handle;
                _originalParent = User32.SetParent(WindowHandle, (IntPtr)User32.HWND_MESSAGE);
                Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Enable visibility on this window again.
        /// </summary>
        protected void EnableVisibility()
        {
            if (_originalParent.HasValue)
            {
                User32.SetParent(WindowHandle, _originalParent.Value);
                _originalParent = null;
            }
        }        

        #endregion
    }
}
