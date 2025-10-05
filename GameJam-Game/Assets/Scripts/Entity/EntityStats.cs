using System.Collections.Generic;
using System.Linq;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using UnityEngine;

namespace Nidavellir.Entity
{
    public class EntityStats : MonoBehaviour
    {
        [SerializeField] private InitialStatData m_initialStatData;
        
        private Dictionary<CharacterStat, StatController> m_currentCharacterStats = new();
        
        public IReadOnlyDictionary<CharacterStat, StatController> CurrentCharacterStats => this.m_currentCharacterStats;

        public StatController this[CharacterStat stat]
        {
            get
            {
                if (!this.m_currentCharacterStats.ContainsKey(stat))
                {
                    return null;
                }

                return this.m_currentCharacterStats[stat];
            }
        }
        
        private void Awake()
        {
            if (this.m_initialStatData is not null)
            {
                this.InitStatDict(this.m_initialStatData);
            }
        }

        public void Init(InitialStatData initialStatData)
        {
            this.InitStatDict(initialStatData);
        }

        private void InitStatDict(InitialStatData initialStatData)
        {
            this.m_currentCharacterStats = 
                initialStatData
                    .InitialStats
                    .ToDictionary(kvp => kvp.CharacterStat, kvp => new StatController(kvp));
        }

        public void Init(RuntimeEnemyInformation enemyInformation)
        {
            this.m_currentCharacterStats = enemyInformation.Stats.ToDictionary(kvp => kvp.Key, kvp => new StatController(kvp.Value));
        }
    }
}