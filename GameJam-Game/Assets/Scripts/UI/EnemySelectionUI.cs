using System.Collections.Generic;
using Nidavellir.UI.Draft;
using Nidavellir.UI.Profile;
using UnityEngine;

namespace Nidavellir.UI
{
    public class EnemySelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_enemyDisplayRow;
        [SerializeField] private BaseProfileCardUI m_baseProfileCardPrefab;
        [SerializeField] private GameObject m_enemySelectionPanel;
        
        private readonly List<BaseProfileCardUI> m_displayedEnemies = new();

        public void Show(List<RuntimeEnemyInformation> profile)
        {
            this.m_enemySelectionPanel.SetActive(true);
            this.Clear();
            foreach (var toDisplay in profile)
            {
                var profileCard = Instantiate(this.m_baseProfileCardPrefab, this.m_enemyDisplayRow.transform);
                profileCard.DisplayEnemy(toDisplay);
                this.m_displayedEnemies.Add(profileCard);
            }
        }
        
        private void Clear()
        {
            foreach (var displayedEnemy in this.m_displayedEnemies)
            {
                Destroy(displayedEnemy.gameObject);
            }
            
            this.m_displayedEnemies.Clear();
        }
    }
}