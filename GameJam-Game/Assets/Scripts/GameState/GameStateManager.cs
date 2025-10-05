using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.UI.Draft;
using UnityEngine;

namespace Nidavellir.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private DraftUI m_draftUi;
        [SerializeField] private GameObject m_shopUi;
        [SerializeField] private GameObject m_fightUi;
        
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;
        
        private State m_currentState;
        
        public State CurrentState => this.m_currentState;

        private void Awake()
        {
            this.m_draftUi ??= FindFirstObjectByType<DraftUI>(FindObjectsInactive.Include);

            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFight);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
            
            this.m_startDraftEventBinding = new EventBinding<StartDraftEvent>(this.OnStartDraft);
            GameEventBus<StartDraftEvent>.Register(this.m_startDraftEventBinding);
            
            this.m_currentState = State.Draft;
            this.m_draftUi?.gameObject.SetActive(true);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_draftUi?.ShowProfiles();
        }

        private void Start()
        {
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.CurrentState));
        }
        
        private void OnDestroy()
        {
            GameEventBus<StartFightEvent>.Unregister(this.m_startFightEventBinding);
        }
        
        private void OnStartFight(object sender, StartFightEvent evt)
        {
            this.m_currentState = State.Fight;
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(true);
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }
        
        private void OnVisitShop(object sender, VisitShopEvent evt)
        {
            this.m_currentState = State.Shop;
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(true);
            this.m_fightUi?.SetActive(false);
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }
        
        private void OnStartDraft(object sender, StartDraftEvent evt)
        {
            this.m_currentState = State.Draft;
            this.m_draftUi?.gameObject.SetActive(true);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_draftUi?.ShowProfiles();
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }
    }
}