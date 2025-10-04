using System.Collections.Generic;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private List<InitialStatValue> m_stats;

        public List<InitialStatValue> Stats => this.m_stats;
    }
}