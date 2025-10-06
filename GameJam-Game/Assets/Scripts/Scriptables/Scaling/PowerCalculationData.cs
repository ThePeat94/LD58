using System.Collections.Generic;
using UnityEngine;

namespace Nidavellir.Scriptables.Scaling
{
    [CreateAssetMenu(fileName = "Power Calculation Data", menuName = "Data/Stats/Power Calculation", order = 0)]
    public class PowerCalculationData : ScriptableObject
    {
        [SerializeField] private List<StatWeightData> m_statWeights;
        [SerializeField] private float m_bountyPerPower;
        [SerializeField] private AnimationCurve m_bountyJitter;
        [SerializeField] private float m_superlikeAmplifier;
        
        
        public List<StatWeightData> StatWeights => this.m_statWeights;
        public float BountyPerPower => this.m_bountyPerPower;
        public AnimationCurve BountyJitter => this.m_bountyJitter;
        public float SuperlikeAmplifier => this.m_superlikeAmplifier;
    }
}