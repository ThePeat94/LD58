using System.Collections.Generic;
using System.Linq;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.Scriptables;
using Nidavellir.UI.Shop;
using UnityEngine;

namespace Nidavellir.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradeData> m_availableUpgrades;
        [SerializeField] private ShopUI m_shopUI;
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        
        private int m_upgradeAmount = 2;
        
        private IEventBinding<RerollUpgradesEvent> m_rerollUpgradesEventBinding;
        private IEventBinding<PurchaseUpgradeEvent> m_purchaseUpgradeEventBinding;
        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        
        private void Start()
        {
            this.m_rerollUpgradesEventBinding = new EventBinding<RerollUpgradesEvent>(this.OnRerollUpgrades);
            GameEventBus<RerollUpgradesEvent>.Register(this.m_rerollUpgradesEventBinding);
            
            this.m_purchaseUpgradeEventBinding = new EventBinding<PurchaseUpgradeEvent>(this.OnPurchaseUpgrade);
            GameEventBus<PurchaseUpgradeEvent>.Register(this.m_purchaseUpgradeEventBinding);
            
            this.m_startDraftEventBinding = new EventBinding<StartDraftEvent>(this.OnStartDraft);
            GameEventBus<StartDraftEvent>.Register(this.m_startDraftEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
        }

        private void OnVisitShop(object sender, VisitShopEvent e)
        {
            this.m_shopUI.Show(this.GetRandomUpgrades(this.m_upgradeAmount));
        }

        private void OnPurchaseUpgrade(object sender, PurchaseUpgradeEvent e)
        {
            this.m_entityStats[this.m_characterStatFacade.Money].UseResource(e.UpgradeData.Cost);
        }

        private void OnRerollUpgrades(object sender, RerollUpgradesEvent e)
        {
            this.m_shopUI.Show(this.GetRandomUpgrades(this.m_upgradeAmount));
        }
        
        private void OnStartDraft(object sender, StartDraftEvent e)
        {
        }
        
        private void OnDestroy()
        {
            GameEventBus<RerollUpgradesEvent>.Unregister(this.m_rerollUpgradesEventBinding);
            GameEventBus<PurchaseUpgradeEvent>.Unregister(this.m_purchaseUpgradeEventBinding);
            GameEventBus<StartDraftEvent>.Unregister(this.m_startDraftEventBinding);
            GameEventBus<VisitShopEvent>.Unregister(this.m_visitShopEventBinding);
        }
        
        private List<UpgradeData> GetRandomUpgrades(int count)
        {
            return this.m_availableUpgrades.OrderBy(x => Random.value)
                .Take(this.m_upgradeAmount)
                .ToList();
        }
    }
}