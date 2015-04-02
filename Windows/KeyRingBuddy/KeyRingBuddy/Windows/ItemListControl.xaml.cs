using KeyRingBuddy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ItemListControl.xaml
    /// </summary>
    public partial class ItemListControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemListControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The caption displayed.
        /// </summary>
        public string Caption
        {
            get { return textBlockCaption.Text; }
            set { textBlockCaption.Text = value; }
        }

        /// <summary>
        /// The currently selected item or null if there isn't one.
        /// </summary>
        public Item SelectedItem 
        {
            get 
            {
                foreach (ToggleButton button in stackPanel.Children)
                    if (button.IsChecked.HasValue && button.IsChecked.Value)
                        return button.Tag as Item;

                return null;
            }
            set
            {
                foreach (ToggleButton button in stackPanel.Children)
                {
                    if (Object.Equals(value, button.Tag))
                    {
                        button.IsChecked = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear out items.
        /// </summary>
        public void ClearItems()
        {
            bool isSelectedItem = (SelectedItem != null);
            stackPanel.Children.Clear();
            if (isSelectedItem)
                OnSelectedItemChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Add an item to the view.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            int index = 0;
            for (; index < stackPanel.Children.Count; index++)
            {
                ToggleButton b = stackPanel.Children[index] as ToggleButton;
                Item c = b.Tag as Item;
                int order = String.Compare(item.Name, c.Name);
                if (order <= 0)
                    break;
            }

            ToggleButton button = new ToggleButton();
            button.Style = FindResource("ChromeListButtonStyle") as Style;
            button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.Tag = item;

            if (item.Icon == null)
            {
                button.Content = item.Name;
            }
            else
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Stretch;

                Image img = new Image();
                img.Height = 16;
                img.Width = 16;
                img.VerticalAlignment = VerticalAlignment.Center;
                img.Source =  item.Icon;
                sp.Children.Add(img);

                TextBlock tb = new TextBlock();
                tb.Text = item.Name;
                tb.Margin = new Thickness(10, 0, 10, 0);
                tb.VerticalAlignment = VerticalAlignment.Center;
                sp.Children.Add(tb);

                button.Content = sp;
            }

            if (index == stackPanel.Children.Count)
                stackPanel.Children.Add(button);
            else
                stackPanel.Children.Insert(index, button);
        }

        /// <summary>
        /// Remove the given item from the list.
        /// </summary>
        /// <param name="item">Object that raised the event.</param>
        public void RemoveItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            for (int i = 0; i < stackPanel.Children.Count; i++)
            {
                if (Object.Equals(item, (stackPanel.Children[i] as ToggleButton).Tag))
                {
                    bool isSelected = SelectedItem != null;
                    stackPanel.Children.RemoveAt(i);
                    if (isSelected && SelectedItem == null)
                        OnSelectedItemChanged(EventArgs.Empty);

                    break;
                }
            }
        }

        /// <summary>
        /// Raises the SelectedItemChanged event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Mark the newly selected category.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void stackPanel_Checked(object sender, RoutedEventArgs e)
        {
            foreach (ToggleButton button in stackPanel.Children)
            {
                if (button != e.Source)
                {
                    button.IsChecked = false;
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = false;
                }
            }

            OnSelectedItemChanged(EventArgs.Empty);
            (e.Source as ToggleButton).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the SelectedItem is changed.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion
    }
}
