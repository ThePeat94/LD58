using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.Scriptables;
using UnityEngine;

namespace Nidavellir.Draft
{
    public class DraftManager : MonoBehaviour
    {
        private EnemyData m_currentProfile;

        private IEventBinding<ProfileDislikedEvent> m_dislikedEventBinding;
        private IEventBinding<ProfileLikedEvent> m_likedEventBinding;
        private IEventBinding<ProfileSuperLikedEvent> m_superLikedEventBinding;
        
        private void Start()
        {
            this.m_dislikedEventBinding = new EventBinding<ProfileDislikedEvent>(this.OnDislikeClick);
            GameEventBus<ProfileDislikedEvent>.Register(this.m_dislikedEventBinding);

            this.m_likedEventBinding = new EventBinding<ProfileLikedEvent>(this.OnLikeClick);
            GameEventBus<ProfileLikedEvent>.Register(this.m_likedEventBinding);
            
            this.m_superLikedEventBinding = new EventBinding<ProfileSuperLikedEvent>(this.OnSuperLikeClick);
            GameEventBus<ProfileSuperLikedEvent>.Register(this.m_superLikedEventBinding);
        }

        private void OnDislikeClick(object sender, ProfileDislikedEvent e)
        {
            Debug.Log("Ohhh :(");
        }

        private void OnLikeClick(object sender, ProfileLikedEvent e)
        {
            Debug.Log("Yay, its a match :) Literally.");
        }

        private void OnSuperLikeClick(object sender, ProfileSuperLikedEvent e)
        {
            Debug.Log("You really want to fight this dude, right?");
        }
    }
}