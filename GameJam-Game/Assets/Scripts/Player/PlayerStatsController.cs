using System;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;
        
        private void Awake()
        {
            this.m_startDraftEventBinding = new EventBinding<StartDraftEvent>(this.OnStartDraftEvent);
            GameEventBus<StartDraftEvent>.Register(this.m_startDraftEventBinding);
        }

        private void OnStartDraftEvent(object sender, StartDraftEvent e)
        {
            this.m_entityStats[this.m_characterStatFacade.Likes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.Dislikes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.SuperLike].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.Hp].ResetToMax();
        }
        
        private void OnDestroy()
        {
            GameEventBus<StartDraftEvent>.Unregister(this.m_startDraftEventBinding);
        }
    }
}