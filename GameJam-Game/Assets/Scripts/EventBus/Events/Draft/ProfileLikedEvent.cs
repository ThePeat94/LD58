using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.EventBus.Events.Draft
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