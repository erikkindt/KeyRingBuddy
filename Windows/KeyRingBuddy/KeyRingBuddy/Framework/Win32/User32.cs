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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Framework.Win32
{
    /// <summary>
    /// Win32 API methods.
    /// </summary>
    internal static class User32
    {
        #region Fields

        /// <summary>
        /// Clipboard text format.
        /// </summary>
        public const uint CF_TEXT = 1;

        /// <summary>
        /// Sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in 
        /// that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData 
        /// function.
        /// </summary>
        public const int WM_RENDERFORMAT = 0x0305;

        /// <summary>
        /// Sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. 
        /// For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the 
        /// formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.
        /// 
        /// A window receives this message through its WindowProc function.
        /// </summary>
        public const int WM_RENDERALLFORMATS = 0x0306;

        /// <summary>
        /// Used to create a message only window.
        /// </summary>
        public const int HWND_MESSAGE = -3;

        /// <summary>
        /// Used to send a system wide message.
        /// </summary>
        public const int HWND_BROADCAST = 0xFFFF;

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the window handle of the current owner of the clipboard.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the window that owns the clipboard.
        /// If the clipboard is not owned, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardOwner();

        /// <summary>
        /// Opens the clipboard for examination and prevents other applications from modifying the clipboard content.
        /// </summary>
        /// <param name="hWndNewOwner">
        /// A handle to the window to be associated with the open clipboard. If this parameter is NULL, the open clipboard is associated 
        /// with the current task.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// Closes the clipboard.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseClipboard();

        /// <summary>
        /// Empties the clipboard and frees handles to data in the clipboard. The function then assigns ownership of the clipboard 
        /// to the window that currently has the clipboard open.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EmptyClipboard();

        /// <summary>
        /// Places data on the clipboard in a specified clipboard format. The window must be the current clipboard owner, and the 
        /// application must have called the OpenClipboard function. (When responding to the WM_RENDERFORMAT and WM_RENDERALLFORMATS 
        /// messages, the clipboard owner must not call OpenClipboard before calling SetClipboardData.)
        /// </summary>
        /// <param name="uFormat">
        /// The clipboard format. This parameter can be a registered format or any of the standard clipboard formats. For more information, 
        /// see Standard Clipboard Formats and Registered Clipboard Formats.
        /// </param>
        /// <param name="hMem">
        /// A handle to the data in the specified format. This parameter can be NULL, indicating that the window provides data in the specified 
        /// clipboard format (renders the format) upon request. If a window delays rendering, it must process the WM_RENDERFORMAT and 
        /// WM_RENDERALLFORMATS messages.
        /// 
        /// If SetClipboardData succeeds, the system owns the object identified by the hMem parameter. The application may not write to or 
        /// free the data once ownership has been transferred to the system, but it can lock and read from the data until the CloseClipboard 
        /// function is called. (The memory must be unlocked before the Clipboard is closed.) If the hMem parameter identifies a memory object, 
        /// the object must have been allocated using the function with the GMEM_MOVEABLE flag.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the data.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        /// <summary>
        /// Changes the parent window of the specified child window.
        /// </summary>
        /// <param name="hwnd">A handle to the child window.</param>
        /// <param name="hwndNewParent">
        /// A handle to the new parent window. If this parameter is NULL, the desktop window becomes the new parent window. 
        /// If this parameter is HWND_MESSAGE, the child window becomes a message-only window.</param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the previous parent window.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings. 
        /// This function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="lpClassName">
        /// The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. 
        /// The atom must be in the low-order word of lpClassName; the high-order word must be zero.
        /// 
        /// If lpClassName points to a string, it specifies the window class name. The class name can be any name registered 
        /// with RegisterClass or RegisterClassEx, or any of the predefined control-class names.
        /// 
        /// If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.
        /// </param>
        /// <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match.</param>
        /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the 
        /// specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hwnd">
        /// A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), 
        /// the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, 
        /// and pop-up windows; but the message is not sent to child windows.
        /// 
        /// Message sending is subject to UIPI. The thread of a process can send messages only to message queues of threads in processes 
        /// of lesser or equal integrity level.
        /// </param>
        /// <param name="wMsg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion
    }
}
