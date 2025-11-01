using Nidavellir.Scriptables.Location;

namespace Nidavellir.EventBus.Events.Location
{
    public class LocationSelectedEvent : IEvent
    {
        public LocationSelectedEvent(BaseLocationData selectedLocation)
        {
            this.SelectedLocation = selectedLocation;
        }
        
        public BaseLocationData SelectedLocation { get; }
    }
}