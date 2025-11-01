using System;
using Nidavellir.Entity;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.EventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private void OnExitShop(object sender, ShopExitedEvent e)
        {
            this.m_entityStats[this.m_characterStatFacade.Hp].ResetToMax();
        }


        public void ResetStatsAfterShop()
        {
            this.m_entityStats[this.m_characterStatFacade.Hp].ResetToMax();
        }

        public void ResetStatsBeforeLocations()
        {
            this.m_entityStats[this.m_characterStatFacade.Likes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.Dislikes].ResetToMax();
            this.m_entityStats[this.m_characterStatFacade.SuperLike].ResetToMax();
        }
    }
}