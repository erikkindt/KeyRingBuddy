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
