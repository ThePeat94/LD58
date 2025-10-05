using System;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Player
{
    public class PlayerRizzController : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private void Awake()
        {
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
        }

        private void Start()
        {
            this.m_playerStats[this.m_characterStatFacade.Rizz].OnValueChanged += this.OnRizzChanged;
        }

        private void OnRizzChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (e.NewValue <= 4)
                return;
            
            GameEventBus<GameWonEvent>.Invoke(this, new());
        }
    }
}