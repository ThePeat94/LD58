using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Scriptables;
using Nidavellir.UI.Shop;
using UnityEngine;

namespace Nidavellir.Shop
{
    public class RerollManager : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private RerollButton m_rerollButton;

        private int m_initialRerollCost = 2;
        private int m_rerollCost = 2;
        

        private void Awake()
        {
            this.m_rerollButton ??= FindFirstObjectByType<RerollButton>(FindObjectsInactive.Include);
            
            this.m_rerollButton.OnRerollClicked += this.HandleRerollClick;
            this.m_playerStats[this.m_characterStatFacade.Money].OnValueChanged += this.HandlePlayerMoneyChange;
        }
        
        private void HandlePlayerMoneyChange(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_rerollButton.ShowRerollInformation(this.m_rerollCost, this.CanAfford());
        }

        private void OnDestroy()
        {
            this.m_rerollButton.OnRerollClicked -= this.HandleRerollClick;
        }

        public void ResetCost()
        {
            this.m_rerollCost = this.m_initialRerollCost;
            this.m_rerollButton.ShowRerollInformation(this.m_rerollCost, this.CanAfford());
        }

        private void HandleRerollClick()
        {
            this.m_playerStats[this.m_characterStatFacade.Money].UseResource(this.m_rerollCost);
            this.m_rerollCost++;
            this.m_rerollButton.ShowRerollInformation(this.m_rerollCost, this.CanAfford());
        }

        private bool CanAfford() => this.m_playerStats[this.m_characterStatFacade.Money].CurrentValue >= this.m_rerollCost;
    }
}