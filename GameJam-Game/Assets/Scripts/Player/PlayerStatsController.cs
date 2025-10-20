using System;
using Nidavellir.Entity;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private IEventBinding<LocationDraftStartedEvent> m_startDraftEventBinding;
        
        private void Awake()
        {
            // TODO: This should also be executed in a more direct manner and not by some events thrown at several places. Oh man.
            this.m_startDraftEventBinding = new EventBinding<LocationDraftStartedEvent>(this.OnStartLocationDraft);
            GameEventBus<LocationDraftStartedEvent>.Register(this.m_startDraftEventBinding);
        }

        private void OnStartLocationDraft(object sender, LocationDraftStartedEvent e)
        {
            this.m_entityStats[this.m_characterStatFacade.Likes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.Dislikes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.SuperLike].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.Hp].ResetToMax();
        }
        
        private void OnDestroy()
        {
            GameEventBus<LocationDraftStartedEvent>.Unregister(this.m_startDraftEventBinding);
        }
    }
}