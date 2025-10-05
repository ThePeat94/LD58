using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.UI.Draft;
using Nidavellir.UI.GameOver;
using UnityEngine;

namespace Nidavellir.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private DraftUI m_draftUi;
        [SerializeField] private GameObject m_shopUi;
        [SerializeField] private GameObject m_fightUi;
        [SerializeField] private GameOverUI m_gameOverUI;
        
        
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;
        private IEventBinding<BountyRequirementNotFulfilled> m_bountyRequirementNotFulfilledEvent;
        private IEventBinding<PlayerDiedEvent> m_playerDiedEvent;
        
        private State m_currentState;
        
        public State CurrentState => this.m_currentState;

        private void Awake()
        {
            this.m_draftUi ??= FindFirstObjectByType<DraftUI>(FindObjectsInactive.Include);
            this.m_gameOverUI ??= FindFirstObjectByType<GameOverUI>(FindObjectsInactive.Include);

            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFight);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
            
            this.m_startDraftEventBinding = new EventBinding<StartDraftEvent>(this.OnStartDraft);
            GameEventBus<StartDraftEvent>.Register(this.m_startDraftEventBinding);
            
            this.m_bountyRequirementNotFulfilledEvent = new EventBinding<BountyRequirementNotFulfilled>(this.OnBountyRequirementNotFulfilled);
            GameEventBus<BountyRequirementNotFulfilled>.Register(this.m_bountyRequirementNotFulfilledEvent);
            
            this.m_playerDiedEvent = new EventBinding<PlayerDiedEvent>(this.OnPlayerDied);
            GameEventBus<PlayerDiedEvent>.Register(this.m_playerDiedEvent);
            
            this.m_currentState = State.Draft;
            this.m_draftUi?.gameObject.SetActive(true);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_draftUi?.ShowProfiles();
            this.m_gameOverUI.Hide();
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

        private void OnBountyRequirementNotFulfilled(object sender, BountyRequirementNotFulfilled e)
        {
            this.TriggerGameOver(GameOverReason.BountyNotFulFilled);
        }

        private void OnPlayerDied(object sender, PlayerDiedEvent e)
        {
            this.TriggerGameOver(GameOverReason.Died);
        }

        private void TriggerGameOver(GameOverReason reason)
        {
            this.m_currentState = State.Gameover;
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_gameOverUI?.ShowGameOverPanel(reason);
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }
    }
}