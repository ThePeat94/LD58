using Nidavellir.Entity;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using UnityEngine;

namespace Nidavellir.Player
{
    public class EntityInformation : MonoBehaviour
    {
        [SerializeField] private string m_name;
        [SerializeField] private Sprite m_profilePicture;
        [SerializeField] private Sprite m_backgroundImage;
        [SerializeField] private EntityStats m_entityStats;
        
        public string Name => this.m_name;
        public Sprite ProfilePicture => this.m_profilePicture;
        public Sprite BackgroundImage => this.m_backgroundImage;
        public EntityStats EntityStats => this.m_entityStats;

        public void Init(RuntimeEnemyInformation enemyData, EntityStats entityStats)
        {
            this.m_entityStats = entityStats;
            this.m_name = enemyData.BaseData.Name;
            this.m_profilePicture = enemyData.BaseData.Icon;
            if (enemyData.BaseData.PossibleBackgrounds is null or { Count: 0 })
            {
                this.m_backgroundImage = null;
            }
            else
            {
                this.m_backgroundImage = enemyData.BaseData.PossibleBackgrounds[Random.Range(0, enemyData.BaseData.PossibleBackgrounds.Count)];
            }
        }
    }
}