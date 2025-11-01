using System;
using Nidavellir.Entity;
using Nidavellir.GameState;
using Nidavellir.Location.Handler;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Location;
using Nidavellir.Shop;
using UnityEngine;

namespace Nidavellir.Location
{
    public class EventLocationFactory : MonoBehaviour
    {
        [SerializeField] private RerollManager m_rerollManager;
        [SerializeField] private ShopManager m_shopManager;
        [SerializeField] private GameStateManager m_gameStateManager;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private LocationDraftManager m_locationDraftManager;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        
        private void Awake()
        {
            this.m_rerollManager ??= FindFirstObjectByType<RerollManager>(FindObjectsInactive.Include);
            this.m_shopManager ??= FindFirstObjectByType<ShopManager>(FindObjectsInactive.Include);
            this.m_gameStateManager ??= FindFirstObjectByType<GameStateManager>(FindObjectsInactive.Include);
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include); 
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
            this.m_locationDraftManager  ??= FindFirstObjectByType<LocationDraftManager>(FindObjectsInactive.Include);
        }

        public IEventLocationHandler CreateHandler(EventLocationData eventLocation)
        {
            return eventLocation switch
            {
                VisitTavernEventData _ => new TavernEventLocationHandler(
                    this.m_shopManager, 
                    this.m_rerollManager, 
                    this.m_gameStateManager,
                    this.m_bountyRequirementController
                ),
                StatBuffEventLocationData statBuffEventLocationData => new StatBuffEventLocationHandler(this.m_playerStats, statBuffEventLocationData, this.m_locationDraftManager), 
                HealFountainEventLocationData healFountainEventLocationData => new HealFountainLocationHandler(this.m_playerStats, this.m_characterStatFacade.Hp, healFountainEventLocationData, this.m_locationDraftManager),
                _ => throw new ArgumentException($"Unknown event location type {eventLocation.GetType()}")
            };
        }
    }
}