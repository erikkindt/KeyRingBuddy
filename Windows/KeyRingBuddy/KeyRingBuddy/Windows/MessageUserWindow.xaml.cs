using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Interaction logic for AskUserWindow.xaml
    /// </summary>
    public partial class MessageUserWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Constructors.
        /// </summary>
        public MessageUserWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The image displayed.
        /// </summary>
        public MessageBoxImage Image
        {
            set
            {
                switch (value)
                {
                    case MessageBoxImage.Error:
                        textBlockIcon.Text = "!";
                        textBlockIcon.Foreground = Brushes.Firebrick;
                        borderIcon.BorderBrush = Brushes.Firebrick;
                        break;

                    case MessageBoxImage.Information:
                        textBlockIcon.Text = "i";
                        textBlockIcon.Foreground = Brushes.CornflowerBlue;
                        borderIcon.BorderBrush = Brushes.CornflowerBlue;
                        break;

                    case MessageBoxImage.Warning:
                        textBlockIcon.Text = "!";
                        textBlockIcon.Foreground = Brushes.Goldenrod;
                        borderIcon.BorderBrush = Brushes.Goldenrod;
                        break;

                    case MessageBoxImage.Question:
                        textBlockIcon.Text = "?";
                        textBlockIcon.Foreground = Brushes.CornflowerBlue;
                        borderIcon.BorderBrush = Brushes.CornflowerBlue;
                        break;

                    case MessageBoxImage.None:
                    default:
                        textBlockIcon.Text = "";
                        borderIcon.BorderBrush = Brushes.Transparent;
                        break;
                }
            }
        }

        /// <summary>
        /// The text message displayed.
        /// </summary>
        public string Message
        {
            get { return textBlockMessage.Text; }
            set { textBlockMessage.Text = value; }
        }

        /// <summary>
        /// The possible answers the user can choose from.
        /// </summary>
        public IEnumerable<string> Answers
        {
            set
            {
                foreach (Button b in stackPanelButtons.Children)
                    b.Click -= button_Click;

                stackPanelButtons.Children.Clear();

                if (value != null)
                {
                    foreach (string answer in value)
                    {
                        Button button = new Button();
                        button.Style = FindResource("ChromeButtonStyle") as Style;
                        button.Content = answer;
                        button.Margin = new Thickness(5);
                        button.Padding = new Thickness(10, 5, 10, 5);
                        button.Click += button_Click;

                        stackPanelButtons.Children.Add(button);
                    }
                }
            }
        }

        /// <summary>
        /// The answer chosen by the user or null if they didn't choose.
        /// </summary>
        public string Answer { get; private set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Set the answer and close the dialog.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                Answer = b.Content as string;
                Close();
            }
            catch (Exception err)
            {
                App.HandleException(err);
            }
        }

        #endregion
    }
}
