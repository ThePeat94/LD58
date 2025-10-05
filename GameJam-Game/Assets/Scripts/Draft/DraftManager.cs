using System;
using System.Collections.Generic;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.GameState;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using UnityEngine;

namespace Nidavellir.Draft
{
    public class DraftManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> m_initialProfiles;
        [SerializeField] private DraftUI m_draftUI;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        private EnemyData m_currentProfile;

        private IEventBinding<ProfileDislikedEvent> m_dislikedEventBinding;
        private IEventBinding<ProfileLikedEvent> m_likedEventBinding;
        private IEventBinding<ProfileSuperLikedEvent> m_superLikedEventBinding;
        private IEventBinding<StartFightEvent> m_startFightEventBinding;
        private IEventBinding<StartDraftEvent> m_startDraftEventBinding;

        private List<EnemyData> m_likedProfiles = new();
        private List<EnemyData> m_dislikedProfiles = new();
        private List<EnemyData> m_superLikedProfiles = new();
        private List<EnemyData> m_availableProfiles;
        
        public EnemyData CurrentProfile => this.m_currentProfile;

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

            this.m_availableProfiles = new List<EnemyData>(this.m_initialProfiles);
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
            }
            
            this.m_availableProfiles = new List<EnemyData>(this.m_initialProfiles);
            this.m_likedProfiles.Clear();
            this.m_dislikedProfiles.Clear();
            this.m_superLikedProfiles.Clear();
            this.ChooseNewProfile();
            this.m_draftUI.ShowProfiles();
        }

        private void OnDestroy()
        {
            GameEventBus<ProfileDislikedEvent>.Unregister(this.m_dislikedEventBinding);
            GameEventBus<ProfileLikedEvent>.Unregister(this.m_likedEventBinding);
            GameEventBus<ProfileSuperLikedEvent>.Unregister(this.m_superLikedEventBinding);
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

        private void ChooseNewProfile()
        {
            this.m_currentProfile = this.m_availableProfiles[UnityEngine.Random.Range(0, this.m_availableProfiles.Count)];
            this.m_draftUI.DisplayProfile(this.m_currentProfile);
            this.m_availableProfiles.Remove(this.m_currentProfile);
        }
        
        private void OnStartFightEvent(object sender, StartFightEvent e)
        {
            this.m_likedProfiles.Clear();
            this.m_dislikedProfiles.Clear();
            this.m_superLikedProfiles.Clear();
        }
    }
}