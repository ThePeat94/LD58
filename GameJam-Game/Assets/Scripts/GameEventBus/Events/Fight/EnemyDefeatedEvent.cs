using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Fight
{
    public class EnemyDefeatedEvent : IEvent
    {
        public EnemyDefeatedEvent(EnemyData defeatedEnemy)
        {
            this.DefeatedEnemy = defeatedEnemy;
        }
        
        public EnemyData DefeatedEnemy { get; }
    }
}