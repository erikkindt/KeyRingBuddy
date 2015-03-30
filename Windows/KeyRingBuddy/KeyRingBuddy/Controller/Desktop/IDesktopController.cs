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
