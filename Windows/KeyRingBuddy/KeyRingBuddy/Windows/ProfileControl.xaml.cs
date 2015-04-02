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
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProfileControl()
        {
            InitializeComponent();
            AccountName = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The currently selected category.
        /// </summary>
        public Item SelectedCategory
        {
            get { return itemListCategories.SelectedItem; }
            set { itemListCategories.SelectedItem = value; }
        }

        /// <summary>
        /// The currently selected account.
        /// </summary>
        public Item SelectedAccount
        {
            get { return itemListAccounts.SelectedItem; }
            set { itemListAccounts.SelectedItem = value; }
        }

        /// <summary>
        /// The account name.
        /// </summary>
        public string AccountName
        {
            get { return textBlockAccountCaption.Text; }
            set
            {
                textBlockAccountCaption.Text = value;
                gridAccount.Visibility = (value == null) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// The account icon.
        /// </summary>
        public ImageSource AccountIcon
        {
            get { return imageAccountCaption.Source; }
            set { imageAccountCaption.Source = value; }
        }

        /// <summary>
        /// The account category.
        /// </summary>
        public string AccountCategory
        {
            get { return accountContent.Category; }
            set { accountContent.Category = value; }
        }

        /// <summary>
        /// The account site.
        /// </summary>
        public string AccountSite
        {
            get { return accountContent.Site; }
            set { accountContent.Site = value; }
        }

        /// <summary>
        /// The account details displayed.
        /// </summary>
        public IList<AccountDetail> AccountDetails
        {
            get { return accountContent.GetDetails(); }
            set
            {
                accountContent.ClearDetails();
                if (value != null)
                {
                    foreach (AccountDetail detail in value)
                        accountContent.AddDetail(detail);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clear out the view.
        /// </summary>
        public void Clear()
        {
            ClearCategories();
            ClearAccounts();
        }

        /// <summary>
        /// Clear out all categories.
        /// </summary>
        public void ClearCategories()
        {
            itemListCategories.ClearItems();
        }

        /// <summary>
        /// Clear out all accounts.
        /// </summary>
        public void ClearAccounts()
        {
            itemListAccounts.ClearItems();
        }

        /// <summary>
        /// Add a category to the view.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(Item category)
        {
            itemListCategories.AddItem(category);
        }

        /// <summary>
        /// Remove a category from the view.
        /// </summary>
        /// <param name="category">The category to remove.</param>
        public void RemoveCategory(Item category)
        {
            itemListCategories.RemoveItem(category);
        }

        /// <summary>
        /// Add an account to the view.
        /// </summary>
        /// <param name="account">The account to add.</param>
        public void AddAccount(Item account)
        {
            itemListAccounts.AddItem(account);
        }

        /// <summary>
        /// Remove an account from the view.
        /// </summary>
        /// <param name="account">The account to remove.</param>
        public void RemoveAccount(Item account)
        {
            itemListAccounts.RemoveItem(account);
        }

        /// <summary>
        /// Raises the SelectedCategoryChanged event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnSelectedCategoryChanged(EventArgs e)
        {
            if (SelectedCategoryChanged != null)
                SelectedCategoryChanged(this, e);
        }

        /// <summary>
        /// Raises the SelectedAccountChanged event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnSelectedAccountChanged(EventArgs e)
        {
            if (SelectedAccountChanged != null)
                SelectedAccountChanged(this, e);
        }

        /// <summary>
        /// Raises the AccountLaunchClick event.
        /// </summary>
        /// <param name="e">Arguments passed with the event.</param>
        protected virtual void OnAccountLaunchClick(EventArgs e)
        {
            if (AccountLaunchClick != null)
                AccountLaunchClick(this, e);
        }

        /// <summary>
        /// Raises the AccountEditClick event.
        /// </summary>
        /// <param name="e">Arguments passed with the event.</param>
        protected virtual void OnAccountEditClick(EventArgs e)
        {
            if (AccountEditClick != null)
                AccountEditClick(this, e);
        }

        /// <summary>
        /// Raises the AccountDeleteClick event.
        /// </summary>
        /// <param name="e">Arguments passed with the event.</param>
        protected virtual void OnAccountDeleteClick(EventArgs e)
        {
            if (AccountDeleteClick != null)
                AccountDeleteClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the SelectedCategoryChanged event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void itemListCategories_SelectedItemChanged(object sender, EventArgs e)
        {
            OnSelectedCategoryChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the SelectedAccountChanged event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void itemListAccounts_SelectedItemChanged(object sender, EventArgs e)
        {
            OnSelectedAccountChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raise the AccountLaunchClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonAccountLaunch_Click(object sender, RoutedEventArgs e)
        {
            OnAccountLaunchClick(EventArgs.Empty);
        }

        /// <summary>
        /// Raise the AccountEditClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonAccountEdit_Click(object sender, RoutedEventArgs e)
        {
            OnAccountEditClick(EventArgs.Empty);
        }

        /// <summary>
        /// Raise the AccountDeleteClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonAccountDelete_Click(object sender, RoutedEventArgs e)
        {
            OnAccountDeleteClick(EventArgs.Empty);
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the selected category is changed.
        /// </summary>
        public event EventHandler SelectedCategoryChanged;

        /// <summary>
        /// Raised when the selected account is changed.
        /// </summary>
        public event EventHandler SelectedAccountChanged;

        /// <summary>
        /// Raised when the user clicks to launch the account.
        /// </summary>
        public event EventHandler AccountLaunchClick;

        /// <summary>
        /// Raised when the user clicks to edit the account.
        /// </summary>
        public event EventHandler AccountEditClick;

        /// <summary>
        /// Raised when the user clicks to delete the account.
        /// </summary>
        public event EventHandler AccountDeleteClick;

        #endregion
    }
}
