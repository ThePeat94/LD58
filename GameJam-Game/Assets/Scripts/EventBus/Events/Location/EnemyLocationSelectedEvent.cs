using Nidavellir.Scriptables.Location;

namespace Nidavellir.EventBus.Events.Location
{
    public class EnemyLocationSelectedEvent : IEvent
    {
        public EnemyLocationSelectedEvent(EnemyLocationData location)
        {
            this.Location = location;
        }
        
        public EnemyLocationData Location { get; }
    }
}