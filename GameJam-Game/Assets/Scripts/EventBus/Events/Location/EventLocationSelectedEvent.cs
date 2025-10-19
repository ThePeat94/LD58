using Nidavellir.Scriptables.Location;

namespace Nidavellir.EventBus.Events.Location
{
    public class EventLocationSelectedEvent : IEvent
    {
        public EventLocationSelectedEvent(EventLocationData selectedEventLocation)
        {
            this.SelectedEventLocation = selectedEventLocation;
        }
        
        public EventLocationData SelectedEventLocation { get; }
    }
}