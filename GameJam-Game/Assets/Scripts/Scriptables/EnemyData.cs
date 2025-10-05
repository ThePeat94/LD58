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
        [SerializeField] private int m_age;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private string m_profileDescription;
        [SerializeField] private InitialStatData m_initialStats;
        [SerializeField] private List<Sprite> m_possibleBackgrounds;
        
        
        public int ID => this.m_id;
        public string Name => this.m_name;
        public int Age => this.m_age;
        public Sprite Icon => this.m_icon;
        public string ProfileDescription => this.m_profileDescription;
        public InitialStatData InitialStats => this.m_initialStats;
        public List<Sprite> PossibleBackgrounds => this.m_possibleBackgrounds;
    }
}