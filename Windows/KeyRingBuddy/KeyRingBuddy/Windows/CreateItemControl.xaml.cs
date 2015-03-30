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
    /// Interaction logic for CreateItemControl.xaml
    /// </summary>
    public partial class CreateItemControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateItemControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The caption for the view.
        /// </summary>
        public string Caption
        {
            get { return textBlockCaption.Text; }
            set { textBlockCaption.Text = value; }
        }

        /// <summary>
        /// The caption for the create button.
        /// </summary>
        public string CreateCaption
        {
            get { return buttonCreate.Content as string; }
            set { buttonCreate.Content = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the CreateClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnCreateClick(EventArgs e)
        {
            if (CreateClick != null)
                CreateClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the CreateClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            OnCreateClick(EventArgs.Empty);
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the user clicks the create button.
        /// </summary>
        public event EventHandler CreateClick;

        #endregion
    }
}
