using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "Heal Fountain Location", menuName = "Data/Location/Event/Heal Fountain", order = 0)]
    public class HealFountainEventLocationData : EventLocationData
    {
        [SerializeField] private int m_healingAmount;
        
        public int HealingAmount => this.m_healingAmount;
    }
}