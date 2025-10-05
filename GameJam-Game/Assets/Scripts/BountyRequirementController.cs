using System;
using System.Collections.Generic;
using Nidavellir.Draft;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class BountyRequirementController : MonoBehaviour
    {
        [SerializeField] private List<int> m_bountyRequirementsPerRound;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private DraftManager m_draftManager;
        
        private IEventBinding<BountyRequirementFulfilled> m_bountyRequirementFulfilledEventBinding;
        
        public List<int> BountyRequirementsPerRound => this.m_bountyRequirementsPerRound;
        public int CurrentBountyRequirement => this.m_bountyRequirementsPerRound[Mathf.Min(this.m_playerStats[this.m_characterStatFacade.Round]?.CurrentValue - 1 ?? 0, this.m_bountyRequirementsPerRound.Count - 1)];

        private void Awake()
        {
            this.m_draftManager ??= FindFirstObjectByType<DraftManager>();
            this.m_bountyRequirementFulfilledEventBinding = new EventBinding<BountyRequirementFulfilled>(this.OnBountyRequirementFulfilled);
            GameEventBus<BountyRequirementFulfilled>.Register(this.m_bountyRequirementFulfilledEventBinding);
        }
        
        private void OnDestroy()
        {
            GameEventBus<BountyRequirementFulfilled>.Unregister(this.m_bountyRequirementFulfilledEventBinding);
        }

        private void OnBountyRequirementFulfilled(object sender, BountyRequirementFulfilled e)
        {
            var bountyStat = this.m_playerStats[this.m_characterStatFacade.Bounty];
            var excess = bountyStat.CurrentValue - this.CurrentBountyRequirement;
            bountyStat.SetCurrentValue(0);
            this.m_playerStats[this.m_characterStatFacade.Money]
                .Add(excess);
        }

        public bool HasFulfilledBountyRequirement()
        {
            return (this.m_playerStats[this.m_characterStatFacade.Bounty]?.CurrentValue ?? 0) >= this.CurrentBountyRequirement;
        }
    }
}