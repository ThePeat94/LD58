using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events;
using Nidavellir.EventBus.Events.Draft;
using Nidavellir.EventBus.Events.Fight;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.UI;
using Nidavellir.UI.Draft;
using Nidavellir.UI.GameOver;
using Nidavellir.UI.GameWon;
using Nidavellir.UI.Location;
using UnityEngine;

namespace Nidavellir.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private DraftUI m_draftUi;
        [SerializeField] private GameObject m_shopUi;
        [SerializeField] private GameObject m_fightUi;
        [SerializeField] private GameOverUI m_gameOverUI;
        [SerializeField] private GameWonUI m_gameWonUI;
        [SerializeField] private LocationSelectionUI m_locationSelectionUI;
        [SerializeField] private EnemySelectionUI m_enemySelectionUI;
        
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        private IEventBinding<VisitShopEvent> m_visitShopEventBinding;
        private IEventBinding<BountyRequirementNotFulfilled> m_bountyRequirementNotFulfilledEventBinding;
        private IEventBinding<PlayerDiedEvent> m_playerDiedEventBinding;
        private IEventBinding<GameWonEvent> m_gameWonEventBinding;
        private IEventBinding<StartLocationDraftEvent> m_startLocationEventBinding;
        private IEventBinding<EnemyLocationSelectedEvent> m_enemyLocationSelectedEventBinding;
        private IEventBinding<EventLocationSelectedEvent> m_eventLocationSelectedEventBinding;
        
        private State m_currentState;

        public State CurrentState => this.m_currentState;

        private void Awake()
        {
            this.m_currentState = State.LocationDraft;
            this.m_draftUi ??= FindFirstObjectByType<DraftUI>(FindObjectsInactive.Include);
            this.m_gameOverUI ??= FindFirstObjectByType<GameOverUI>(FindObjectsInactive.Include);
            this.m_gameWonUI ??= FindFirstObjectByType<GameWonUI>(FindObjectsInactive.Include);
            this.m_locationSelectionUI ??= FindFirstObjectByType<LocationSelectionUI>(FindObjectsInactive.Include);
            this.m_enemySelectionUI ??= FindFirstObjectByType<EnemySelectionUI>(FindObjectsInactive.Include);

            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFight);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
            
            this.m_visitShopEventBinding = new EventBinding<VisitShopEvent>(this.OnVisitShop);
            GameEventBus<VisitShopEvent>.Register(this.m_visitShopEventBinding);
            
            this.m_bountyRequirementNotFulfilledEventBinding = new EventBinding<BountyRequirementNotFulfilled>(this.OnBountyRequirementNotFulfilled);
            GameEventBus<BountyRequirementNotFulfilled>.Register(this.m_bountyRequirementNotFulfilledEventBinding);
            
            this.m_playerDiedEventBinding = new EventBinding<PlayerDiedEvent>(this.OnPlayerDied);
            GameEventBus<PlayerDiedEvent>.Register(this.m_playerDiedEventBinding);
            
            this.m_gameWonEventBinding = new EventBinding<GameWonEvent>(this.OnGameWon);
            GameEventBus<GameWonEvent>.Register(this.m_gameWonEventBinding);
            
            this.m_enemyLocationSelectedEventBinding = new EventBinding<EnemyLocationSelectedEvent>(this.OnEnemyLocationSelected);
            GameEventBus<EnemyLocationSelectedEvent>.Register(this.m_enemyLocationSelectedEventBinding);
            
            this.m_eventLocationSelectedEventBinding = new EventBinding<EventLocationSelectedEvent>(this.OnEventLocationSelected);
            GameEventBus<EventLocationSelectedEvent>.Register(this.m_eventLocationSelectedEventBinding);
            
            this.m_startLocationEventBinding = new EventBinding<StartLocationDraftEvent>(this.OnStartLocationDraft);
            GameEventBus<StartLocationDraftEvent>.Register(this.m_startLocationEventBinding);
        }

        private void OnGameWon(object sender, GameWonEvent e)
        {
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_gameWonUI.Show();
        }

        private void Start()
        {
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.CurrentState));
            
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_enemySelectionUI?.gameObject.SetActive(false);
            this.m_locationSelectionUI?.gameObject.SetActive(true);
            this.m_gameWonUI.Hide();
            this.m_gameOverUI.Hide();
        }

        private void OnDestroy()
        {
            GameEventBus<StartFightEvent>.Unregister(this.m_startFightEventBinding);
            GameEventBus<VisitShopEvent>.Unregister(this.m_visitShopEventBinding);
            GameEventBus<BountyRequirementNotFulfilled>.Unregister(this.m_bountyRequirementNotFulfilledEventBinding);
            GameEventBus<PlayerDiedEvent>.Unregister(this.m_playerDiedEventBinding);
            GameEventBus<GameWonEvent>.Unregister(this.m_gameWonEventBinding);
            GameEventBus<EnemyLocationSelectedEvent>.Unregister(this.m_enemyLocationSelectedEventBinding);
            GameEventBus<EventLocationSelectedEvent>.Unregister(this.m_eventLocationSelectedEventBinding);
            GameEventBus<StartLocationDraftEvent>.Unregister(this.m_startLocationEventBinding);
        }

        private void OnStartFight(object sender, StartFightEvent evt)
        {
            this.m_currentState = State.Fight;
            this.m_enemySelectionUI?.gameObject.SetActive(false);
            this.m_fightUi?.SetActive(true);
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }

        private void OnVisitShop(object sender, VisitShopEvent evt)
        {
            this.m_currentState = State.Shop;
            this.m_draftUi?.gameObject.SetActive(false);
            this.m_fightUi?.SetActive(false);
            this.m_locationSelectionUI?.gameObject.SetActive(false);
            this.m_shopUi?.SetActive(true);
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

        private void OnEnemyLocationSelected(object sender, EnemyLocationSelectedEvent e)
        {
            this.m_currentState = State.EnemyDraft;
            this.m_locationSelectionUI.gameObject.SetActive(false);
            this.m_enemySelectionUI.gameObject.SetActive(true);
            
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
        }

        private void OnEventLocationSelected(object sender, EventLocationSelectedEvent e)
        {
            // No implementation needed for now
        }

        private void OnStartLocationDraft(object sender, StartLocationDraftEvent e)
        {
            this.m_currentState = State.LocationDraft;
            this.m_enemySelectionUI.gameObject.SetActive(false);
            this.m_shopUi.gameObject.SetActive(false);
            this.m_locationSelectionUI.gameObject.SetActive(true);
            
            GameEventBus<GameStateChangedEvent>.Invoke(this, new GameStateChangedEvent(this.m_currentState));
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