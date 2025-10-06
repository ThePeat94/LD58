using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.GameEventBus.Events.Fight;
using Nidavellir.Player;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using Nidavellir.UI.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nidavellir.Fight
{
    public class FightManager : MonoBehaviour
    {
        [SerializeField] private EntityAttacker m_playerAttacker;
        [SerializeField] private EntityInformation m_playerInformation;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private FightUI m_fightUI;
        [SerializeField] private BountyRequirementController m_bountyRequirementController;
        
        private List<RuntimeEnemyInformation> m_likedEnemies;
        private List<RuntimeEnemyInformation> m_defeatedEnemies = new();
        
        private Queue<RuntimeEnemyInformation> m_enemyQueue = new();

        private EntityInformation m_currentEnemyInformation;
        private RuntimeEnemyInformation m_currentEnemyData;
        private EntityAttacker m_currentEnemyAttacker;
        
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        
        private void Awake()
        {
            this.m_fightUI ??= FindFirstObjectByType<FightUI>();
            this.m_bountyRequirementController ??= FindFirstObjectByType<BountyRequirementController>(FindObjectsInactive.Include);

            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFight);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
        }

        private void Start()
        {
            this.m_fightUI.InitPlayerCard(this.m_playerInformation, this.m_playerAttacker);
        }

        private void OnDestroy()
        {
            GameEventBus<StartFightEvent>.Unregister(this.m_startFightEventBinding);
        }

        private void OnStartFight(object sender, StartFightEvent e)
        {
            this.StartFight(e.LikedProfiles);
            this.m_fightUI.ShowFightUI();
        }
        
        private void StartFight(List<RuntimeEnemyInformation> likedEnemies)
        {
            this.m_likedEnemies = likedEnemies;
            var shuffledList = this.m_likedEnemies.OrderBy(x => x.Stats[this.m_characterStatFacade.Distance]).ToList();
            foreach (var enemy in shuffledList)
            {
                this.m_enemyQueue.Enqueue(enemy);
            }
            
            var firstEnemy = this.m_enemyQueue.Dequeue();
            this.CreateEnemy(firstEnemy);
            this.m_playerAttacker.CanAttack = true;
            this.m_currentEnemyAttacker.CanAttack = true;
        }

        private void CreateEnemy(RuntimeEnemyInformation enemyData)
        {
            this.ResetEnemy();
            this.m_currentEnemyData = enemyData;
            var currentEnemyGameObject = new GameObject();
            var entityStats = currentEnemyGameObject.AddComponent<EntityStats>();
            entityStats.Init(enemyData);
            var entityInformation = currentEnemyGameObject.AddComponent<EntityInformation>();
            entityInformation.Init(enemyData, entityStats);
            this.m_currentEnemyInformation = entityInformation;
            this.RegisterToEnemyHealth(entityInformation);

            this.m_currentEnemyAttacker = currentEnemyGameObject.AddComponent<EntityAttacker>();
            this.m_currentEnemyAttacker.Initialize(entityInformation, this.m_playerInformation, this.m_characterStatFacade, EntityAttacker.EntityMode.Enemy);
            this.m_fightUI.ShowEnemy(entityInformation, this.m_currentEnemyAttacker);
            this.m_playerAttacker.SetTarget(this.m_currentEnemyInformation);
        }

        private void RegisterToEnemyHealth(EntityInformation entityInformation)
        {
            var hpStat = entityInformation.EntityStats[this.m_characterStatFacade.Hp];
            hpStat.OnValueChanged += this.OnEnemyHpChanged;
        }
        
        private void OnEnemyHpChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            if (e.NewValue > 0)
                return;

            GameEventBus<EnemyDefeatedEvent>.Invoke(this, new EnemyDefeatedEvent(this.m_currentEnemyInformation));
            this.m_defeatedEnemies.Add(this.m_currentEnemyData);
            this.m_playerAttacker.CanAttack = false;
            this.m_playerAttacker.SetTarget(null);
            this.m_currentEnemyInformation.EntityStats[this.m_characterStatFacade.Hp].OnValueChanged -= this.OnEnemyHpChanged;
            this.m_defeatedEnemies.Add(this.m_currentEnemyData);
            this.EvaluateEnemies();
        }
        

        private void EvaluateEnemies()
        {
            if (this.m_enemyQueue.Count == 0)
            {

                if (!this.m_bountyRequirementController.HasFulfilledBountyRequirement())
                {
                    GameEventBus<BountyRequirementNotFulfilled>.Invoke(this, new());
                    return;
                }
                
                this.StartCoroutine(this.QueueAfterFight());
                return;
            }
            
            this.StartCoroutine(this.QueueEnemy());
        }

        private IEnumerator QueueEnemy()
        {
            yield return new WaitForSeconds(1f);
            var nextEnemy = this.m_enemyQueue.Dequeue();
            this.CreateEnemy(nextEnemy);
            this.m_playerAttacker.ResetAttack();
            this.m_currentEnemyAttacker.CanAttack = true;
            this.m_playerAttacker.CanAttack = true;
        }

        private IEnumerator QueueAfterFight()
        {
            yield return new WaitForSeconds(1f);
            GameEventBus<BountyRequirementFulfilled>.Invoke(this, new());
            this.m_fightUI.ShowAfterFightUI();
        }

        private void ResetEnemy()
        {
            if (this.m_currentEnemyInformation != null)
            {
                Destroy(this.m_currentEnemyInformation.gameObject);
                this.m_currentEnemyInformation = null;
            }
            
            if (this.m_currentEnemyAttacker != null)
            {
                Destroy(this.m_currentEnemyAttacker.gameObject);
                this.m_currentEnemyAttacker = null;
            }

            this.m_currentEnemyData = null;
        }
    }
}