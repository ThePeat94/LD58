using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "Location", menuName = "Data/Location/Enemy", order = 0)]
    public class EnemyLocationData : BaseLocationData
    {
        [SerializeField] private List<EnemyData> m_availableNonBossProfiles;
        [SerializeField] private List<EnemyData> m_availableBossProfiles;

        public List<EnemyData> AvailableNonBossProfiles => this.m_availableNonBossProfiles;
        public List<EnemyData> AvailableBossProfiles => this.m_availableBossProfiles;
    }
}