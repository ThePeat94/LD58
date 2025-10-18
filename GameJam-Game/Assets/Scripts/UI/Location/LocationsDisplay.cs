using System.Collections.Generic;
using Nidavellir.Scriptables.Location;
using UnityEngine;

namespace Nidavellir.UI.Location
{
    public class LocationsDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject m_locationsCardParent;
        [SerializeField] private LocationCardUI m_locationCardUiPrefab;
        

        private List<LocationCardUI> m_currentlyDisplayed = new();
        
        public void ShowLocations(List<BaseLocationData> locationCards)
        {
            this.ClearLocations();
            
            foreach (var toDisplay in locationCards)
            {
                var locationCard = Instantiate(this.m_locationCardUiPrefab, this.m_locationsCardParent.transform);
                locationCard.ShowLocation(toDisplay);
                this.m_currentlyDisplayed.Add(locationCard);
            }
        }
        
        private void ClearLocations()
        {
            if (this.m_currentlyDisplayed == null) return;
            
            foreach (var locationCard in this.m_currentlyDisplayed)
            {
                Destroy(locationCard.gameObject);
            }

            this.m_currentlyDisplayed.Clear();
        }
    }
}