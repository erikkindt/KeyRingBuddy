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

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Interaction logic for NavigationBackControl.xaml
    /// </summary>
    public partial class NavigationBackButtonControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public NavigationBackButtonControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raise the Click event on this object.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Update the foreground based on the control being enabled or not.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            button.Foreground = IsEnabled ? FindResource("LightPlusBrush") as Brush : Brushes.Gray;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event that is raised when a tile is clicked.
        /// </summary>
        public event EventHandler Click;

        #endregion
    }
}
