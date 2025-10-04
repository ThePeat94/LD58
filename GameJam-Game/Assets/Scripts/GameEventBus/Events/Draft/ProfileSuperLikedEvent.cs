using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileSuperLikedEvent : IEvent
    {
        public EnemyData EnemyData
        {
            get;
            private set;
        }

        public ProfileSuperLikedEvent(EnemyData enemy)
        {
            this.EnemyData = enemy;
        }
    }

}