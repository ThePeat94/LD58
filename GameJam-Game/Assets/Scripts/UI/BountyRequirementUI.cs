using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI
{
    public class BountyRequirementUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_currentAccumulatedBounty;
        [SerializeField] private TextMeshProUGUI m_bountyRequirement;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;
        [SerializeField] private Image m_fulfillmentIndicator;

        private void Awake()
        {
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            this.m_playerStats[this.m_characterStatFacade.Round].OnValueChanged += this.OnRoundChange;
            this.m_playerStats[this.m_characterStatFacade.Bounty].OnValueChanged += this.OnBountyChange;
            this.m_bountyRequirement.text = this.m_bountyRequirementController.CurrentBountyRequirement.ToString();
        }

        private void OnDestroy()
        {
            if (this.m_playerStats is null)
                return;
            
            this.m_playerStats[this.m_characterStatFacade.Bounty].OnValueChanged -= this.OnBountyChange;
            this.m_playerStats[this.m_characterStatFacade.Round].OnValueChanged -= this.OnRoundChange;
        }

        private void OnBountyChange(object sender, CharacterStatValueChangeEventArgs e)
        {
            var color = e.NewValue < this.m_bountyRequirementController.CurrentBountyRequirement ? Color.indianRed : Color.forestGreen;
            this.m_currentAccumulatedBounty.color = color;
            this.m_fulfillmentIndicator.color = color;
        }
        
        
        private void OnRoundChange(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_bountyRequirement.text = this.m_bountyRequirementController.CurrentBountyRequirement.ToString();
        }
    }
}