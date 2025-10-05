using UnityEngine;

namespace Nidavellir.Scriptables.Scaling
{
    [CreateAssetMenu(fileName = "Stat Weight", menuName = "Data/Stats/Weight", order = 0)]
    public class StatWeightData : ScriptableObject
    {
        [SerializeField] private CharacterStat m_stat;
        [SerializeField] private float m_weight;
        
        public CharacterStat Stat => this.m_stat;
        public float Weight => this.m_weight;
    }
}