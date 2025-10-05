using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileLikedEvent : IEvent
    {
        public RuntimeEnemyInformation EnemyData
        {
            get;
            private set;
        }

        public ProfileLikedEvent(RuntimeEnemyInformation enemy)
        {
            this.EnemyData = enemy;
        }
    }

}