using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileDislikedEvent : IEvent
    {
        public RuntimeEnemyInformation EnemyData
        {
            get;
            private set;
        }

        public ProfileDislikedEvent(RuntimeEnemyInformation enemy)
        {
            this.EnemyData = enemy;
        }
    }

}