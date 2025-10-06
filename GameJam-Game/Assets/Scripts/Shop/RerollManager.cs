using System;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Shop
{
    public class RerollManager : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private EventHandler m_rerollCostsChanged;
        
        private IEventBinding<RerollUpgradesEvent> m_rerollUpgradesEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        
        private int m_initialRerollCost = 2;
        private int m_rerollCost = 2;
        
        public int RerollCost => this.m_rerollCost;
        
        public event EventHandler RerollCostsChanged
        {
            add => this.m_rerollCostsChanged += value;
            remove => this.m_rerollCostsChanged -= value;
        }

        private void Awake()
        {
            this.m_rerollUpgradesEventBinding = new EventBinding<RerollUpgradesEvent>(this.OnRerollUpgrades);
            GameEventBus<RerollUpgradesEvent>.Register(this.m_rerollUpgradesEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
        }

        private void OnDestroy()
        {
            GameEventBus<RerollUpgradesEvent>.Unregister(this.m_rerollUpgradesEventBinding);
            GameEventBus<VisitShopEvent>.Unregister(this.m_visitShopEventBinding);
        }

        private void OnVisitShop(object sender, VisitShopEvent e)
        {
            this.m_rerollCost = this.m_initialRerollCost;
            this.m_rerollCostsChanged?.Invoke(this, System.EventArgs.Empty);
        }

        private void OnRerollUpgrades(object sender, RerollUpgradesEvent e)
        {
            this.m_playerStats[this.m_characterStatFacade.Money].UseResource(this.m_rerollCost);
            this.m_rerollCost++;
            this.m_rerollCostsChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }
}