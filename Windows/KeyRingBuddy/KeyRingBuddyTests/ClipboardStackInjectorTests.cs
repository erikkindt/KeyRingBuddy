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
