using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;

namespace Nidavellir.EventBus.Events.Draft
{
    public class ProfileSuperLikedEvent : IEvent
    {
        public RuntimeEnemyInformation Enemy
        {
            get;
            private set;
        }

        public ProfileSuperLikedEvent(RuntimeEnemyInformation enemy)
        {
            this.Enemy = enemy;
        }
    }

}