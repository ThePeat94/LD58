using System;

namespace Nidavellir.EventBus.EventBindings
{
    public interface IEventBinding<T>
    {
        public event EventHandler<T> Handler;
    
        public void Invoke(object sender, T args);
    }
}