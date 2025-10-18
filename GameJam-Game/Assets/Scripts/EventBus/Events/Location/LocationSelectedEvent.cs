using Nidavellir.Scriptables.Location;

namespace Nidavellir.EventBus.Events.Location
{
    public class LocationSelectedEvent : IEvent
    {
        public LocationSelectedEvent(BaseLocationData locationData)
        {
            this.LocationData = locationData;
        }
        
        public BaseLocationData LocationData { get; }
    }
}