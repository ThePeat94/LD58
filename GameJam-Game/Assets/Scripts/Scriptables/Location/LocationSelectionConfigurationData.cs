using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "Location Configuration", menuName = "Data/Location/Configuration", order = 0)]
    public class LocationSelectionConfigurationData : ScriptableObject
    {
        [SerializeField] private float m_eventLocationChance;
        [SerializeField] private int m_numberOfLocationsToSelect;
        
        public float EventLocationChance => this.m_eventLocationChance;
        public int NumberOfLocationsToSelect => this.m_numberOfLocationsToSelect;
    }
}