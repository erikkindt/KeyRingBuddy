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
    /// Interaction logic for BreadCrumbControl.xaml
    /// </summary>
    public partial class BreadCrumbControl : UserControl
    {
        #region Fields

        /// <summary>
        /// Holds links to the buttons.
        /// </summary>
        private IDictionary<BreadCrumb, Button> _breadCrumbButtons;

        /// <summary>
        /// Holds links to the panels.
        /// </summary>
        private IDictionary<BreadCrumb, StackPanel> _breadCrumbPanels;

        /// <summary>
        /// Holds all buttons in order.
        /// </summary>
        private IList<Button> _buttonList;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        public BreadCrumbControl()
        {
            InitializeComponent();

            _breadCrumbButtons = new Dictionary<BreadCrumb, Button>();
            _breadCrumbPanels = new Dictionary<BreadCrumb, StackPanel>();
            _buttonList = new List<Button>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The top most (or right most) bread crumb.
        /// </summary>
        public BreadCrumb TopBreadCrumb
        {
            get
            {
                if (_buttonList.Count > 0)
                    return _buttonList[_buttonList.Count - 1].Tag as BreadCrumb;
                else
                    return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Move back one in the list of bread crumbs.  This method is what's called when the user clicks the
        /// back button.
        /// </summary>
        public void Back()
        {
            if (_buttonList.Count > 1)
                OnBreadCrumbClick(new BreadCrumbEventArgs(_buttonList[_buttonList.Count - 2].Tag as BreadCrumb));
        }

        /// <summary>
        /// Add a bread crumb to the view.
        /// </summary>
        /// <param name="breadCrumb">The breadCrumb to add.</param>
        public void AddBreadCrumb(BreadCrumb breadCrumb)
        {
            if (breadCrumb == null)
                throw new ArgumentNullException("breadCrumb");

            if (_breadCrumbButtons.ContainsKey(breadCrumb))
                return;

            Button b = new Button();
            b.Content = breadCrumb.Name;
            b.FontSize = 18;
            b.FontWeight = FontWeights.Medium;
            b.Padding = new Thickness(5);
            b.Margin = new Thickness(5);
            b.VerticalAlignment = VerticalAlignment.Center;
            b.IsEnabled = false;
            b.Foreground = FindResource("LightPlusBrush") as Brush;
            b.Tag = breadCrumb;
            b.Click += BreadCrumb_Click;
            _buttonList.Add(b);

            TextBlock tb = new TextBlock();
            tb.Text = "/";
            tb.FontSize = 18;
            tb.FontWeight = FontWeights.Bold;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Foreground = Brushes.Gray;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(b);
            panel.Children.Add(tb);
            _breadCrumbButtons.Add(breadCrumb, b);
            _breadCrumbPanels.Add(breadCrumb, panel);

            wrapPanel.Children.Add(panel);

            SetEnabledButtons();
        }

        /// <summary>
        /// Update the given bread crumb.
        /// </summary>
        /// <param name="breadCrumb">The bread crumb to update.</param>
        public void UpdateBreadCrumb(BreadCrumb breadCrumb)
        {
            if (breadCrumb == null)
                throw new ArgumentNullException("breadCrumb");

            if (_breadCrumbButtons.ContainsKey(breadCrumb))
            {
                Button b = _breadCrumbButtons[breadCrumb];
                b.Content = breadCrumb.Name;
            }
        }

        /// <summary>
        /// Remove all of the bread crumbs that come after the given bread crumb.
        /// </summary>
        /// <param name="breadCrumb">The bread crumb to remove bread crumbs after.</param>
        public void RemoveAfterBreadCrumb(BreadCrumb breadCrumb)
        {
            if (breadCrumb == null)
                throw new ArgumentNullException("breadCrumb");

            if (_breadCrumbButtons.ContainsKey(breadCrumb))
            {
                Button b = _breadCrumbButtons[breadCrumb];
                int index = _buttonList.IndexOf(b);
                if (index != -1)
                {
                    index++;
                    while (index < _buttonList.Count)
                    {
                        RemoveBreadCrumb(_buttonList[index].Tag as BreadCrumb);
                    }
                }
            }

            SetEnabledButtons();
        }

        /// <summary>
        /// Remove the bread crumb from the view.
        /// </summary>
        /// <param name="breadCrumb">The bread crumb to remove.</param>
        public void RemoveBreadCrumb(BreadCrumb breadCrumb)
        {
            if (breadCrumb == null)
                throw new ArgumentNullException("breadCrumb");

            if (!_breadCrumbButtons.ContainsKey(breadCrumb))
                return;

            Button b = _breadCrumbButtons[breadCrumb];
            b.Click -= BreadCrumb_Click;
            _breadCrumbButtons.Remove(breadCrumb);

            StackPanel panel = _breadCrumbPanels[breadCrumb];
            wrapPanel.Children.Remove(panel);
            _breadCrumbPanels.Remove(breadCrumb);

            _buttonList.Remove(b);

            SetEnabledButtons();
        }

        /// <summary>
        /// Enable all buttons except the last and disable the back button if there is only one bread crumb.
        /// </summary>
        private void SetEnabledButtons()
        {
            navigationBackButton.IsEnabled = _buttonList.Count > 1;
            for (int i = 0; i < _buttonList.Count - 1; i++)
                _buttonList[i].IsEnabled = true;
            if (_buttonList.Count > 0)
                _buttonList[_buttonList.Count - 1].IsEnabled = false;
        }

        /// <summary>
        /// Raise the BreadCrumbClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnBreadCrumbClick(BreadCrumbEventArgs e)
        {
            if (BreadCrumbClick != null)
                BreadCrumbClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Process the back click.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void navigationBackButton_Click(object sender, EventArgs e)
        {
            Back();
        }

        /// <summary>
        /// Raise the BreadCrumbClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void BreadCrumb_Click(object sender, RoutedEventArgs e)
        {
            OnBreadCrumbClick(new BreadCrumbEventArgs((sender as Button).Tag as BreadCrumb));
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when a bread crumb is clicked.
        /// </summary>
        public event EventHandler<BreadCrumbEventArgs> BreadCrumbClick;

        #endregion
    }
}
