using System;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Draft;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;

namespace Nidavellir.UI.EnemyDraft
{
    public class BountyTrackerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_potentialBountyText;
        [SerializeField] private TextMeshProUGUI m_bountyRequirementText;

        public void Show(int potentialBounty, int collectedBounty, int bountyRequirement)
        {
            this.m_potentialBountyText.text = potentialBounty.ToString();
            this.m_bountyRequirementText.text = $"{collectedBounty}/{bountyRequirement}";
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}