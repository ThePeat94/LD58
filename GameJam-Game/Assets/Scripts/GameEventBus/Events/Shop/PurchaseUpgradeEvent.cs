using Nidavellir.Scriptables;

namespace Nidavellir.GameEventBus.Events.Shop
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