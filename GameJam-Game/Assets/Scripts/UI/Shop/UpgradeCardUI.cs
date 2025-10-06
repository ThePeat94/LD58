using System;
using System.Linq;
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
        
        private const string COST_FORMAT = "{0}g";
        
        private UpgradeData m_data;
        
        public Button BuyButton => this.m_buyButton;

        public void Show(UpgradeData data, EntityStats playerStats)
        {
            this.m_playerStats = playerStats;
            this.m_data = data;
            var upgradeText = "";
            var affectedTags = $"against {String.Join(", ", data.AffectedTags.Select(t => t.Name).ToList())}";
            foreach(var statIncrease in data.AffectedStats)
            {
                upgradeText += $"+{statIncrease.IncreaseAmount} {statIncrease.AffectedStat.Name} {(data.AffectedTags.Count > 0 ? affectedTags : string.Empty)}\n";
            }
            if (upgradeText.Length > 0)
            {
                upgradeText = upgradeText.Trim();
            }
            this.m_upgradeText.text = upgradeText;
            this.m_costText.text = String.Format(COST_FORMAT, data.Cost);
            
            var moneyStat = this.m_playerStats[this.m_characterStatFacade.Money];
            moneyStat.OnValueChanged += this.OnMoneyChanged;
            
            this.m_buyButton.interactable = moneyStat.CurrentValue >= data.Cost;
        }

        private void OnMoneyChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (this.m_data is null)
                return;
            
            this.m_buyButton.interactable = e.NewValue >= this.m_data.Cost;
        }

        private void OnDestroy()
        {
            if (this.m_playerStats is null)
                return;
            
            this.m_playerStats[this.m_characterStatFacade.Money].OnValueChanged -= this.OnMoneyChanged;
        }
    }
}