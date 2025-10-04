using System;

namespace Nidavellir.GameEventBus.EventBindings
{
    public interface IEventBinding<T>
    {
        public event EventHandler NoArgsHandler;
        public event EventHandler<T> Handler;
    
        public void Invoke(object sender);
        public void Invoke(object sender, T args);
    }
}