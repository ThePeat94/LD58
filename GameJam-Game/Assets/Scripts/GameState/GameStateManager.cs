using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.UI.Draft;
using UnityEngine;

namespace Nidavellir.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_draftUi;
        [SerializeField] private GameObject m_shopUi;
        
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        
        private State m_currentState;
        
        public State CurrentState => this.m_currentState;

        private void Awake()
        {
            this.m_draftUi ??= FindFirstObjectByType<DraftUI>()?.gameObject;

            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFight);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
        }

        private void Start()
        {
            this.m_currentState = State.Draft;
            this.m_draftUi?.SetActive(true);
            this.m_shopUi?.SetActive(false);
        }
        
        private void OnDestroy()
        {
            GameEventBus<StartFightEvent>.Unregister(this.m_startFightEventBinding);
        }
        
        private void OnStartFight(object sender, StartFightEvent evt)
        {
            this.m_currentState = State.Fight;
            this.m_draftUi?.SetActive(false);
            this.m_shopUi?.SetActive(false);
        }
    }
}