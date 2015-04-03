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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddyTests
{
    /// <summary>
    /// Tests for AccountHeader class.
    /// </summary>
    [TestClass]
    public class AccountHeaderTests
    {
        /// <summary>
        /// Test the ToString method.
        /// </summary>
        [TestMethod]
        public void AccountHeader_ToStringTest()
        {
            AccountHeader ah = new AccountHeader("TestCategory", "TestAccount", null, Guid.NewGuid());
            Assert.AreEqual<string>("TestAccount", ah.ToString());
        }

        /// <summary>
        /// Test the Equals method.
        /// </summary>
        [TestMethod]
        public void AccountHeader_EqualsTest()
        {
            AccountHeader ah1 = new AccountHeader("TestCategory1", "TestAccount1", null, Guid.NewGuid());
            AccountHeader ah2 = new AccountHeader("TestCategory2", "TestAccount2", null, ah1.AccountId);

            Assert.AreEqual<AccountHeader>(ah1, ah2);
            Assert.AreNotEqual<AccountHeader>(null, ah1);
        }
    }
}
