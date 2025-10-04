using Nidavellir.Entity;
using Nidavellir.EventArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Fight
{
    public class HpBarDisplay : MonoBehaviour
    {
        [SerializeField] private Slider m_slider;
        [SerializeField] private TextMeshProUGUI m_hpTexxter;
        
        private StatController m_statController;
        
        public void Init(StatController hpController)
        {
            this.m_statController = hpController;
            this.m_slider.maxValue = hpController.MaxValue;
            this.m_slider.value = hpController.CurrentValue;
            this.m_statController.OnValueChanged += this.OnHpChanged;
            this.m_statController.OnMaxValueChanged += this.OnMaxHpChanged;
            this.m_hpTexxter.text = $"{this.m_statController.CurrentValue:D}/{this.m_statController.MaxValue:D}";
        }
        
        private void OnDestroy()
        {
            if (this.m_statController == null)
                return;
            
            this.m_statController.OnValueChanged -= this.OnHpChanged;
            this.m_statController.OnMaxValueChanged -= this.OnMaxHpChanged;
        }
        
        private void OnHpChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_slider.value = e.NewValue;
            this.m_hpTexxter.text = $"{this.m_statController.CurrentValue:D}/{this.m_statController.MaxValue:D}";
        }

        private void OnMaxHpChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_slider.maxValue = e.NewValue;
        }
    }
}