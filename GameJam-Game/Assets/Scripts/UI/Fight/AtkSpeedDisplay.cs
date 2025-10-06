using System;
using Nidavellir.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Fight
{
    public class AtkSpeedDisplay : MonoBehaviour
    {
        [SerializeField] private Slider m_slider;
        
        private EntityAttacker m_attacker;

        private void Update()
        {
            if (this.m_attacker is null || !this.m_attacker.CanAttack)
                return;
            
            this.m_slider.value = this.m_attacker.CurrentAttackFrame;
        }

        public void Init(EntityAttacker attacker)
        {
            this.m_slider.maxValue = attacker.AttackFrames;
            this.m_slider.value = attacker.CurrentAttackFrame;
            this.m_attacker = attacker;
        }
    }
}