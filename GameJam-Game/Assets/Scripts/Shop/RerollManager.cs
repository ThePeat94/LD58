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
        
        private IEventBinding<RerollUpgradesEvent> m_rerollUpgradesEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        
        private int m_initialRerollCost = 2;
        private int m_rerollCost = 2;
        
        public int RerollCost => this.m_rerollCost;

        private void Awake()
        {
            this.m_rerollUpgradesEventBinding = new EventBinding<RerollUpgradesEvent>(this.OnRerollUpgrades);
            GameEventBus<RerollUpgradesEvent>.Register(this.m_rerollUpgradesEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
        }

        private void OnVisitShop(object sender, VisitShopEvent e)
        {
            this.m_rerollCost = this.m_initialRerollCost;
        }

        private void OnRerollUpgrades(object sender, RerollUpgradesEvent e)
        {
            this.m_playerStats[this.m_characterStatFacade.Money].UseResource(this.m_rerollCost);
            this.m_rerollCost++;
        }
    }
}