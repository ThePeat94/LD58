using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.Scriptables;
using Nidavellir.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Shop
{
    public class RerollButton : MonoBehaviour
    {
        private const string REROLL_COST_FORMAT = "Reroll ({0}€)";
        
        [SerializeField] private Button m_button;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private RerollManager m_rerollManager;
        [SerializeField] private TextMeshProUGUI m_rerollCostText;
        
        private void Awake()
        {
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
            this.m_button.onClick.AddListener(this.OnRerollClick);
            this.m_rerollManager ??= FindFirstObjectByType<RerollManager>();
        }

        private void Start()
        {
            this.m_playerStats[this.m_characterStatFacade.Money].OnValueChanged += this.OnMoneyChanged;
            this.m_rerollCostText.text = String.Format(REROLL_COST_FORMAT, this.m_rerollManager.RerollCost);
            this.m_button.interactable = this.m_playerStats[this.m_characterStatFacade.Money].CurrentValue >= this.m_rerollManager.RerollCost;
        }

        private void OnMoneyChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_button.interactable = e.NewValue >= this.m_rerollManager.RerollCost;
        }

        private void OnRerollClick()
        {
            GameEventBus<RerollUpgradesEvent>.Invoke(this, new RerollUpgradesEvent());
            this.m_rerollCostText.text = String.Format(REROLL_COST_FORMAT, this.m_rerollManager.RerollCost);
            this.m_button.interactable = this.m_playerStats[this.m_characterStatFacade.Money].CurrentValue >= this.m_rerollManager.RerollCost;
        }
    }
}