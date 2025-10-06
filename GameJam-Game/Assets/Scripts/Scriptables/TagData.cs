using UnityEngine;

namespace Nidavellir.Scriptables
{
    [CreateAssetMenu(fileName = "Tag", menuName = "Data/Tag", order = 0)]
    public class TagData : ScriptableObject
    {
        [SerializeField] private string m_name;
        
        public string Name => this.m_name;
    }
}