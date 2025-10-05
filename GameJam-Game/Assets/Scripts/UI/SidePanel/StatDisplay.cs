using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;

namespace Nidavellir.UI.SidePanel
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private CharacterStat m_statToDisplay;
        [SerializeField] private TextMeshProUGUI m_statName;
        [SerializeField] private TextMeshProUGUI m_statValue;

        private void Start()
        {
            var statController = this.m_playerStats[this.m_statToDisplay];
            statController.OnValueChanged += this.OnStatValueChanged;
            if (this.m_characterStatFacade is not null)
            {
                this.m_statName.text = this.m_statToDisplay.Name;
            }
            
            this.m_statValue.text = $"{statController.CurrentValue:D}";
        }

        private void OnStatValueChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_statValue.text = $"{e.NewValue:D}";
        }
        
        private void OnDestroy()
        {
            if (this.m_playerStats is null)
                return;
            
            this.m_playerStats[this.m_statToDisplay].OnValueChanged -= this.OnStatValueChanged;
        }
    }
}