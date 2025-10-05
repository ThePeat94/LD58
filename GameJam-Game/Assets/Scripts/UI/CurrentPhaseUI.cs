using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.GameState;
using TMPro;
using UnityEngine;

namespace Nidavellir.UI
{
    public class CurrentPhaseUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_currentPhaseText;
        [SerializeField] private TextMeshProUGUI m_description;
        
        private IEventBinding<GameStateChangedEvent> m_gameStateChangedEventBinding;

        private void Awake()
        {
            this.m_gameStateChangedEventBinding = new EventBinding<GameStateChangedEvent>(this.OnGameStateChanged);
            GameEventBus<GameStateChangedEvent>.Register(this.m_gameStateChangedEventBinding);
        }

        private void Start()
        {
            var gameStateManager = FindFirstObjectByType<GameStateManager>();
            this.UpdateTexts(gameStateManager.CurrentState);
        }

        private void OnGameStateChanged(object sender, GameStateChangedEvent e)
        {
            this.UpdateTexts(e.NewState);
        }

        private void UpdateTexts(State state)
        {
            this.m_currentPhaseText.text = state.ToString().ToUpper();
            this.m_description.text = state switch
            {
                State.Draft => "Choose your enemies wisely!",
                State.Fight => "Defeat all your enemies!",
                State.Shop => "Buy upgrades to become stronger!",
                State.Gameover => "You have been defeated!",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void OnDestroy()
        {
            GameEventBus<GameStateChangedEvent>.Unregister(this.m_gameStateChangedEventBinding);
        }
    }
}