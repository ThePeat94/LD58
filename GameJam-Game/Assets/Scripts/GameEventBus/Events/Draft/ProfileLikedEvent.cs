using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileLikedEvent : IEvent
    {
        public EnemyData EnemyData
        {
            get;
            private set;
        }

        public ProfileLikedEvent(EnemyData enemy)
        {
            this.EnemyData = enemy;
        }
    }

}