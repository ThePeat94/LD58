using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerRewardController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private IEventBinding<EnemyDefeatedEvent> m_enemyDefeatedEvent;
        
        private void Awake()
        {
            this.m_enemyDefeatedEvent = new EventBinding<EnemyDefeatedEvent>(this.OnEnemyDefeated);
            GameEventBus<EnemyDefeatedEvent>.Register(this.m_enemyDefeatedEvent);
        }

        private void OnEnemyDefeated(object sender, EnemyDefeatedEvent e)
        {
            this.m_playerStats[this.m_characterStatFacade.Money].Add(e.DefeatedEnemy.EntityStats[this.m_characterStatFacade.Money]?.CurrentValue ?? 0);
        }
    }
}