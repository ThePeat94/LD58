using System;
using System.Collections.Generic;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Draft;
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

        private List<EnemyData> m_likedProfiles = new();
        private List<EnemyData> m_availableProfiles;

        private void Awake()
        {
            this.m_draftUI ??= FindFirstObjectByType<DraftUI>();
        }

        private void Start()
        {
            this.m_dislikedEventBinding = new EventBinding<ProfileDislikedEvent>(this.OnDislikeClick);
            GameEventBus<ProfileDislikedEvent>.Register(this.m_dislikedEventBinding);

            this.m_likedEventBinding = new EventBinding<ProfileLikedEvent>(this.OnLikeClick);
            GameEventBus<ProfileLikedEvent>.Register(this.m_likedEventBinding);

            this.m_superLikedEventBinding = new EventBinding<ProfileSuperLikedEvent>(this.OnSuperLikeClick);
            GameEventBus<ProfileSuperLikedEvent>.Register(this.m_superLikedEventBinding);
            
            this.m_startFightEventBinding = new EventBinding<StartFightEvent>(this.OnStartFightClick);
            GameEventBus<StartFightEvent>.Register(this.m_startFightEventBinding);

            this.m_availableProfiles = new List<EnemyData>(this.m_initialProfiles);
            this.ChooseNewProfile();
            this.m_draftUI.ShowProfiles();
        }

        private void OnDestroy()
        {
            GameEventBus<ProfileDislikedEvent>.Unregister(this.m_dislikedEventBinding);
            GameEventBus<ProfileLikedEvent>.Unregister(this.m_likedEventBinding);
            GameEventBus<ProfileSuperLikedEvent>.Unregister(this.m_superLikedEventBinding);
        }

        private void OnDislikeClick(object sender, ProfileDislikedEvent e)
        {
            Debug.Log("Ohhh :(");
        }

        private void OnLikeClick(object sender, ProfileLikedEvent e)
        {
            Debug.Log("Yay, its a match :) Literally.");
            this.m_likedProfiles.Add(e.EnemyData);
            var playerLikes = this.m_playerStats[this.m_characterStatFacade.Likes];
            playerLikes.UseResource(1);
            Debug.Log($"You have {playerLikes.CurrentValue} likes remaining.");
            if (playerLikes.CurrentValue > 0)
            {
                this.ChooseNewProfile();
            }
            else
            {
                this.m_draftUI.ShowStartFight(this.m_likedProfiles);
            }
        }

        private void OnSuperLikeClick(object sender, ProfileSuperLikedEvent e)
        {
            Debug.Log("You really want to fight this dude, right?");
        }

        private void ChooseNewProfile()
        {
            this.m_currentProfile = this.m_availableProfiles[UnityEngine.Random.Range(0, this.m_availableProfiles.Count)];
            this.m_draftUI.DisplayProfile(this.m_currentProfile);
            this.m_availableProfiles.Remove(this.m_currentProfile);
        }
        
        private void OnStartFightClick(object sender, StartFightEvent e)
        {
            this.m_likedProfiles.Clear();
        }
    }
}