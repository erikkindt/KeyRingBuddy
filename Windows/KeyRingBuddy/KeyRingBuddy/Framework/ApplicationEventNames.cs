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

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// Well known event names.
    /// </summary>
    public class ApplicationEventNames
    {
        #region Fields

        /// <summary>
        /// The application has been started.
        /// </summary>
        public const string STARTED = "APPLICATION.STARTED";

        /// <summary>
        /// The application is requesting to create a profile.
        /// </summary>
        public const string PROFILE_CREATE = "APPLICATION.PROFILE_CREATE";

        /// <summary>
        /// The application has created a profile.
        /// </summary>
        public const string PROFILE_CREATED = "APPLICATION.PROFILE_CREATED";

        /// <summary>
        /// The application is requesting to edit a profile.
        /// </summary>
        public const string PROFILE_EDIT = "APPLICATION.PROFILE_EDIT";

        /// <summary>
        /// The application has edited a profile.
        /// </summary>
        public const string PROFILE_EDITED = "APPLICATION.PROFILE_EDITED";

        /// <summary>
        /// The application is requesting to open a profile.
        /// </summary>
        public const string PROFILE_OPEN = "APPLICATION.PROFILE_OPEN";

        /// <summary>
        /// The application has opened a profile.
        /// </summary>
        public const string PROFILE_OPENED = "APPLICATION.PROFILE_OPENED";

        /// <summary>
        /// The application is requesting to create an account.
        /// </summary>
        public const string ACCOUNT_CREATE = "APPLICATION.ACCOUNT_CREATE";

        /// <summary>
        /// The application has created an account.
        /// </summary>
        public const string ACCOUNT_CREATED = "APPLICATION.ACCOUNT_CREATED";

        /// <summary>
        /// The application is requesting to edit an account.
        /// </summary>
        public const string ACCOUNT_EDIT = "APPLICATION.ACCOUNT_EDIT";

        /// <summary>
        /// The application has edited an account.
        /// </summary>
        public const string ACCOUNT_EDITED = "APPLICATION.ACCOUNT_EDITED";

        #endregion
    }
}
