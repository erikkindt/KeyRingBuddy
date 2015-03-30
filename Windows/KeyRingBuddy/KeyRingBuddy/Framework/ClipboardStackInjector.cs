using KeyRingBuddy.Framework.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Threading;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Class used to 
    /// </summary>
    public class ClipboardStackInjector
    {
        #region Fields

        /// <summary>
        /// Used to accept window messages.
        /// </summary>
        private MessageOnlyWindow _window;

        /// <summary>
        /// Hook into window messages.
        /// </summary>
        private HwndSourceHook _hook;

        /// <summary>
        /// Holds the stack of items to be injected into the clipboard.
        /// </summary>
        private Stack<string> _stack;

        /// <summary>
        /// Set to true after this object has been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Used to dispose this object after the timeout is up.
        /// </summary>
        private DispatcherTimer _timeout;

        /// <summary>
        /// An instance of this ClipboardStackInjector used for singleton pattern.
        /// </summary>
        private static ClipboardStackInjector _instance;

        /// <summary>
        /// Set to true when the window is being closed.
        /// </summary>
        private bool _windowIsClosing;

        /// <summary>
        /// Gets signaled when this object has been initialized.
        /// </summary>
        private EventWaitHandle _initialized;

        /// <summary>
        /// The dispatcher used with this object.
        /// </summary>
        private Dispatcher _dispatcher;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="items">The items to inject into the clipboard.</param>
        /// <param name="timeout">
        /// The amount of time to wait for the entire stack to be injected before timing out and disposing of this object.
        /// </param>
        public ClipboardStackInjector(IEnumerable<string> items, TimeSpan timeout)
        {
            _initialized = new EventWaitHandle(false, EventResetMode.ManualReset);
            Thread t = new Thread(() => Init(items, timeout));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            _initialized.WaitOne();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new ClipboardStackInjector that runs on a background thread and injects the given items
        /// one at a time as they are pasted.
        /// </summary>
        /// <param name="items">The items to inject into the clipboard.</param>
        /// <param name="timeout">
        /// The amount of time to wait for the entire stack to be injected before timing out and disposing of the ClipboardStackInjector.
        /// </param>
        public static void Inject(IEnumerable<string> items, TimeSpan timeout)
        {
            if (items == null || items.Count() == 0)
                throw new ArgumentException("items is null or empty.", "items");
            if (items.Any(i => String.IsNullOrEmpty(i)))
                throw new ArgumentException("items contains a null or empty string.", "items");
            if (timeout == TimeSpan.Zero)
                throw new ArgumentException("timeout is zero.", "timeout");

            if (_instance != null)
                _instance.Dispose();

            _instance = new ClipboardStackInjector(items, timeout);
        }

        /// <summary>
        /// Initializes this object.
        /// </summary>
        /// <param name="items">The items to inject into the clipboard.</param>
        /// <param name="timeout">
        /// The amount of time to wait for the entire stack to be injected before timing out and disposing of this object.
        /// </param>
        private void Init(IEnumerable<string> items, TimeSpan timeout)
        {
            try
            {
                if (items == null)
                    throw new ArgumentException("items is null.", "items");

                _stack = new Stack<string>();
                foreach (string text in items.Reverse())
                {
                    if (text == null)
                        throw new ArgumentException("items contains a null value.", "items");

                    _stack.Push(text);
                }
                if (_stack.Count == 0)
                    throw new ArgumentException("items is empty.", "items");

                _isDisposed = false;

                _windowIsClosing = false;
                _window = new MessageOnlyWindow();
                _window.Closing += new CancelEventHandler(_window_Closing);
                _window.Show();                

                _hook = new HwndSourceHook(WndProc);
                _window.WindowSource.AddHook(_hook);

                InjectText(null, 0);

                _dispatcher = Dispatcher.CurrentDispatcher;
                _timeout = new DispatcherTimer(DispatcherPriority.Normal, _dispatcher);
                _timeout.Tick += new EventHandler(timeout_Tick);
                _timeout.Interval = timeout;
                _timeout.Start();

                _initialized.Set();
                Dispatcher.CurrentDispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler((sender, e) =>
                {
                    OnError(new ThreadExceptionEventArgs(e.Exception));
                });

                Dispatcher.Run();
            }
            catch (Exception err)
            {
                Dispose();
                OnError(new ThreadExceptionEventArgs(err));
            }
        }

        /// <summary>
        /// Injects the given text into the clipboard.
        /// </summary>
        /// <param name="text">
        /// The text to inject.
        /// If the value is null the clipboard will be setup for delayed rendering.
        /// If the value is String.Empty the clipboard will be emptied.
        /// If the text is neither null or String.Empty the text will be set on the clipboard (without taking ownership of the clipboard).
        /// </param>
        /// <param name="delay">
        /// The number of milliseconds before the text will be injected into the clipboard.
        /// If the value is zero or negative then the text is injecte immediately.
        /// </param>
        private void InjectText(string text, int delay)
        {
            try
            {
                if (delay <= 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (String.IsNullOrEmpty(text))
                        {
                            if (!User32.OpenClipboard(_window.WindowHandle))
                            {
                                Thread.Sleep(100); // wait a 1/10 th of a second before trying again.
                                continue;
                            }
                        }

                        if (text == null)
                        {
                            if (!User32.EmptyClipboard())
                                throw new Win32Exception();
                            User32.SetClipboardData(User32.CF_TEXT, IntPtr.Zero);
                        }
                        else if (text == String.Empty)
                        {
                            if (!User32.EmptyClipboard())
                                throw new Win32Exception();
                        }
                        else
                        {
                            byte[] bits = Encoding.ASCII.GetBytes(String.Format("{0}\0", text));
                            IntPtr memPtr = Marshal.AllocHGlobal(bits.Length);
                            Marshal.Copy(bits, 0, memPtr, bits.Length);

                            if (User32.SetClipboardData(User32.CF_TEXT, memPtr) == IntPtr.Zero)
                                throw new Win32Exception();
                        }

                        if (String.IsNullOrEmpty(text))
                        {
                            if (!User32.CloseClipboard())
                                throw new Win32Exception();
                        }

                        break;
                    }
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep(delay);
                        InjectText(text, 0);
                    });
                }
            }
            catch (Exception err)
            {
                Dispose();
                OnError(new ThreadExceptionEventArgs(err));
            }
        }        

        /// <summary>
        /// Handles window messages.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message id.</param>
        /// <param name="wParam">A parameter.</param>
        /// <param name="lParam">A parameter.</param>
        /// <param name="handled">Flag indicated if the message was handled.</param>
        /// <returns>null.</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                switch (msg)
                {
                    case User32.WM_RENDERFORMAT:

                        if (_stack.Count > 0)
                            InjectText(_stack.Pop(), 0);

                        if (_stack.Count == 0)
                            _timeout.Interval = TimeSpan.FromMilliseconds(200);
                        else
                            InjectText(null, 200);

                        handled = true;
                        break;

                    case User32.WM_RENDERALLFORMATS:

                        Dispose();
                        handled = true;
                        break;

                    default:
                        break;
                }                
            }
            catch (Exception err)
            {
                Dispose();
                OnError(new ThreadExceptionEventArgs(err));                
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Remove message hook and close window.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (User32.GetClipboardOwner() == _window.WindowHandle)
                    InjectText(String.Empty, 0);
                _window.WindowSource.RemoveHook(_hook);
                if (!_windowIsClosing)
                    _dispatcher.InvokeShutdown();
            }
        }

        /// <summary>
        /// Raises the Error event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        private static void OnError(ThreadExceptionEventArgs e)
        {
            if (Error != null)
                Error(null, e);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Dispose this class.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void timeout_Tick(object sender, EventArgs e)
        {
            _timeout.Stop();
            Dispose();
        }

        /// <summary>
        /// Dispose of this object if the window is being closed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void _window_Closing(object sender, CancelEventArgs e)
        {
            _windowIsClosing = true;
            Dispose();
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when an error has occured.
        /// </summary>
        public static event ThreadExceptionEventHandler Error;

        #endregion
    }
}
