using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_entityStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private void Awake()
        {
            this.m_entityStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            this.m_entityStats[this.m_characterStatFacade.Hp].OnValueChanged += this.OnHpChanged;
        }

        private void OnHpChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (e.NewValue > 0)
                return;

            GameEventBus<PlayerDiedEvent>.Invoke(this, new());
        }
    }
}