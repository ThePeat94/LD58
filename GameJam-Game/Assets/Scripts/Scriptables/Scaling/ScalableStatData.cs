using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir.Scriptables.Scaling
{
    [CreateAssetMenu(fileName = "Scalable Stat Data", menuName = "Data/Stats/Scaling", order = 0)]
    public class ScalableStatData : ScriptableObject
    {
        
        [SerializeField] private List<ScalableStatValue> m_scalableStats;
        
        public List<ScalableStatValue> ScalableStats => this.m_scalableStats;

        [Serializable]
        public class ScalableStatValue
        {
            [SerializeField] private CharacterStat m_stat;
            [SerializeField] private int m_baseValue;
            [SerializeField] private AnimationCurve m_scalingFactor;
            [SerializeField] private AnimationCurve m_varianceFactor;
        
            public CharacterStat Stat => this.m_stat;
            public int BaseValue => this.m_baseValue;
            public AnimationCurve ScalingFactor => this.m_scalingFactor;
            public AnimationCurve VarianceFactor => this.m_varianceFactor;
        }
    }
}