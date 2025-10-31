using System;
using Nidavellir.Entity;
using Nidavellir.GameState;
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

        private void Awake()
        {
            this.m_rerollManager ??= FindFirstObjectByType<RerollManager>(FindObjectsInactive.Include);
            this.m_shopManager ??= FindFirstObjectByType<ShopManager>(FindObjectsInactive.Include);
            this.m_gameStateManager ??= FindFirstObjectByType<GameStateManager>(FindObjectsInactive.Include);
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include); 
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
                StatBuffEventLocationData statBuffEventLocationData => new StatBuffEventLocationHandler(this.m_playerStats, statBuffEventLocationData),
                _ => throw new ArgumentException($"Unknown event location type {eventLocation.GetType()}")
            };
        }
    }
}