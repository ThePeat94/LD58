using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Entity;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Location;
using Nidavellir.Player;
using Nidavellir.Scriptables;
using Nidavellir.UI.Shop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nidavellir.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradeData> m_availableUpgrades;
        [SerializeField] private ShopUI m_shopUI;
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private LocationDraftManager m_locationDraftManager;
        [SerializeField] private PlayerStatsController m_playerStatsController;
        [SerializeField] private RerollButton m_rerollButton;
        
        private int m_upgradeAmount = 2;
        
        private IEventBinding<PurchaseUpgradeEvent> m_purchaseUpgradeEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;


        private void Awake()
        {
            this.m_rerollButton ??= FindFirstObjectByType<RerollButton>(FindObjectsInactive.Include);
            this.m_playerStatsController = FindFirstObjectByType<PlayerStatsController>(FindObjectsInactive.Include);
            this.m_locationDraftManager ??= FindFirstObjectByType<LocationDraftManager>(FindObjectsInactive.Include);
            
            this.m_purchaseUpgradeEventBinding = new EventBinding<PurchaseUpgradeEvent>(this.OnPurchaseUpgrade);
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            
            this.m_shopUI.OnStartLocationDraftClicked += this.HandleStartLocationDraftClick;
            this.m_rerollButton.OnRerollClicked += this.HandleRerollClick;
            GameEventBus<PurchaseUpgradeEvent>.Register(this.m_purchaseUpgradeEventBinding);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
        }


        private void OnDestroy()
        {
            this.m_shopUI.OnStartLocationDraftClicked -= this.HandleStartLocationDraftClick;
            this.m_rerollButton.OnRerollClicked -= this.HandleRerollClick;
            GameEventBus<PurchaseUpgradeEvent>.Unregister(this.m_purchaseUpgradeEventBinding);
            GameEventBus<VisitShopEvent>.Unregister(this.m_visitShopEventBinding);
        }

        private void OnVisitShop(object sender, VisitShopEvent e)
        {
            this.m_shopUI.Show(this.GetRandomUpgrades(this.m_upgradeAmount));
        }

        private void OnPurchaseUpgrade(object sender, PurchaseUpgradeEvent e)
        {
            this.m_entityStats[this.m_characterStatFacade.Money].UseResource(e.UpgradeData.Cost);
        }

        private void HandleRerollClick()
        {
            this.m_shopUI.Show(this.GetRandomUpgrades(this.m_upgradeAmount));
        }

        private void HandleStartLocationDraftClick()
        {
            GameEventBus<ShopExitedEvent>.Invoke(this, new());
            this.m_playerStatsController.ResetStatsAfterShop();
            this.m_locationDraftManager.SelectLocations();
        }

        private List<UpgradeData> GetRandomUpgrades(int count)
        {
            return this.m_availableUpgrades.OrderBy(x => Random.value)
                .Take(this.m_upgradeAmount)
                .ToList();
        }
    }
}