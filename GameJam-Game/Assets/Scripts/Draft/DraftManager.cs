using System.Collections.Generic;
using System.Linq;
using Nidavellir.Entity;
using Nidavellir.EventBus;
using Nidavellir.EventBus.EventBindings;
using Nidavellir.EventBus.Events.Draft;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.Fight;
using Nidavellir.Location;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Location;
using Nidavellir.UI;
using Nidavellir.UI.Draft;
using Nidavellir.UI.EnemyDraft;
using Nidavellir.Util;
using UnityEngine;

namespace Nidavellir.Draft
{
    public class DraftManager : MonoBehaviour
    {
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private EnemyFactory m_enemyFactory;
        [SerializeField] private EnemySelectionUI m_enemySelectionUI;
        [SerializeField] private LocationDraftManager m_locationDraftManager;
        [SerializeField] private FightManager m_fightManager;
        
        private readonly List<RuntimeEnemyInformation> m_likedProfiles = new();
        private readonly List<RuntimeEnemyInformation> m_superLikedProfiles = new();
        
        private List<RuntimeEnemyInformation> m_allSelectedProfiles = new();
        
        private List<EnemyData> m_availableNonBossProfiles;
        private List<EnemyData> m_availableBossProfiles;

        private EnemyLocationData m_selectedEnemyLocation;
        
        private readonly List<RuntimeEnemyInformation> m_availableForSelection = new();
        
        private void Awake()
        {
            this.m_enemyFactory ??= FindFirstObjectByType<EnemyFactory>(FindObjectsInactive.Include);
            this.m_enemySelectionUI ??= FindFirstObjectByType<EnemySelectionUI>(FindObjectsInactive.Include);
            this.m_fightManager ??= FindFirstObjectByType<FightManager>(FindObjectsInactive.Include);
            
            this.m_enemySelectionUI.OnProfileLiked += this.HandleProfileLiked;
        }


        private void OnDestroy()
        {
            this.m_enemySelectionUI.OnProfileLiked -= this.HandleProfileLiked;
        }
        
        private void ChooseNewProfiles()
        {
            this.m_availableForSelection.Clear();
            var availableProfiles = new List<EnemyData>(this.m_availableNonBossProfiles);
            for (var i = 0; i < 3; i++)
            {
                var selectedProfile = availableProfiles.GetRandomElement();
                var runtimeEnemyInformation = this.m_enemyFactory.CreateEnemy(selectedProfile);
                this.m_availableForSelection.Add(runtimeEnemyInformation);
                availableProfiles.Remove(selectedProfile);
            }
        }
        

        public void StartDraft(List<EnemyData> availableNonBossProfiles, List<EnemyData>  availableBossProfiles)
        {
            this.m_availableNonBossProfiles = new List<EnemyData>(availableNonBossProfiles);
            this.m_availableBossProfiles = new List<EnemyData>(availableBossProfiles);
            this.m_likedProfiles.Clear();
            this.m_superLikedProfiles.Clear();
            this.ChooseNewProfiles();
            this.m_enemySelectionUI.UpdateDisplayedProfiles(this.m_availableForSelection);
        }

        public void HandleProfileLiked(RuntimeEnemyInformation enemy)
        {
            this.m_likedProfiles.Add(enemy);
            var playerLikes = this.m_playerStats[this.m_characterStatFacade.Likes];
            playerLikes.UseResource(1);
            if (playerLikes.CurrentValue > 0)
            {
                this.ChooseNewProfiles();
                this.m_enemySelectionUI.UpdateDisplayedProfiles(this.m_availableForSelection);
            }
            else
            {
                this.m_allSelectedProfiles = this.m_likedProfiles.Concat(this.m_superLikedProfiles).ToList();
                this.m_fightManager.StartFight(this.m_allSelectedProfiles);
            }
            GameEventBus<ProfileLikedEvent>.Invoke(this, new ProfileLikedEvent(enemy));
        }

        private void OnSuperLikeEvent(object sender, ProfileSuperLikedEvent e)
        {
            this.m_enemyFactory.AmplifyEnemyForSuperlike(e.Enemy);
            this.m_superLikedProfiles.Add(e.Enemy);
            var playerSuperlikes = this.m_playerStats[this.m_characterStatFacade.SuperLike];
            playerSuperlikes.UseResource(1);
            this.ChooseNewProfiles();
            GameEventBus<ProfileSuperLikedEvent>.Invoke(this, new ProfileSuperLikedEvent(e.Enemy));
        }
    }
}