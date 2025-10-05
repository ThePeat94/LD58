using UnityEngine;


namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Stat", menuName = "Data/Stats/Stat", order = 0)]
    public class CharacterStat : ScriptableObject
    {
        [SerializeField] private string m_id;
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_icon;

        public string Id => this.m_id;
        public string Name => this.m_name;
        public Sprite Icon => this.m_icon;
    }
}