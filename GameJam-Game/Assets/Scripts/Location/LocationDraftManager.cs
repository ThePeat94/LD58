using System;
using System.Collections.Generic;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Scriptables.Location;
using Nidavellir.UI.Location;
using Nidavellir.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nidavellir.Location
{
    public class LocationDraftManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyLocationData> m_availableEnemyLocations;
        [SerializeField] private List<EventLocationData> m_availableEventLocations;
        [SerializeField] private LocationSelectionConfigurationData m_locationSelectionConfigurationData;

        [SerializeField] private EventLocationData m_returnToShopEventLocation;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;

        [SerializeField] private LocationSelectionUI m_locationSelectionUI;
        
        private readonly List<BaseLocationData> m_selectedLocations = new();
        private BaseLocationData m_lastSelectedLocation;
        
        private IEventBinding<LocationSelectedEvent> m_locationSelectedEventBinding;

        private void Awake()
        {
            this.m_locationSelectionUI ??= FindFirstObjectByType<LocationSelectionUI>(FindObjectsInactive.Include);
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include);
            
            this.m_locationSelectedEventBinding = new EventBinding<LocationSelectedEvent>(this.OnLocationSelected);
            GameEventBus<LocationSelectedEvent>.Register(this.m_locationSelectedEventBinding);
        }

        private void Start()
        {
            this.SelectLocations();
        }

        public void SelectLocations()
        {
            var shouldSelectShopLocation = this.m_bountyRequirementController.HasFulfilledBountyRequirement();
            
            var shouldSelectEventLocation = !shouldSelectShopLocation && Random.value <= this.m_locationSelectionConfigurationData.EventLocationChance;
            var enemyLocationsToSelect = (shouldSelectEventLocation || shouldSelectShopLocation) ? 2 : 3;
            
            // add rnd enemies (no duplicates)
            for (var i = 0; i < enemyLocationsToSelect; i++)
            {
                EnemyLocationData selectedEnemyLocation;
                do
                {
                    var rndIndex = Random.Range(0, this.m_availableEnemyLocations.Count);
                    selectedEnemyLocation = this.m_availableEnemyLocations[rndIndex];
                } while (this.m_selectedLocations.Contains(selectedEnemyLocation));
                
                this.m_selectedLocations.Add(selectedEnemyLocation);
            }
            
            if (shouldSelectEventLocation)
            {
                var rndIndex = Random.Range(0, this.m_availableEventLocations.Count);
                var selectedEventLocation = this.m_availableEventLocations[rndIndex];
                this.m_selectedLocations.Add(selectedEventLocation);
            }
            
            if (shouldSelectShopLocation)
            {
                this.m_selectedLocations.Add(this.m_returnToShopEventLocation);
            }
            
            this.m_locationSelectionUI.ShowLocations(this.m_selectedLocations.Shuffle());
            GameEventBus<LocationDraftStartedEvent>.Invoke(this, new());
        }
        
        private void OnLocationSelected(object sender, LocationSelectedEvent e)
        {
            this.m_selectedLocations.Clear();
            this.m_lastSelectedLocation = e.SelectedLocation;
            
            switch (e.SelectedLocation)
            {
                case EnemyLocationData enemyLocation:
                    GameEventBus<EnemyLocationSelectedEvent>.Invoke(this, new(enemyLocation));
                    return;
                case EventLocationData eventLocation:
                    GameEventBus<EventLocationSelectedEvent>.Invoke(this, new(eventLocation));
                    eventLocation.TriggerEvent();
                    return;
            }
        }
    }
}