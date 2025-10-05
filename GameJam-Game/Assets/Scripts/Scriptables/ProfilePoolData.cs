using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Profile Pool", menuName = "Data/Profile Pool", order = 0)]
    public class ProfilePoolData : ScriptableObject
    {
        [SerializeField] private List<EnemyData> m_nonBossProfiles;
        [SerializeField] private List<EnemyData> m_bossProfiles;

        public List<EnemyData> NonBossProfiles => this.m_nonBossProfiles;
        public List<EnemyData> BossProfiles => this.m_bossProfiles;
    }
}