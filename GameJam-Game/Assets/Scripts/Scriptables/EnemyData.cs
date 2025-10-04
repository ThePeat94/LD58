using System.Collections.Generic;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/Enemy", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private int m_id;
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private string m_profileDescription;
        [SerializeField] private List<StatRange> m_initialStats;

        public int ID => this.m_id;
        public string Name => this.m_name;
        public Sprite Icon => this.m_icon;
        public string ProfileDescription => this.m_profileDescription;
        public List<StatRange> InitialStats => this.m_initialStats;
    }
}