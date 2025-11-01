using System.Collections.Generic;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    [CreateAssetMenu(fileName = "StatBuffEventLocationData", menuName = "Data/Location/Event/Stat Buff")]
    public class StatBuffEventLocationData : EventLocationData
    {
        [SerializeField] private List<CharacterStatIncrease> m_characterStatIncreases;
        
        public List<CharacterStatIncrease> CharacterStatIncreases => this.m_characterStatIncreases;
    }
}