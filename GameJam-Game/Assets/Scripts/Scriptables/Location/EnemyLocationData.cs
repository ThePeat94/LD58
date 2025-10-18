using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "Location", menuName = "Data/Location/Enemy", order = 0)]
    public class EnemyLocationData : BaseLocationData
    {
        [SerializeField] private List<EnemyData> m_availableProfiles;

        public List<EnemyData> AvailableProfiles => this.m_availableProfiles;
    }
}