/*
 * Copyright (c) 2015 Nathaniel Wallace
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

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
    /// Interaction logic for AccountContentView.xaml
    /// </summary>
    public partial class AccountContentView : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountContentView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Category.
        /// </summary>
        public string Category
        {
            get { return textBlockCategory.Text; }
            set { textBlockCategory.Text = value; }
        }

        /// <summary>
        /// The Site.
        /// </summary>
        public string Site
        {
            get { return textBlockSite.Text; }
            set { textBlockSite.Text = value; }
        }

        /// <summary>
        /// Show/Hide details.
        /// </summary>
        public bool IsDetailsVisible
        {
            get { return (buttonDetails.Content as string) == "- Details:"; }
            set
            {
                if (IsDetailsVisible == value)
                    return;

                buttonDetails.Content = value ? "- Details:" : "+ Details:";

                int startRow = 3;
                foreach (UIElement element in gridMain.Children)
                {
                    int row = Grid.GetRow(element);
                    if (row >= startRow)
                        element.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a detail to the view.
        /// </summary>
        /// <param name="detail">The detail to add.</param>
        public void AddDetail(AccountDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException("detail");

            int row = gridMain.RowDefinitions.Count;

            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            gridMain.RowDefinitions.Add(rowDef);

            TextBlock textBlockName = new TextBlock();
            textBlockName.HorizontalAlignment = HorizontalAlignment.Right;
            textBlockName.FontSize = 16;
            textBlockName.FontWeight = FontWeights.Medium;
            textBlockName.Foreground = FindResource("DarkBrush") as Brush;
            textBlockName.Margin = new Thickness(10, 5, 5, 5);
            textBlockName.Text = String.Format("{0}:", detail.Name);
            textBlockName.Tag = detail;
            textBlockName.Visibility = IsDetailsVisible ? Visibility.Visible : Visibility.Collapsed;
            Grid.SetRow(textBlockName, row);
            Grid.SetColumn(textBlockName, 0);
            gridMain.Children.Add(textBlockName);

            TextBlock textBlockValue = new TextBlock();
            textBlockValue.FontSize = 16;
            textBlockValue.FontWeight = FontWeights.Medium;
            textBlockValue.Foreground = FindResource("DarkPlusBrush") as Brush;
            textBlockValue.Margin = new Thickness(5);
            textBlockValue.Text = detail.Value;
            textBlockValue.Visibility = IsDetailsVisible ? Visibility.Visible : Visibility.Collapsed;
            Grid.SetRow(textBlockValue, row);
            Grid.SetColumn(textBlockValue, 1);
            gridMain.Children.Add(textBlockValue);
        }

        /// <summary>
        /// Get the details displayed.
        /// </summary>
        /// <returns>The details displayed.</returns>
        public IList<AccountDetail> GetDetails()
        {
            List<AccountDetail> result = new List<AccountDetail>();
            foreach (UIElement element in gridMain.Children)
            {
                TextBlock tb = element as TextBlock;
                if (tb != null && tb.Tag is AccountDetail)
                    result.Add(tb.Tag as AccountDetail);
            }

            return result;
        }

        /// <summary>
        /// Clear out all of the details.
        /// </summary>
        public void ClearDetails()
        {
            int startRow = 3;
            int total = gridMain.RowDefinitions.Count - 3;

            for (int i = 0; i < gridMain.Children.Count; i++)
            {
                UIElement element = gridMain.Children[i];
                int row = Grid.GetRow(element);

                if (row >= startRow)
                {
                    gridMain.Children.RemoveAt(i);
                    i--;
                }
            }

            if (total > 0)
                gridMain.RowDefinitions.RemoveRange(startRow, total);

            IsDetailsVisible = false;
        }

        /// <summary>
        /// Toggle the visibility of the details.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonDetails_Click(object sender, RoutedEventArgs e)
        {
            IsDetailsVisible = !IsDetailsVisible;
        }

        #endregion
    }
}
