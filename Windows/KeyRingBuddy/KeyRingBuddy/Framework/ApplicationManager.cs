using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRingBuddy.Framework
{
    /// <summary>
    /// This class is used to help manage the application.
    /// </summary>
    public class ApplicationManager<TController> where TController : IController
    {
        #region Fields

        /// <summary>
        /// Holds listeners with the event name used as the key.
        /// </summary>
        private IDictionary<string, IList<IEventListener>> _listenerMap;

        /// <summary>
        /// Holds the controllers being displayed.
        /// </summary>
        private IList<TController> _controllerList;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationManager()
        {
            _listenerMap = new Dictionary<string, IList<IEventListener>>();
            _controllerList = new List<TController>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The current controller that is being displayed.
        /// </summary>
        public TController CurrentController
        {
            get
            {
                if (_controllerList.Count == 0)
                    return default(TController);
                else
                    return _controllerList[_controllerList.Count - 1];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Classes that inherit this one should override this method to provide the custom logic
        /// needed to display the controller.
        /// </summary>
        /// <param name="controller">The controller to display.</param>
        protected virtual void OpenController(TController controller)
        {
            if (controller != null)
                controller.Opened();
        }

        /// <summary>
        /// Classes that inherit this one should override this method to provide custom logic
        /// needed to hide the controller.
        /// </summary>
        /// <param name="controller">The controller to hide.</param>
        /// <returns>true if the controller was closed, false if it wasn't.</returns>
        protected virtual bool CloseController(TController controller)
        {
            if (controller != null)
            {
                if (!controller.Closing())
                    return false;

                controller.Closed();
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Load the given controller.
        /// </summary>
        /// <param name="controller">The controller to load.</param>
        /// <returns>true if the controller was loaded, false if it wasn't.</returns>
        public bool LoadController(TController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (!CloseController(CurrentController))
                return false;

            _controllerList.Add(controller);

            OpenController(controller);

            return true;
        }

        /// <summary>
        /// Unload the current controller.
        /// </summary>
        /// <param name="controller">The controller to unload.</param>
        /// <returns>true if the controller was unloaded, false if it wasn't.</returns>
        public bool UnloadController(TController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (!CloseController(controller))
                return false;

            RemoveController(controller);

            return true;
        }

        /// <summary>
        /// Remove the controller from the list.
        /// </summary>
        /// <param name="controller">The controller to remove.</param>
        protected void RemoveController(TController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            int index = _controllerList.IndexOf(controller);
            if (index != -1)
                _controllerList.RemoveAt(index);
        }

        /// <summary>
        /// Register the listener for the given event.
        /// </summary>
        /// <param name="listener">The listener to register.</param>
        public void RegisterListener(IEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException("listener");

            IEnumerable<string> eventNames = listener.GetRegisteredEventNames();
            if (eventNames == null)
                return;

            foreach (string eventName in eventNames)
            {
                string key = NormalizeEventName(eventName);

                if (!_listenerMap.ContainsKey(key))
                    _listenerMap.Add(key, new List<IEventListener>());

                _listenerMap[key].Add(listener);
            }
        }

        /// <summary>
        /// Unregister the listener from the given event.
        /// </summary>
        /// <param name="listener">The listener to unregister.</param>        
        public void UnregisterListener(IEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException("listener");

            List<string> emptyLists = new List<string>();
            foreach (KeyValuePair<string, IList<IEventListener>> kvp in _listenerMap)
            {
                kvp.Value.Remove(listener);
                if (kvp.Value.Count == 0)
                    emptyLists.Add(kvp.Key);
            }

            foreach (string key in emptyLists)
                _listenerMap.Remove(key);
        }

        /// <summary>
        /// Raise an application event.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <param name="arguments">Optional arguments to pass with the event.</param>
        /// <returns>The result from raising the event.</returns>
        public bool RaiseEvent(string eventName, params object[] arguments)
        {
            if (String.IsNullOrWhiteSpace(eventName))
                throw new ArgumentException("eventName is null or whitespace.", "eventName");

            string key = NormalizeEventName(eventName);
            if (key == "*")
                throw new ArgumentException("'*' is a reserved event name and can't be used here.", "eventName");

            bool result = true;

            if (_listenerMap.ContainsKey(key))
            {
                foreach (IEventListener listener in _listenerMap[key])
                {
                    result &= listener.EventRaised(
                        NormalizeEventName(eventName, listener.GetRegisteredEventNames()), 
                        arguments);
                }
            }

            if (_listenerMap.ContainsKey("*"))
            {
                foreach (IEventListener listener in _listenerMap["*"])
                {
                    result &= listener.EventRaised(
                        NormalizeEventName(eventName, listener.GetRegisteredEventNames()),
                        arguments);
                }
            }

            return result;
        }

        /// <summary>
        /// Raise an application event.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <returns>The result from raising the event.</returns>
        public bool RaiseEvent(string eventName)
        {
            return RaiseEvent(eventName, null);
        }

        /// <summary>
        /// Normalizes the event name passed in.
        /// </summary>
        /// <param name="eventName">The event name to normalize.</param>
        /// <returns>The normalized event name.</returns>
        private string NormalizeEventName(string eventName)
        {
            if (eventName == null)
                return String.Empty;
            else
                return eventName.Trim().ToUpper();
        }

        /// <summary>
        /// Normalize the given event name using the list of available names.
        /// </summary>
        /// <param name="eventName">The event name to normalize.</param>
        /// <param name="names">The names to normalize against.</param>
        /// <returns>The normalized name.</returns>
        private string NormalizeEventName(string eventName, IEnumerable<string> names)
        {
            if (names == null || eventName == null)
                return eventName;

            foreach (string name in names)
            {
                if (String.Compare(name.Trim(), eventName.Trim(), true) == 0)
                    return name;
            }

            return eventName;
        }

        #endregion
    }
}
