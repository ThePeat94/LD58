using System;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Shop
{
    public class RerollButton : MonoBehaviour
    {
        private const string REROLL_COST_FORMAT = "{0}g";
        
        [SerializeField] private Button m_button;
        [SerializeField] private TextMeshProUGUI m_rerollCostText;

        private Action m_onRerollClicked;
        
        public event Action OnRerollClicked
        {
            add =>  this.m_onRerollClicked += value;
            remove =>  this.m_onRerollClicked -= value;
        }
        
        private void Awake()
        {
            this.m_button.onClick.AddListener(this.OnRerollClick);
        }

        public void ShowRerollInformation(int cost, bool canAfford)
        {
            this.m_rerollCostText.text = string.Format(REROLL_COST_FORMAT, cost);
            this.m_button.interactable = canAfford;
        }


        private void OnRerollClick()
        {
            this.m_onRerollClicked?.Invoke();
        }
    }
}