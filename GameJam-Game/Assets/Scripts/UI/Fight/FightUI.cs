using Nidavellir.Player;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.UI.Fight
{
    public class FightUI : MonoBehaviour
    {
        [SerializeField] private ProfileFightCard m_playerProfileFightCard;
        [SerializeField] private ProfileFightCard m_enemyProfileFightCards;
        [SerializeField] private GameObject m_fightPanelUi;
        
        public void ShowFightUI()
        {
            this.m_fightPanelUi.SetActive(true);
        }

        public void InitPlayerCard(EntityInformation playerInformation)
        {
            this.m_playerProfileFightCard.DisplayProfile(playerInformation);
        }
        
        public void ShowEnemy(EntityInformation enemyInformation)
        {
            this.m_enemyProfileFightCards.DisplayProfile(enemyInformation);
        }
    }
}