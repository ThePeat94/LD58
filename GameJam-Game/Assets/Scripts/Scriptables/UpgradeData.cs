using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Upgrade Card", menuName = "Data/Upgrade", order = 0)]
    public class UpgradeData : ScriptableObject
    {
        [SerializeField] private int m_id;
        [SerializeField] private string m_name;
        [SerializeField] private List<CharacterStatIncrease> m_affectedStats;
        [SerializeField] private int m_cost;
        [SerializeField] private Sprite m_cardImage;
        
        public List<CharacterStatIncrease> AffectedStats => this.m_affectedStats;
        public int Cost => this.m_cost;
        public Sprite CardImage => this.m_cardImage;

        [Serializable]
        public class CharacterStatIncrease
        {
            [SerializeField] private CharacterStat m_affectedStat;
            [SerializeField] private int m_increaseAmount;
            
            public CharacterStat AffectedStat => this.m_affectedStat;
            public int IncreaseAmount => this.m_increaseAmount;
        }
    }
}