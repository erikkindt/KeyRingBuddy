using System;
using System.Threading.Tasks;
using KeyRingBuddy.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Test the ClipboardStackInjector class.
    /// </summary>
    [TestClass]
    public class ClipboardStackInjectorTests
    {
        #region Methods

        /// <summary>
        /// Test the Inject method.
        /// </summary>
        [TestMethod]
        public void ClipboardStackInjector_InjectTest()
        {
            ClipboardStackInjector.Inject(new string[] { "first", "second", "third" }, TimeSpan.FromSeconds(10));

            Assert.AreEqual<string>("first", System.Windows.Clipboard.GetText());
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual<string>("second", System.Windows.Clipboard.GetText());
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual<string>("third", System.Windows.Clipboard.GetText());
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Test the timeout feature of the ClipboardStackInjector.
        /// </summary>
        [TestMethod]
        public void ClipboardStackInjector_TimeoutTest()
        {
            ClipboardStackInjector.Inject(new string[] { "first", "second", "third" }, TimeSpan.FromSeconds(4));
            System.Threading.Thread.Sleep(5000);
            Assert.AreEqual<string>(String.Empty, System.Windows.Clipboard.GetText());
        }

        /// <summary>
        /// Test the overwrite of a ClipboardStackInjector.
        /// </summary>
        [TestMethod]
        public void ClipboardStackInjector_OverwriteTest()
        {
            ClipboardStackInjector.Inject(new string[] { "first", "second", "third" }, TimeSpan.FromSeconds(4));
            System.Threading.Thread.Sleep(1000);
            System.Windows.Clipboard.SetText("overwrite");
            System.Threading.Thread.Sleep(5000);

            Assert.AreEqual<string>("overwrite", System.Windows.Clipboard.GetText());
        }

        #endregion
    }
}
