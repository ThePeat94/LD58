using System;
using System.Collections.Generic;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events;
using UnityEngine;

namespace Nidavellir.EventBus
{
    public static class GameEventBus<T> where T : IEvent
    {
        private static readonly SortedList<int, HashSet<IEventBinding<T>>> s_sortedEventBindings = new();
        
        public static void Register(IEventBinding<T> eventBinding, int priority = 0)
        {
            if (!s_sortedEventBindings.ContainsKey(priority))
            {
                s_sortedEventBindings[priority] = new HashSet<IEventBinding<T>>();
            }
            
            s_sortedEventBindings[priority].Add(eventBinding);
        }

        public static void Unregister(IEventBinding<T> eventBinding)
        {
            foreach (var bindingsSet in s_sortedEventBindings.Values)
            {
                if (bindingsSet.Remove(eventBinding))
                {
                    break;
                }
            }
        }
    
        public static void Invoke(object sender, T args)
        {
            var bindingsCopy = new SortedDictionary<int, HashSet<IEventBinding<T>>>(s_sortedEventBindings);
            foreach (var (prio, eventBindings) in bindingsCopy)
            {
                Debug.Log($"Invoking event bindings with priority {prio} for event {typeof(T).Name}");
                foreach (var eventBinding in eventBindings)
                {
                    try
                    {
                        eventBinding.Invoke(sender, args);
                    } 
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error invoking event binding: {ex}");
                    }
                }

            }
        }
    }
}