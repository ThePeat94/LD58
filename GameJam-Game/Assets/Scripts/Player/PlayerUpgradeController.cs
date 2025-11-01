using System.Collections.Generic;
using Nidavellir.Entity;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerUpgradeController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        
        private IEventBinding<PurchaseUpgradeEvent> m_purchaseUpgradeEventBinding;
        
        private List<UpgradeData> m_purchasedUpgrades = new List<UpgradeData>();
        
        public IReadOnlyList<UpgradeData> PurchasedUpgrades => this.m_purchasedUpgrades.AsReadOnly();
        
        private void Awake()
        {
            this.m_purchaseUpgradeEventBinding = new EventBinding<PurchaseUpgradeEvent>(this.OnPurchaseUpgrade);
            GameEventBus<PurchaseUpgradeEvent>.Register(this.m_purchaseUpgradeEventBinding);
        }
        
        private void OnDestroy()
        {
            GameEventBus<PurchaseUpgradeEvent>.Unregister(this.m_purchaseUpgradeEventBinding);
        }

        private void OnPurchaseUpgrade(object sender, PurchaseUpgradeEvent e)
        {
            this.m_playerStats.ApplyStatIncreases(e.UpgradeData.AffectedStats);
            this.m_purchasedUpgrades.Add(e.UpgradeData);
        }
    }
}