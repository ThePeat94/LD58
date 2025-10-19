using Nidavellir.Scriptables.Location;

namespace Nidavellir.EventBus.Events.Shop
{
    public class StartEnemyDraftEvent : IEvent
    {
        public EnemyLocationData SelectedLocation { get; }
        
        public StartEnemyDraftEvent(EnemyLocationData selectedLocation)
        {
            this.SelectedLocation = selectedLocation;
        }
    }
}