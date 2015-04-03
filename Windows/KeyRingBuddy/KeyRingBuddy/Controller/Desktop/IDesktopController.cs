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

using KeyRingBuddy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyRingBuddy.Controller.Desktop
{
    /// <summary>
    /// Controller for desktop.
    /// </summary>
    public interface IDesktopController : IController
    {
        /// <summary>
        /// Get the display name for the desktop view.
        /// </summary>
        /// <returns>The display name for the desktop view.</returns>
        string GetDisplayName();

        /// <summary>
        /// Get the content to display in the desktop view.
        /// </summary>
        /// <returns>The content to display in the desktop view.</returns>
        UIElement GetContent();

        /// <summary>
        /// Indicates if the controller can be returned to after navigating away.
        /// </summary>
        /// <returns>If true, the controller is kept in the navigation path, if false it is removed.</returns>
        bool CanRestore();

        /// <summary>
        /// If true, focus is given to the controller when it's opened.  If false, focus will not start on the controller
        /// when it is opened.
        /// </summary>
        /// <returns>true to give focus to the controller when it's opened, false if focus should not be given.</returns>
        bool StartWithFocus();

        /// <summary>
        /// Get the names of functions for the given column.
        /// </summary>
        /// <param name="column">The column to get functions for.</param>
        /// <returns>The functions for the given column.</returns>
        IEnumerable<string> GetFunctionNames(int column);

        /// <summary>
        /// Execute the given function.
        /// </summary>
        /// <param name="name">The name of the function to execute.</param>
        void FunctionExecuted(string name);
    }
}
