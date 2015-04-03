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

using KeyRingBuddy.Controller.Desktop;
using KeyRingBuddy.Framework;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyRingBuddy.Windows
{
    /// <summary>
    /// Interaction logic for DesktopWindow.xaml
    /// </summary>
    public partial class DesktopWindow : Window, IDesktopView
    {
        #region Fields

        /// <summary>
        /// Storyboard used for transitions.
        /// </summary>
        private Storyboard _storyboardTransition = null;

        /// <summary>
        /// When true, focus will be given to an element that has been transitioned to.
        /// </summary>
        private bool _transitionFocus = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public DesktopWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The content displayed.
        /// </summary>
        public UIElement ViewContent
        {
            get { return borderContent.Child; }
            set 
            { 
                borderContent.Child = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the view content using a slide animation.
        /// </summary>
        /// <param name="element">The content.</param>
        /// <param name="direction">The direction to slide.</param>
        /// <param name="giveFocus">true to give the element focus when the transition is complete.</param>
        public void SetContent(UIElement element, FlowDirection direction, bool giveFocus)
        {
            FrameworkElement currentElement = ViewContent as FrameworkElement;
            FrameworkElement nextElement = element as FrameworkElement;

            if (nextElement == null || currentElement == null)
            {
                ViewContent = element;
                return;
            }

            if (_storyboardTransition != null)
            {
                ViewContent = element;
                return;
            }

            _transitionFocus = giveFocus;

            // display transition view
            borderContent.Visibility = Visibility.Hidden;
            borderTransition.Visibility = Visibility.Visible;

            // take snapshot of current view
            VisualBrush currentElementBrush = new VisualBrush(currentElement);
            currentElementBrush.Viewbox = new Rect(0, 0, currentElement.ActualWidth, currentElement.ActualHeight);
            currentElementBrush.ViewboxUnits = BrushMappingMode.Absolute;
            currentRectangle.Fill = currentElementBrush;

            // take snapshot of next view            
            VisualBrush nextElementBrush = new VisualBrush(nextElement);
            nextElementBrush.Viewbox = new Rect(0, 0, currentElement.ActualWidth, currentElement.ActualHeight);
            nextElementBrush.ViewboxUnits = BrushMappingMode.Absolute;
            nextRectangle.Fill = nextElementBrush;

            // create storyboard
            _storyboardTransition = (Resources["storyboardTransition"] as Storyboard).Clone();
            DoubleAnimation currentAnimation = _storyboardTransition.Children[0] as DoubleAnimation;
            DoubleAnimation nextAnimation = _storyboardTransition.Children[1] as DoubleAnimation;
            switch (direction)
            {
                case FlowDirection.RightToLeft:
                    currentAnimation.From = 0;
                    currentAnimation.To = -1 * gridTransition.ActualWidth;
                    nextAnimation.From = gridTransition.ActualWidth;
                    nextAnimation.To = 0;
                    break;

                case FlowDirection.LeftToRight:
                default:
                    currentAnimation.From = 0;
                    currentAnimation.To = gridTransition.ActualWidth;
                    nextAnimation.From = -1 * gridTransition.ActualWidth;
                    nextAnimation.To = 0;
                    break;
            }
            Storyboard.SetTarget(currentAnimation, currentRectangle);
            Storyboard.SetTarget(nextAnimation, nextRectangle);

            ViewContent = element;

            // start storyboard
            borderContent.Tag = element;
            _storyboardTransition.Completed += FinishTransition;
            _storyboardTransition.Begin();
        }

        /// <summary>
        /// Add a controller to the view.
        /// </summary>
        /// <param name="name">The name for the controller.</param>
        /// <param name="controller">The controller to add.  It will be made the current controller.</param>
        public void AddController(IDesktopController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            BreadCrumb breadCrumb = new BreadCrumb(controller.GetDisplayName(), controller);
            breadCrumbView.AddBreadCrumb(breadCrumb);
        }

        /// <summary>
        /// Remove a controller from the view.
        /// </summary>
        /// <param name="controller">
        /// A controller to remove that was added previously with the AddController method.
        /// </param>
        public void RemoveController(IDesktopController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            BreadCrumb breadCrumb = new BreadCrumb(controller.GetDisplayName(), controller);
            breadCrumbView.RemoveBreadCrumb(breadCrumb);
        }

        /// <summary>
        /// Set the current controller.
        /// </summary>
        /// <param name="controller">
        /// A controller that was added previously with the AddController method.
        /// </param>
        /// <remarks>
        /// All controllers added after the given controller are removed from the view.
        /// </remarks>
        public void SetCurrentController(IDesktopController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            BreadCrumb breadCrumb = new BreadCrumb(controller.GetDisplayName(), controller);
            breadCrumbView.RemoveAfterBreadCrumb(breadCrumb);
            breadCrumbView.UpdateBreadCrumb(breadCrumb);
        }

        /// <summary>
        /// Navigate to the given controller.
        /// </summary>
        /// <param name="controller">The controller to navigate to.</param>
        private void Navigate(IDesktopController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            App.DesktopManager.RestoreController(controller);
        }

        /// <summary>
        /// Add a function to the view.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="column">The column to put the function in.</param>
        public void AddFunction(string name, int column)
        {
            while (column >= stackPanelFunctions.Children.Count)
                stackPanelFunctions.Children.Add(new StackPanel());

            Button b = new Button();
            b.Content = name;
            b.Foreground = FindResource("LightPlusBrush") as Brush;

            (stackPanelFunctions.Children[column] as StackPanel).Children.Add(b);
        }

        /// <summary>
        /// Clear all of the functions.
        /// </summary>
        public void ClearFunctions()
        {
            stackPanelFunctions.Children.Clear();
        }

        /// <summary>
        /// Move back one controller.
        /// </summary>
        public void Back()
        {
            breadCrumbView.Back();
        }

        /// <summary>
        /// Raises the FunctionClick event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnFunctionClick(FunctionEventArgs e)
        {
            if (FunctionClick != null)
                FunctionClick(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Navigate to the clicked bread crumb.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void breadCrumbView_BreadCrumbClick(object sender, BreadCrumbEventArgs e)
        {
            if (e.BreadCrumb.Tag is IDesktopController)
                Navigate(e.BreadCrumb.Tag as IDesktopController);
        }

        /// <summary>
        /// Raise the FunctionClick event.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void stackPanelFunctions_Click(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            if (b != null)
            {
                string text = b.Content as string;
                if (text != null)
                {
                    BreadCrumb breadCrumb = breadCrumbView.TopBreadCrumb;
                    if (breadCrumb != null && breadCrumb.Tag is IDesktopController)
                        (breadCrumb.Tag as IDesktopController).FunctionExecuted(text);
                }
            }
        }

        /// <summary>
        /// Finish a transition.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void FinishTransition(object sender, EventArgs e)
        {
            _storyboardTransition.Completed -= FinishTransition;

            borderTransition.Visibility = Visibility.Hidden;
            borderContent.Visibility = Visibility.Visible;

            currentRectangle.Fill = null;
            nextRectangle.Fill = null;

            _storyboardTransition = null;

            if (_transitionFocus)
            {
                this.Dispatcher.BeginInvoke(
                    (Action)delegate()
                    {
                        borderContent.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    },
                    System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when a function is clicked.
        /// </summary>
        public event EventHandler<FunctionEventArgs> FunctionClick;

        #endregion
    }
}
