﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Ap.Email.Mvvm
{
    /// <summary>
    /// Handles management and dispatching of EventHandlers in a weak way.
    /// 
    /// </summary>
    public static class WeakEventHandlerManager
    {
        private static readonly SynchronizationContext syncContext = SynchronizationContext.Current;

        static WeakEventHandlerManager()
        {
        }

        /// <summary>
        /// Invokes the handlers
        /// 
        /// </summary>
        /// <param name="sender"/><param name="handlers"/>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers == null)
                return;
            var callees = new EventHandler[handlers.Count];
            int count = 0;
            int num = WeakEventHandlerManager.CleanupOldHandlers(handlers, callees, count);
            for (int index = 0; index < num; ++index)
                WeakEventHandlerManager.CallHandler(sender, callees[index]);
        }

        private static void CallHandler(object sender, EventHandler eventHandler)
        {
            if (eventHandler == null)
                return;
            if (WeakEventHandlerManager.syncContext != null)
                WeakEventHandlerManager.syncContext.Post((SendOrPostCallback)(o => eventHandler(sender, EventArgs.Empty)), (object)null);
            else
                eventHandler(sender, EventArgs.Empty);
        }

        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count)
        {
            for (int index = handlers.Count - 1; index >= 0; --index)
            {
                var eventHandler = handlers[index].Target as EventHandler;
                if (eventHandler == null)
                {
                    handlers.RemoveAt(index);
                }
                else
                {
                    callees[count] = eventHandler;
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Adds a handler to the supplied list in a weak way.
        /// 
        /// </summary>
        /// <param name="handlers">Existing handler list.  It will be created if null.</param><param name="handler">Handler to add.</param><param name="defaultListSize">Default list size.</param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            handlers.Add(new WeakReference((object)handler));
        }

        /// <summary>
        /// Removes an event handler from the reference list.
        /// 
        /// </summary>
        /// <param name="handlers">Handler list to remove reference from.</param><param name="handler">Handler to remove.</param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers == null)
                return;
            for (int index = handlers.Count - 1; index >= 0; --index)
            {
                var eventHandler = handlers[index].Target as EventHandler;
                if (eventHandler == null || eventHandler == handler)
                    handlers.RemoveAt(index);
            }
        }
    }
}
