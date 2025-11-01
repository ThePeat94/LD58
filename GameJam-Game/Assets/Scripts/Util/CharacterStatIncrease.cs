using System;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Util
{
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