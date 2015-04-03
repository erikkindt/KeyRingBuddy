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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Defines the interface for the desktop view.
    /// </summary>
    public interface IDesktopView
    {
        /// <summary>
        /// The content displayed.
        /// </summary>
        UIElement ViewContent { get; set; }

        /// <summary>
        /// Set the view content using a slide animation.
        /// </summary>
        /// <param name="element">The content.</param>
        /// <param name="direction">The direction to slide.</param>
        /// <param name="giveFocus">true to give the element focus when the transition is complete.</param>
        void SetContent(UIElement element, FlowDirection direction, bool giveFocus);

        /// <summary>
        /// Add a controller to the view.
        /// </summary>
        /// <param name="controller">The controller to add.  It will be made the current controller.</param>
        void AddController(IDesktopController controller);

        /// <summary>
        /// Remove a controller from the view.
        /// </summary>
        /// <param name="controller">
        /// A controller to remove that was added previously with the AddController method.
        /// </param>
        void RemoveController(IDesktopController controller);

        /// <summary>
        /// Set the current controller.
        /// </summary>
        /// <param name="controller">
        /// A controller that was added previously with the AddController method.
        /// </param>
        /// <remarks>
        /// All controllers added after the given controller are removed from the view.
        /// </remarks>
        void SetCurrentController(IDesktopController controller);

        /// <summary>
        /// Add a function to the view.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="column">The column to put the function in.</param>
        void AddFunction(string name, int column);

        /// <summary>
        /// Clear all of the functions.
        /// </summary>
        void ClearFunctions();

        /// <summary>
        /// Move back on controller.
        /// </summary>
        void Back();
    }
}
