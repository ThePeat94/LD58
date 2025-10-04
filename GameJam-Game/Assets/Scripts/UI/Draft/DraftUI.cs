using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Draft
{
    public class DraftUI : MonoBehaviour
    {
        [SerializeField] private Button m_dislikeButton;
        [SerializeField] private Button m_likeButton;
        [SerializeField] private Button m_superlikeButton;

        [SerializeField] private EnemyData m_currentEnemy;
        

        private void Awake()
        {
            this.m_dislikeButton.onClick.AddListener(this.OnDislikeClick);
            this.m_likeButton.onClick.AddListener(this.OnLikeClick);
            this.m_superlikeButton.onClick.AddListener(this.OnSuperLikeClick);
        }

        private void OnDislikeClick()
        {
            GameEventBus<ProfileDislikedEvent>.Invoke(this, new(this.m_currentEnemy));
        }

        private void OnLikeClick()
        {
            GameEventBus<ProfileLikedEvent>.Invoke(this, new(this.m_currentEnemy));
        }

        private void OnSuperLikeClick()
        {
            GameEventBus<ProfileSuperLikedEvent>.Invoke(this, new(this.m_currentEnemy));
        }
    }
}