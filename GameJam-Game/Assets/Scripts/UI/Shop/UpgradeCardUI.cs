using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Shop
{
    public class UpgradeCardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_upgradeText;
        [SerializeField] private TextMeshProUGUI m_costText;
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Button m_buyButton;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        
        private EntityStats m_playerStats;
        
        private const string COST_FORMAT = "{0}€";
        
        private UpgradeData m_data;
        
        public Button BuyButton => m_buyButton;

        public void Show(UpgradeData data, EntityStats playerStats)
        {
            this.m_playerStats = playerStats;
            this.m_data = data;
            var upgradeText = "";
            foreach(var statIncrease in data.AffectedStats)
            {
                upgradeText += $"+{statIncrease.IncreaseAmount} {statIncrease.AffectedStat.Name}\n";
            }
            if (upgradeText.Length > 0)
            {
                upgradeText = upgradeText[..^1]; // Remove last newline
            }
            this.m_upgradeText.text = upgradeText;
            this.m_costText.text = String.Format(COST_FORMAT, data.Cost);
            
            this.m_playerStats[this.m_characterStatFacade.Money].OnValueChanged += this.OnMoneyChanged;
        }

        private void OnMoneyChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (this.m_data == null)
                return;
            
            this.m_buyButton.interactable = e.NewValue >= this.m_data.Cost;
        }

        private void OnDestroy()
        {
            if (this.m_playerStats == null)
                return;
            
            this.m_playerStats[this.m_characterStatFacade.Money].OnValueChanged -= this.OnMoneyChanged;
        }
    }
}