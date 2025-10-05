using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileSuperLikedEvent : IEvent
    {
        public RuntimeEnemyInformation EnemyData
        {
            get;
            private set;
        }

        public ProfileSuperLikedEvent(RuntimeEnemyInformation enemy)
        {
            this.EnemyData = enemy;
        }
    }

}