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
    /// Defines interface for classes that respond to events.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Get the names of the events to register this listener for.
        /// </summary>
        /// <returns>The names of the events to register this listener for.</returns>
        IEnumerable<string> GetRegisteredEventNames();

        /// <summary>
        /// Called when an event is raised.
        /// </summary>
        /// <param name="eventName">The name of the event that was fired.</param>
        /// <param name="arguments">Arguments for the event if there are any.</param>
        /// <returns>true if the event was processed successfully, false if it was not.</returns>
        bool EventRaised(string eventName, object[] arguments);
    }
}
