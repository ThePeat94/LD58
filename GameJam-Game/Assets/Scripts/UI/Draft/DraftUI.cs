using System;
using System.Collections.Generic;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events;
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
        [SerializeField] private Button m_startFightButton;
        [SerializeField] private ProfileCardUI m_profileCardUI;

        [SerializeField] private GameObject m_profilesUi;
        [SerializeField] private GameObject m_startFightUi;
        
        private List<EnemyData> m_likedProfiles = new();
        
        private void Awake()
        {
            this.m_startFightButton.onClick.AddListener(this.OnStartFightClick);
            this.m_profileCardUI ??= this.GetComponentInChildren<ProfileCardUI>();
        }

        public void DisplayProfile(EnemyData enemyData)
        {
            this.m_profileCardUI.DisplayEnemy(enemyData);
        }

        public void ShowProfiles()
        {
            this.m_likedProfiles.Clear();
            this.m_profilesUi.SetActive(true);
            this.m_startFightUi.SetActive(false);
        }

        public void ShowStartFight(List<EnemyData> likedProfiles)
        {
            this.m_likedProfiles = likedProfiles;
            this.m_profilesUi.SetActive(false);
            this.m_startFightUi.SetActive(true);
        }

        private void OnStartFightClick()
        {
            this.m_profilesUi.SetActive(false);
            this.m_startFightUi.SetActive(false);
            GameEventBus<StartFightEvent>.Invoke(this, new (this.m_likedProfiles));
        }
    }
}