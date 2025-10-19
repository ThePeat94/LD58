using System;
using Nidavellir.EventBus.Events;

namespace Nidavellir.EventBus.EventBindings
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        private EventHandler<T> m_handler;

        public event EventHandler<T> Handler
        {
            add => this.m_handler += value;
            remove => this.m_handler -= value;
        }


        public EventBinding(EventHandler<T> onEvent)
        {
            this.m_handler += onEvent;
        }

        public void Invoke(object sender, T args)
        {
            this.m_handler?.Invoke(sender, args);
        }
    }
}