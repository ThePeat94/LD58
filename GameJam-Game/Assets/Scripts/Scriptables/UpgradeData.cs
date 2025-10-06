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
        [SerializeField] private List<TagData> m_affectedTags;
        
        public string Name => this.m_name;
        public List<CharacterStatIncrease> AffectedStats => this.m_affectedStats;
        public int Cost => this.m_cost;
        public Sprite CardImage => this.m_cardImage;
        public List<TagData> AffectedTags => this.m_affectedTags;

        [Serializable]
        public class CharacterStatIncrease
        {
            [SerializeField] private CharacterStat m_affectedStat;
            [SerializeField] private int m_increaseAmount;
            [SerializeField] private float m_relativeIncreaseAmount;
            
            public CharacterStat AffectedStat => this.m_affectedStat;
            public int IncreaseAmount => this.m_increaseAmount;
            public float RelativeIncreaseAmount => this.m_relativeIncreaseAmount;
        }
    }
}