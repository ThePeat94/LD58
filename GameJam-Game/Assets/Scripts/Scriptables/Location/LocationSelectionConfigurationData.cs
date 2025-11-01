using System;
using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "Location Configuration", menuName = "Data/Location/Configuration", order = 0)]
    public class LocationSelectionConfigurationData : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float m_eventLocationChance;
        [SerializeField] private int m_numberOfLocationsToSelect;
        
        public float EventLocationChance => this.m_eventLocationChance;
        public int NumberOfLocationsToSelect => this.m_numberOfLocationsToSelect;

        private void OnValidate()
        {
            if (this.m_eventLocationChance <= 0f || this.m_eventLocationChance > 1f)
                Debug.LogError("Event location chance must be between 0 and 1.", this);
        }
    }
}