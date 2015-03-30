using KeyRingBuddy.Controller.Desktop;
using KeyRingBuddy.Framework;
using KeyRingBuddy.Model;
using KeyRingBuddy.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KeyRingBuddy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        /// <summary>
        /// The folder that holds data for this application.
        /// </summary>
        private static readonly string APP_DATA_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyRingBuddy");

        #endregion

        #region Properties

        /// <summary>
        /// The profile manager for the application.
        /// </summary>
        public static IProfileManager ProfileManager { get; private set; }

        /// <summary>
        /// The manager for the application.
        /// </summary>
        public static DesktopApplicationManager DesktopManager { get; private set; }

        /// <summary>
        /// The desktop window.
        /// </summary>
        private static DesktopWindow DesktopWindow { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize and start the app.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                if (!Directory.Exists(APP_DATA_FOLDER))
                    Directory.CreateDirectory(APP_DATA_FOLDER);

                ProfileManager = new ZipProfileManager(Path.Combine(APP_DATA_FOLDER, "Profiles"));

                DesktopWindow = new DesktopWindow();
                DesktopManager = new DesktopApplicationManager(DesktopWindow);

                InitializeApplication();
                DesktopManager.RaiseEvent(ApplicationEventNames.STARTED);

                DesktopWindow.Show();
            }
            catch (Exception err)
            {
                App.HandleException(err);

                if (Application.Current != null)
                    Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Post a message to the user.
        /// </summary>
        /// <param name="message">The message to post.</param>
        /// <param name="caption">The caption for the message.</param>
        /// <param name="image">The image to display with the message.</param>
        /// <param name="answers">Possible answers the user can select in response to the message.</param>
        /// <returns>The answer the user selected.</returns>
        public static string MessageUser(string message, string caption, MessageBoxImage image, params string[] answers)
        {
            MessageUserWindow dlg = new MessageUserWindow();
            dlg.Message = message;
            dlg.Title = caption;
            dlg.Image = image;
            dlg.Answers = answers;
            App.ShowDialog(dlg);
            return dlg.Answer;
        }

        /// <summary>
        /// Shows the given window as a modal dialog.
        /// </summary>
        /// <param name="window">The window to display as a dialog.</param>
        /// <returns>The dialog result of the window that was displayed.</returns>
        public static bool ShowDialog(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            window.Owner = DesktopWindow;
            window.ShowInTaskbar = false;

            bool? result = window.ShowDialog();

            return (result.HasValue && result.Value);
        }

        /// <summary>
        /// Handle the given execption.
        /// </summary>
        /// <param name="err">The exception to handle.</param>
        public static void HandleException(Exception err)
        {
            MessageBox.Show("ERROR: " + err.Message);
        }

        /// <summary>
        /// Setup the application.
        /// </summary>
        private static void InitializeApplication()
        {
            DesktopManager.RegisterListener(new ProfileSelectController());
            DesktopManager.RegisterListener(new ProfileEditController());
            DesktopManager.RegisterListener(new ProfileLoginController());
            DesktopManager.RegisterListener(new ProfileController());
            DesktopManager.RegisterListener(new AccountEditController());
        }

        #endregion
    }
}
