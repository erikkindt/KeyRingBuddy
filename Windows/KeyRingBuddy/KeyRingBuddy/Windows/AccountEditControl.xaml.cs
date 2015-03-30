using KeyRingBuddy.Model;
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
    /// Interaction logic for AccountEditControl.xaml
    /// </summary>
    public partial class AccountEditControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountEditControl()
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
        /// The account name.
        /// </summary>
        public string AccountName
        {
            get { return textBoxName.Text; }
            set { textBoxName.Text = value; }
        }

        /// <summary>
        /// The account category.
        /// </summary>
        public string AccountCategory
        {
            get { return comboBoxCategory.Text; }
            set { comboBoxCategory.Text = value; }
        }

        /// <summary>
        /// The account site.
        /// </summary>
        public string AccountSite
        {
            get { return textBoxSite.Text; }
            set { textBoxSite.Text = value; }
        }

        /// <summary>
        /// Flag that is set to true whenever an input is modified.
        /// </summary>
        public bool IsModified { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get the details currently entered.
        /// </summary>
        /// <returns>The details currently entered.</returns>
        public IList<AccountDetail> GetDetails()
        {
            int firstRow = Grid.GetRow(textBlockDetails) + 1;
            int lastRow = Grid.GetRow(buttonAddDetail) - 1;
            int total = lastRow - firstRow + 1;
            List<AccountDetail> result = new List<AccountDetail>();
            for (int i = 0; i < total; i++)
                result.Add(new AccountDetail());

            foreach (UIElement element in gridMain.Children)
            {
                int row = Grid.GetRow(element);
                if (row < firstRow || row > lastRow)
                    continue;

                AccountDetail detail = result[row - firstRow];

                int column = Grid.GetColumn(element);
                switch (column)
                {
                    case 0:
                        detail.Name = (element as TextBox).Text;
                        break;

                    case 1:
                        detail.Value = (element as TextBox).Text;
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Add a new detail.
        /// </summary>
        /// <param name="detail">The detail to add or null to add an empty detail.</param>
        public void AddDetail(AccountDetail detail)
        {
            int row = Grid.GetRow(buttonAddDetail);
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            gridMain.RowDefinitions.Insert(row, rowDef);

            Grid.SetRow(buttonAddDetail, row + 1);
            Grid.SetRow(panelButtons, row + 2);

            TextBox textBoxName = new TextBox();
            textBoxName.Text = (detail == null) ? null : detail.Name;
            textBoxName.Margin = new Thickness(5);
            textBoxName.VerticalContentAlignment = VerticalAlignment.Center;
            textBoxName.HorizontalContentAlignment = HorizontalAlignment.Right;
            Grid.SetRow(textBoxName, row);
            Grid.SetColumn(textBoxName, 0);
            textBoxName.TextChanged += textBoxDetail_TextChanged;

            TextBox textBoxValue = new TextBox();
            textBoxValue.Text = (detail == null) ? null : detail.Value;
            textBoxValue.Margin = new Thickness(5);
            textBoxValue.VerticalContentAlignment = VerticalAlignment.Center;
            Grid.SetRow(textBoxValue, row);
            Grid.SetColumn(textBoxValue, 1);
            textBoxValue.TextChanged += textBoxDetail_TextChanged;

            Button buttonRemove = new Button();
            buttonRemove.Content = "- Remove";
            buttonRemove.Margin = new Thickness(5);
            buttonRemove.VerticalContentAlignment = VerticalAlignment.Center;
            buttonRemove.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(buttonRemove, row);
            Grid.SetColumn(buttonRemove, 2);

            int index = gridMain.Children.IndexOf(buttonAddDetail);            
            gridMain.Children.Insert(index, textBoxValue);
            gridMain.Children.Insert(index, textBoxName);

            index = gridMain.Children.IndexOf(panelButtons);
            gridMain.Children.Insert(index, buttonRemove);

            IsModified = true;
        }

        /// <summary>
        /// Remove the detail controls associeated with the given button.
        /// </summary>
        /// <param name="button">The button to remove associated controls for.</param>
        private void RemoveDetail(Button button)
        {
            int row = Grid.GetRow(button);

            for (int i = 0; i < gridMain.Children.Count; i++)
            {
                UIElement element = gridMain.Children[i];
                int r = Grid.GetRow(element);

                if (r == row)
                {
                    gridMain.Children.RemoveAt(i);
                    i--;

                    if (element is TextBox)
                        (element as TextBox).TextChanged -= textBoxDetail_TextChanged;
                }
                else if (r > row)
                {
                    Grid.SetRow(element, r - 1);
                }
            }

            gridMain.RowDefinitions.RemoveAt(row);

            IsModified = true;
        }

        /// <summary>
        /// Clear all of the details.
        /// </summary>
        public void ClearDetails()
        {
            List<Button> buttons = new List<Button>();
            foreach (UIElement element in gridMain.Children)
            {
                Button b = element as Button;
                if (b != null && (b.Content as string) == "- Remove")
                    buttons.Add(b);
            }

            foreach (Button b in buttons)
                RemoveDetail(b);
        }

        /// <summary>
        /// Clear all user input.
        /// </summary>
        public void ClearInput()
        {
            AccountName = null;
            AccountCategory = null;
            AccountSite = null;            

            SetAccountNameErrorMessage(null);
            SetAccountCategoryErrorMessage(null);
            SetAccountSiteErrorMessage(null);

            ClearDetails();
        }

        /// <summary>
        /// Set the categories to display in the drop down.
        /// </summary>
        /// <param name="categories">The categories to display.</param>
        public void SetCategories(IEnumerable<string> categories)
        {
            comboBoxCategory.Items.Clear();

            if (categories != null)
            {
                foreach (string category in categories)
                    comboBoxCategory.Items.Add(category);
            }
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountNameErrorMessage(string message)
        {
            textBlockNameError.Foreground = Brushes.Firebrick;
            textBlockNameError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountNameWarningMessage(string message)
        {
            textBlockNameError.Foreground = Brushes.Goldenrod;
            textBlockNameError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountCategoryErrorMessage(string message)
        {
            textBlockCategoryError.Foreground = Brushes.Firebrick;
            textBlockCategoryError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountCategoryWarningMessage(string message)
        {
            textBlockCategoryError.Foreground = Brushes.Goldenrod;
            textBlockCategoryError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountSiteErrorMessage(string message)
        {
            textBlockSiteError.Foreground = Brushes.Firebrick;
            textBlockSiteError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetAccountSiteWarningMessage(string message)
        {
            textBlockSiteError.Foreground = Brushes.Goldenrod;
            textBlockSiteError.Text = message;
        }

        /// <summary>
        /// Raises the SaveClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnSaveClick(EventArgs e)
        {
            if (SaveClick != null)
                SaveClick(this, e);
        }

        /// <summary>
        /// Raises the CancelClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnCancelClick(EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the SaveClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            OnSaveClick(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the CancelClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancelClick(EventArgs.Empty);
        }

        /// <summary>
        /// Process remove button clicks.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void gridMain_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            if (b != null && (b.Content as string) == "- Remove")
                RemoveDetail(b);
        }

        /// <summary>
        /// Add a new detail.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonAddDetail_Click(object sender, RoutedEventArgs e)
        {
            AddDetail(null);
        }

        /// <summary>
        /// Clear out any errors.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetAccountNameErrorMessage(null);
            IsModified = true;
        }

        /// <summary>
        /// Clear out any errors.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void comboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetAccountCategoryErrorMessage(null);
            IsModified = true;
        }

        /// <summary>
        /// Clear out any errors.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void textBoxSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetAccountSiteErrorMessage(null);
            IsModified = true;
        }

        /// <summary>
        /// Move focus to the next input when enter is keyed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void gridMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (e.Source is TextBox || e.Source is ComboBox))
            {
                int index = gridMain.Children.IndexOf(e.Source as UIElement);
                bool found = false;

                for (int i = index + 1; i < gridMain.Children.Count; i++)
                {
                    UIElement element = gridMain.Children[i];
                    if (element is TextBox || element is ComboBox)
                    {
                        element.Focus();
                        found = true;
                        break;
                    }
                }

                if (!found)
                    buttonSave.Focus();
            }
        }

        /// <summary>
        /// Set the modified flag.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void textBoxDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsModified = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the user clicks the save button.
        /// </summary>
        public event EventHandler SaveClick;

        /// <summary>
        /// Raised when the user clicks the cancel button.
        /// </summary>
        public event EventHandler CancelClick;

        #endregion
    }
}
