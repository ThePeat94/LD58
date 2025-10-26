using System;
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
                _ => throw new ArgumentException($"Unknown event location type {eventLocation.GetType()}")
            };
        }
    }
}