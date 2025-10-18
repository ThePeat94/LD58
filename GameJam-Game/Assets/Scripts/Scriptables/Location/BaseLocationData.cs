using UnityEngine;

namespace Nidavellir.Scriptables.Location
{
    public abstract class BaseLocationData : ScriptableObject
    {
        [SerializeField] protected string m_name;
        [SerializeField] protected string m_description;
        [SerializeField] protected Sprite m_backgroundImage;
        
        public string Name => this.m_name;
        public string Description => this.m_description;
        public Sprite BackgroundImage => this.m_backgroundImage;
    }
}