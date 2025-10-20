using System;
using System.Collections.Generic;
using Nidavellir.Draft;
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
        
        private Action<RuntimeEnemyInformation> m_onProfileCardClicked;
        
        private readonly List<BaseProfileCardUI> m_displayedEnemies = new();

        public event Action<RuntimeEnemyInformation> OnProfileLiked
        {
            add => this.m_onProfileCardClicked += value;
            remove => this.m_onProfileCardClicked -= value;
        }

        public void UpdateDisplayedProfiles(List<RuntimeEnemyInformation> profile)
        {
            this.Clear();
            foreach (var toDisplay in profile)
            {
                var profileCard = Instantiate(this.m_baseProfileCardPrefab, this.m_enemyDisplayRow.transform);
                profileCard.DisplayEnemy(toDisplay);
                profileCard.OnProfileCardClicked += this.LikeEnemy;
                this.m_displayedEnemies.Add(profileCard);
            }
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
            this.m_enemySelectionPanel.SetActive(true);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.m_enemySelectionPanel.SetActive(false);
        }
        
        private void Clear()
        {
            foreach (var displayedEnemy in this.m_displayedEnemies)
            {
                displayedEnemy.OnProfileCardClicked -= this.LikeEnemy;
                Destroy(displayedEnemy.gameObject);
            }
            
            this.m_displayedEnemies.Clear();
        }

        public void LikeEnemy(RuntimeEnemyInformation displayedEnemy)
        {
            this.m_onProfileCardClicked?.Invoke(displayedEnemy);
        }
    }
}