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
    /// Base class for desktop controllers.
    /// </summary>
    public abstract class DesktopControllerBase : IDesktopController, IEventListener
    {
        #region Methods

        /// <summary>
        /// Called when the controller is being closed.
        /// </summary>
        /// <returns>true if it's ok to close the controller, false if it is not.</returns>
        public virtual bool Closing()
        {
            return true;
        }

        /// <summary>
        /// Called when the control is closed.
        /// </summary>
        public virtual void Closed()
        {
        }

        /// <summary>
        /// Called when the control is opened.
        /// </summary>
        public virtual void Opened()
        {
        }

        /// <summary>
        /// Get the display name for the desktop view.
        /// </summary>
        /// <returns>The display name for the desktop view.</returns>
        public virtual string GetDisplayName()
        {
            return "Controller";
        }

        /// <summary>
        /// Get the content to display in the desktop view.
        /// </summary>
        /// <returns>The content to display in the desktop view.</returns>
        public virtual UIElement GetContent()
        {
            return null;
        }

        /// <summary>
        /// Indicates if the controller can be returned to after navigating away.
        /// </summary>
        /// <returns>If true, the controller is kept in the navigation path, if false it is removed.</returns>
        public virtual bool CanRestore()
        {
            return true;
        }

        /// <summary>
        /// If true, focus is given to the controller when it's opened.  If false, focus will not start on the controller
        /// when it is opened.
        /// </summary>
        /// <returns>true to give focus to the controller when it's opened, false if focus should not be given.</returns>
        public virtual bool StartWithFocus()
        {
            return true;
        }

        /// <summary>
        /// Get the names of functions for the given column.
        /// </summary>
        /// <param name="column">The column to get functions for.</param>
        /// <returns>The functions for the given column.</returns>
        public virtual IEnumerable<string> GetFunctionNames(int column)
        {
            return null;
        }

        /// <summary>
        /// Execute the given function.
        /// </summary>
        /// <param name="name">The name of the function to execute.</param>
        public virtual void FunctionExecuted(string name)
        {
        }

        /// <summary>
        /// Get the names of the events to register this listener for.
        /// </summary>
        /// <returns>The names of the events to register this listener for.</returns>
        public virtual IEnumerable<string> GetRegisteredEventNames()
        {
            return null;
        }

        /// <summary>
        /// Called when an event is raised.
        /// </summary>
        /// <param name="eventName">The name of the event that was fired.</param>
        /// <param name="arguments">Arguments for the event if there are any.</param>
        /// <returns>true if the event was processed successfully, false if it was not.</returns>
        public virtual bool EventRaised(string eventName, object[] arguments)
        {
            return App.DesktopManager.LoadController(this);
        }

        /// <summary>
        /// Get the argument.
        /// </summary>
        /// <typeparam name="TType">The expected argument type.</typeparam>
        /// <param name="arguments">The argument collection.</param>
        /// <param name="index">The index of the argument.</param>
        /// <returns>The argument.</returns>
        protected TType GetArgument<TType>(object[] arguments, int index)
        {
            return GetArgument<TType>(arguments, index, false);
        }

        /// <summary>
        /// Get the argument.
        /// </summary>
        /// <typeparam name="TType">The expected argument type.</typeparam>
        /// <param name="arguments">The argument collection.</param>
        /// <param name="index">The index of the argument.</param>
        /// <param name="isNullOk">If true, a null value is ok.  If false an exception is thrown when null is found.</param>
        /// <returns>The argument.</returns>
        protected TType GetArgument<TType>(object[] arguments, int index, bool isNullOk)
        {
            if (arguments == null)
                throw new Exception("no arguments were passed with the event.");
            if (index >= arguments.Length)
                throw new Exception("not enough arguments were passed with the event.");

            if (arguments[index] == null)
            {
                if (isNullOk)
                    return default(TType);
                else
                    throw new Exception("argument was null.");
            }

            if (!(arguments[index] is TType))
                throw new Exception("invalid type of argument was passed with the event.");

            return (TType)arguments[index];
        }

        #endregion
    }
}
