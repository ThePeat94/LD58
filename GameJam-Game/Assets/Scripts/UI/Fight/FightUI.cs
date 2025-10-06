using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Fight
{
    public class FightUI : MonoBehaviour
    {
        [SerializeField] private Button m_goToShopButton;
        [SerializeField] private ProfileFightCard m_playerProfileFightCard;
        [SerializeField] private ProfileFightCard m_enemyProfileFightCards;
        [SerializeField] private GameObject m_fightPanelUi;
        [SerializeField] private GameObject m_duringFightPanelUi;
        [SerializeField] private GameObject m_afterFightPanelUi;


        private void Awake()
        {
            this.m_goToShopButton.onClick.AddListener(this.OnGoToShopClick);
        }

        private void OnGoToShopClick()
        {
            this.m_duringFightPanelUi.SetActive(false);
            this.m_afterFightPanelUi.SetActive(false);
            this.m_fightPanelUi.SetActive(false);
            GameEventBus<VisitShopEvent>.Invoke(this, new VisitShopEvent());
        }

        public void ShowFightUI()
        {
            this.m_fightPanelUi.SetActive(true);
            this.m_duringFightPanelUi.SetActive(true);
            this.m_afterFightPanelUi.SetActive(false);
        }
        
        public void ShowAfterFightUI()
        {
            this.m_duringFightPanelUi.SetActive(false);
            this.m_afterFightPanelUi.SetActive(true);
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