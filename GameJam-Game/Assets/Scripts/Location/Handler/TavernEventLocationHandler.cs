using Nidavellir.EventBus;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.GameState;
using Nidavellir.Shop;

namespace Nidavellir.Location.Handler
{
    public class TavernEventLocationHandler : IEventLocationHandler
    {
        private readonly ShopManager m_shopManager;
        private readonly RerollManager m_rerollManager;
        private readonly GameStateManager m_gameStateManager;
        private readonly BountyRequirementController m_bountyRequirementController;

        public TavernEventLocationHandler(ShopManager shopManager, RerollManager rerollManager,  GameStateManager gameStateManager, BountyRequirementController bountyRequirementController)
        {
            this.m_shopManager = shopManager;
            this.m_rerollManager = rerollManager;
            this.m_gameStateManager = gameStateManager;
            this.m_bountyRequirementController = bountyRequirementController;
        }

        public void Execute()
        {
            this.m_bountyRequirementController.FulfillBounty();
            this.m_gameStateManager.VisitShop();
            this.m_shopManager.VisitShop();
            this.m_rerollManager.ResetCost();
            GameEventBus<ShopEnteredEvent>.Invoke(this, new());
        }
    }
}