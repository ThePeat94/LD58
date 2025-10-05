using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Shop;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerUpgradeController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        
        private IEventBinding<PurchaseUpgradeEvent> m_purchaseUpgradeEventBinding;
        
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
            foreach (var upgradeDataAffectedStat in e.UpgradeData.AffectedStats)
            {
                var stat = this.m_playerStats[upgradeDataAffectedStat.AffectedStat];
                stat.ApplyStatIncrease(upgradeDataAffectedStat.IncreaseAmount);
            }
        }
    }
}