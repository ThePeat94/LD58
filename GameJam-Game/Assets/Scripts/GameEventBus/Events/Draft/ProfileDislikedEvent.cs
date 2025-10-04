using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Draft
{
    public class ProfileDislikedEvent : IEvent
    {
        public EnemyData EnemyData
        {
            get;
            private set;
        }

        public ProfileDislikedEvent(EnemyData enemy)
        {
            this.EnemyData = enemy;
        }
    }

}