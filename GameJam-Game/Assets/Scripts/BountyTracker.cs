using System;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Draft;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.Scriptables;
using Nidavellir.UI.EnemyDraft;
using UnityEngine;

namespace Nidavellir
{
    public class BountyTracker : MonoBehaviour
    {
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;
        [SerializeField] private BountyTrackerUI m_bountyTrackerUI;
        
        private int m_collectedBounty;
        private int m_potentialBounty;
        
        private IEventBinding<EnemyDefeatedEvent> m_enemyDefeatedBinding;
        private IEventBinding<ProfileLikedEvent> m_profileLikedBinding;
        private IEventBinding<ShopEnteredEvent> m_shopEnteredBinding;


        private void Awake()
        {
            this.m_bountyTrackerUI ??= FindFirstObjectByType<BountyTrackerUI>(FindObjectsInactive.Include);
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include);
            this.m_enemyDefeatedBinding = new EventBinding<EnemyDefeatedEvent>(this.OnEnemyDefeatedEvent);
            this.m_profileLikedBinding = new EventBinding<ProfileLikedEvent>(this.OnProfileLiked);
            this.m_shopEnteredBinding = new EventBinding<ShopEnteredEvent>(this.OnShopEnteredEvent);

            GameEventBus<EnemyDefeatedEvent>.Register(this.m_enemyDefeatedBinding);
            GameEventBus<ProfileLikedEvent>.Register(this.m_profileLikedBinding);
            GameEventBus<ShopEnteredEvent>.Register(this.m_shopEnteredBinding);
        }

        private void Start()
        {
            this.m_bountyTrackerUI.Show(this.m_potentialBounty, this.m_collectedBounty, this.m_bountyRequirementController.CurrentBountyRequirement);
        }

        private void OnDestroy()
        {
            GameEventBus<EnemyDefeatedEvent>.Unregister(this.m_enemyDefeatedBinding);
            GameEventBus<ProfileLikedEvent>.Unregister(this.m_profileLikedBinding);
            GameEventBus<ShopEnteredEvent>.Unregister(this.m_shopEnteredBinding);
        }
        private void OnProfileLiked(object sender, ProfileLikedEvent e)
        {
            this.m_potentialBounty += e.EnemyData.Stats[this.m_characterStatFacade.Money];
            this.m_bountyTrackerUI.Show(this.m_potentialBounty, this.m_collectedBounty, this.m_bountyRequirementController.CurrentBountyRequirement);
        }

        private void OnShopEnteredEvent(object sender, ShopEnteredEvent e)
        {
            this.m_potentialBounty = 0;
            this.m_collectedBounty = 0;
            this.m_bountyTrackerUI.Show(this.m_potentialBounty, this.m_collectedBounty, this.m_bountyRequirementController.CurrentBountyRequirement);
        }

        private void OnEnemyDefeatedEvent(object sender, EnemyDefeatedEvent e)
        {
            var bounty = e.DefeatedEnemy.EntityStats[this.m_characterStatFacade.Money].CurrentValue;
            this.m_potentialBounty -= bounty;
            this.m_collectedBounty += bounty;
            this.m_bountyTrackerUI.Show(this.m_potentialBounty, this.m_collectedBounty, this.m_bountyRequirementController.CurrentBountyRequirement);
        }
    }
}