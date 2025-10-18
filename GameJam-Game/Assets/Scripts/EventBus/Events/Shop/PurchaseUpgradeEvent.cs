using Nidavellir.Scriptables;

namespace Nidavellir.EventBus.Events.Shop
{
    public class PurchaseUpgradeEvent : IEvent
    {
        public PurchaseUpgradeEvent(UpgradeData upgradeData)
        {
            this.UpgradeData = upgradeData;
        }
        
        public UpgradeData UpgradeData { get; }
    }
}