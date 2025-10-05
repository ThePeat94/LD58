using Nidavellir.Player;
using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Fight
{
    public class EnemyDefeatedEvent : IEvent
    {
        public EnemyDefeatedEvent(EntityInformation defeatedEnemy)
        {
            this.DefeatedEnemy = defeatedEnemy;
        }
        
        public EntityInformation DefeatedEnemy { get; }
    }
}