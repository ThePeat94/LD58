using System;
using System.Collections.Generic;
using System.Linq;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.GameState;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using UnityEngine;
using Random = System.Random;

namespace Nidavellir.Draft
{
    public class DraftManager : MonoBehaviour
    {
        [SerializeField] private DraftUI m_draftUI;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private List<ProfilePoolData> m_poolsPerRizzLevel;
        
        private RuntimeEnemyInformation m_currentProfile;

        private IEventBinding<ProfileDislikedEvent> m_dislikedEventBinding;
        private IEventBinding<ProfileLikedEvent> m_likedEventBinding;
        private IEventBinding<ProfileSuperLikedEvent> m_superLikedEventBinding;
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;

        private List<RuntimeEnemyInformation> m_likedProfiles = new();
        private List<RuntimeEnemyInformation> m_dislikedProfiles = new();
        private List<RuntimeEnemyInformation> m_superLikedProfiles = new();
        
        private List<EnemyData> m_availableNonBossProfiles;
        private List<EnemyData> m_availableBossProfiles;

        private List<EnemyData> m_profilePool;
        
        public RuntimeEnemyInformation CurrentProfile => this.m_currentProfile;
        
        private void Awake()
        {
            this.m_draftUI ??= FindFirstObjectByType<DraftUI>();
        }

        private void Start()
        {
            this.m_dislikedEventBinding = new EventBinding<ProfileDislikedEvent>(this.OnDislikeEvent);
            GameEventBus<ProfileDislikedEvent>.Register(this.m_dislikedEventBinding);

            this.m_likedEventBinding = new EventBinding<ProfileLikedEvent>(this.OnLikeEvent);
            GameEventBus<ProfileLikedEvent>.Register(this.m_likedEventBinding);

            this.m_superLikedEventBinding = new EventBinding<ProfileSuperLikedEvent>(this.OnSuperLikeEvent);
            GameEventBus<ProfileSuperLikedEvent>.Register(this.m_superLikedEventBinding);
            
            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFightEvent);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);
            
            this.m_startDraftEventBinding = new EventBinding<StartDraftEvent>(this.OnStartDraftEvent);
            GameEventBus<StartDraftEvent>.Register(this.m_startDraftEventBinding);
            
            var rizzIndex = Math.Min(this.m_playerStats[this.m_characterStatFacade.Rizz].CurrentValue - 1, this.m_poolsPerRizzLevel.Count - 1);
            this.m_availableNonBossProfiles = new List<EnemyData>(this.m_poolsPerRizzLevel[rizzIndex].NonBossProfiles);
            this.m_availableBossProfiles = new List<EnemyData>(this.m_poolsPerRizzLevel[rizzIndex].BossProfiles);
            
            this.StartDraft();
        }
        
        private void OnDestroy()
        {
            GameEventBus<ProfileDislikedEvent>.Unregister(this.m_dislikedEventBinding);
            GameEventBus<ProfileLikedEvent>.Unregister(this.m_likedEventBinding);
            GameEventBus<ProfileSuperLikedEvent>.Unregister(this.m_superLikedEventBinding);
        }
        
        private void ChooseNewProfile()
        {
            var roundController = this.m_playerStats[this.m_characterStatFacade.Round];
            List<EnemyData> anchorProfiles;
            if (roundController.CurrentValue % 3 == 0)
            {
                anchorProfiles = this.m_availableBossProfiles;
            }
            else
            {
                anchorProfiles = this.m_availableNonBossProfiles;
            }

            var selectedProfile = anchorProfiles[UnityEngine.Random.Range(0, anchorProfiles.Count)];
            this.m_currentProfile = this.CreateRuntimeEnemyInformation(selectedProfile);
            this.m_draftUI.DisplayProfile(this.m_currentProfile);
            anchorProfiles.Remove(selectedProfile);
        }
        
        private RuntimeEnemyInformation CreateRuntimeEnemyInformation(EnemyData enemyData)
        {
            var stats = enemyData.ScalableStats.ScalableStats.ToDictionary(stat => stat.Stat, stat => stat.BaseValue);
            stats.Add(this.m_characterStatFacade.Distance, UnityEngine.Random.Range(4, 20));
            stats.Add(this.m_characterStatFacade.Money, UnityEngine.Random.Range(4, 12));
            return new RuntimeEnemyInformation(enemyData, stats, 100);
        }

        private void StartDraft()
        {
            this.m_likedProfiles.Clear();
            this.m_dislikedProfiles.Clear();
            this.m_superLikedProfiles.Clear();
            this.ChooseNewProfile();
            this.m_draftUI.ShowProfiles();
        }
        
        private void OnStartDraftEvent(object sender, StartDraftEvent e)
        {
            var roundStatController = this.m_playerStats[this.m_characterStatFacade.Round];
            roundStatController.Add(1);

            if ((roundStatController.CurrentValue - 1) % 3 == 0 && roundStatController.CurrentValue != 1)
            {
                var rizzController = this.m_playerStats[this.m_characterStatFacade.Rizz];
                rizzController.Add(1);
                
                var rizzIndex = Math.Min(this.m_playerStats[this.m_characterStatFacade.Rizz].CurrentValue - 1, this.m_poolsPerRizzLevel.Count - 1);
                this.m_availableNonBossProfiles = new List<EnemyData>(this.m_poolsPerRizzLevel[rizzIndex].NonBossProfiles);
                this.m_availableBossProfiles = new List<EnemyData>(this.m_poolsPerRizzLevel[rizzIndex].BossProfiles);
            }
            
            this.StartDraft();
        }

        private void OnDislikeEvent(object sender, ProfileDislikedEvent e)
        {
            this.m_dislikedProfiles.Add(e.EnemyData);
            var playerDislikes = this.m_playerStats[this.m_characterStatFacade.Dislikes];
            playerDislikes.UseResource(1);
            this.ChooseNewProfile();
        }

        private void OnLikeEvent(object sender, ProfileLikedEvent e)
        {
            this.m_likedProfiles.Add(e.EnemyData);
            var playerLikes = this.m_playerStats[this.m_characterStatFacade.Likes];
            playerLikes.UseResource(1);
            if (playerLikes.CurrentValue > 0)
            {
                this.ChooseNewProfile();
            }
            else
            {
                this.m_draftUI.ShowStartFight(this.m_likedProfiles);
            }
        }

        private void OnSuperLikeEvent(object sender, ProfileSuperLikedEvent e)
        {
            this.m_superLikedProfiles.Add(e.EnemyData);
            var playerSuperlikes = this.m_playerStats[this.m_characterStatFacade.SuperLike];
            playerSuperlikes.UseResource(1);
            this.ChooseNewProfile();
        }
        
        private void OnStartFightEvent(object sender, StartFightEvent e)
        {
            this.m_likedProfiles.Clear();
            this.m_dislikedProfiles.Clear();
            this.m_superLikedProfiles.Clear();
        }
    }
}