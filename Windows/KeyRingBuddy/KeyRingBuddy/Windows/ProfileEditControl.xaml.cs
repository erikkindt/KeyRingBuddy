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
    /// Interaction logic for ProfileEditControl.xaml
    /// </summary>
    public partial class ProfileEditControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProfileEditControl()
        {
            InitializeComponent();

            ClearInput();
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
        /// The name entered for the profile.
        /// </summary>
        public string ProfileName
        {
            get { return textBoxName.Text; }
            set { textBoxName.Text = value; }
        }

        /// <summary>
        /// The password entered for the profile.
        /// </summary>
        public string ProfilePassword
        {
            get { return passwordBoxPassword.Password; }
            set { passwordBoxPassword.Password = value; }
        }

        /// <summary>
        /// The password re-entered.
        /// </summary>
        public string ProfilePasswordReEnter
        {
            get { return passwordBoxPasswordReEnter.Password; }
            set { passwordBoxPasswordReEnter.Password = value; }
        }

        /// <summary>
        /// This flag is set to true whenever the inputs are modified.
        /// </summary>
        public bool IsModified { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfileNameErrorMessage(string message)
        {
            textBlockNameError.Foreground = Brushes.Firebrick;
            textBlockNameError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfileNameWarningMessage(string message)
        {
            textBlockNameError.Foreground = Brushes.Goldenrod;
            textBlockNameError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfilePasswordErrorMessage(string message)
        {
            textBlockPasswordError.Foreground = Brushes.Firebrick;
            textBlockPasswordError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfilePasswordWarningMessage(string message)
        {
            textBlockPasswordError.Foreground = Brushes.Goldenrod;
            textBlockPasswordError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfilePasswordReEnterErrorMessage(string message)
        {
            textBlockPasswordReEnterError.Foreground = Brushes.Firebrick;
            textBlockPasswordReEnterError.Text = message;
        }

        /// <summary>
        /// Display a warning message next to the input.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetProfilePasswordReEnterWarningMessage(string message)
        {
            textBlockPasswordReEnterError.Foreground = Brushes.Goldenrod;
            textBlockPasswordReEnterError.Text = message;
        }

        /// <summary>
        /// Clear all user input.
        /// </summary>
        public void ClearInput()
        {
            ProfileName = null;
            ProfilePassword = null;
            ProfilePasswordReEnter = null;

            SetProfileNameErrorMessage(null);
            SetProfilePasswordErrorMessage(null);
            SetProfilePasswordReEnterErrorMessage(null);
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
        /// Clear any errors.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetProfileNameErrorMessage(null);
            IsModified = true;
        }

        /// <summary>
        /// Warn if the caps lock key is on.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void passwordBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Console.CapsLock)
                SetProfilePasswordWarningMessage("! Caps lock is on");
            else
                SetProfilePasswordWarningMessage(null);

            IsModified = true;
        }

        /// <summary>
        /// Warn if the caps lock key is on.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void passwordBoxPasswordReEnter_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Console.CapsLock)
                SetProfilePasswordReEnterWarningMessage("! Caps lock is on");
            else
                SetProfilePasswordReEnterWarningMessage(null);

            IsModified = true;
        }

        /// <summary>
        /// Move focus to the next input control.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == textBoxName)
                    passwordBoxPassword.Focus();
                else if (sender == passwordBoxPassword)
                    passwordBoxPasswordReEnter.Focus();
                else if (sender == passwordBoxPasswordReEnter)
                    buttonSave.Focus();
            }
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
