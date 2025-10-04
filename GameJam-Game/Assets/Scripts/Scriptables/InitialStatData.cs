using System.Collections.Generic;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Initial Stat Data", menuName = "Data/Initial Stat Data", order = 0)]
    public class InitialStatData : ScriptableObject
    {
        [SerializeField] private List<InitialStatValue> m_initialStats;
        
        public List<InitialStatValue> InitialStats => this.m_initialStats;
    }
}