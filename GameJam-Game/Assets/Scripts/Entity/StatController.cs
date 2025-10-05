using System;
using Nidavellir.EventArgs;
using Nidavellir.Scriptables;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Entity
{
    public class StatController
    {
        private EventHandler<CharacterStatValueChangeEventArgs> m_resourceValueChanged;
        private EventHandler<CharacterStatValueChangeEventArgs> m_maximumValueChanged;

        public int CurrentValue { get; private set; }
        public int MaxValue { get; private set; }

        public StatController(InitialStatValue initialStatValue)
        {
            this.CurrentValue = initialStatValue.Value;
            this.MaxValue = initialStatValue.MaxValue;
        }
        
        public StatController(int initialValue)
        {
            this.CurrentValue = initialValue;
            this.MaxValue = initialValue;
        }
        
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentException($"{value} is less than 0");

            var oldValue = this.CurrentValue;
            this.CurrentValue += value;
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, oldValue));
        }

        public bool CanAfford(int amount)
        {
            if (amount < 0)
                throw new ArgumentException($"{amount} is less than 0");

            return amount <= this.CurrentValue;
        }

        public void UseResource(int amount)
        {
            if (amount < 0)
                throw new ArgumentException($"{amount} is less than 0");

            var clampedAmount = Math.Clamp(amount, 0, this.CurrentValue);
            var oldValue = this.CurrentValue;
            this.CurrentValue -= clampedAmount;
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, oldValue));
        }

        public void SetCurrentValue(int newValue)
        {
            var old = this.CurrentValue;
            var clampedValue = Math.Clamp(newValue, 0, this.MaxValue);
            this.CurrentValue = clampedValue;
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, old));
        }

        public void ApplyDeltaToMaximumValue(int amount)
        {
            var oldMax = this.MaxValue;
            var oldCurrent = this.CurrentValue;
            this.MaxValue += amount;
            this.MaxValue = Math.Max(0, this.MaxValue);
            this.CurrentValue = Mathf.Min(this.CurrentValue, this.MaxValue);
            this.m_maximumValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.MaxValue, oldMax));
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, oldCurrent));
        }

        public void ApplyStatIncrease(int amount)
        {
            var oldMax = this.MaxValue;
            var oldCurrent = this.CurrentValue;
            this.MaxValue += amount;
            this.MaxValue = this.MaxValue = Math.Max(0, this.MaxValue);
            this.CurrentValue = Mathf.Min(this.CurrentValue + amount, this.MaxValue);
            this.m_maximumValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.MaxValue, oldMax));
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, oldCurrent));
        }

        public void ResetToMax()
        {
            var oldValue = this.CurrentValue;
            this.CurrentValue = this.MaxValue;
            this.m_resourceValueChanged?.Invoke(this, new CharacterStatValueChangeEventArgs(this.CurrentValue, oldValue));
        }

        public event EventHandler<CharacterStatValueChangeEventArgs> OnValueChanged
        {
            add => this.m_resourceValueChanged += value;
            remove => this.m_resourceValueChanged -= value;
        }    
    
        public event EventHandler<CharacterStatValueChangeEventArgs> OnMaxValueChanged
        {
            add => this.m_maximumValueChanged += value;
            remove => this.m_maximumValueChanged -= value;
        }
    }
}