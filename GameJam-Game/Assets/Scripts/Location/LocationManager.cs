using System.Collections.Generic;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.Scriptables.Location;
using Nidavellir.UI.Location;
using UnityEngine;

namespace Nidavellir.Location
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyLocationData> m_availableEnemyLocations;
        [SerializeField] private List<EventLocationData> m_availableEventLocations;
        [SerializeField] private LocationSelectionConfigurationData m_locationSelectionConfigurationData;

        [SerializeField] private EventLocationData m_returnToShopEventLocation;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;

        [SerializeField] private LocationsDisplay m_locationsDisplay;
        
        private List<BaseLocationData> m_selectedLocations = new();
        
        private IEventBinding<LocationSelectedEvent> m_locationSelectedEventBinding;
        private IEventBinding<StartLocationDraftEvent> m_startLocationDraftEventBinding;
        
        private void Awake()
        {
            this.m_locationsDisplay ??= FindFirstObjectByType<LocationsDisplay>(FindObjectsInactive.Include);
            
            this.m_startLocationDraftEventBinding = new EventBinding<StartLocationDraftEvent>(this.OnStartLocationDraft);
            GameEventBus<StartLocationDraftEvent>.Register(this.m_startLocationDraftEventBinding);
        }
        
        private void SelectLocations()
        {
            var shouldSelectShopLocation = this.m_bountyRequirementController.HasFulfilledBountyRequirement();
            
            var shouldSelectEventLocation = !shouldSelectShopLocation && Random.value <= this.m_locationSelectionConfigurationData.EventLocationChance;
            var enemyLocationsToSelect = (shouldSelectEventLocation || shouldSelectShopLocation) ? 2 : 3;
            
            // add rnd enemies (no duplicates)
            for (int i = 0; i < enemyLocationsToSelect; i++)
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
            
            this.m_locationsDisplay.ShowLocations(this.m_selectedLocations);
        }

        private void OnStartLocationDraft(object sender, StartLocationDraftEvent e)
        {
            this.SelectLocations();
        }
    }
}