using System;
using System.Linq;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using TMPro;
using UnityEngine;

namespace Nidavellir.UI.Profile
{
    public class ProfileStatsCardUI : MonoBehaviour
    {
        private const string REWARD_FORMAT = "{0:D}g";
        
        private const string DISTANCE_FORMAT = "{0:D}km";
        
        [SerializeField] private ProfileStatUI m_hpStatUi;
        [SerializeField] private ProfileStatUI m_attackStatUi;
        [SerializeField] private ProfileStatUI m_defenseStatUi;
        [SerializeField] private ProfileStatUI m_speedStatUi;
        [SerializeField] private ProfileStatUI m_bountyStatUi;
        [SerializeField] private ProfileStatUI m_distanceStatUi;
        [SerializeField] private ProfileStatUI m_powerStatUi;
        [SerializeField] private TextMeshProUGUI m_tags;
        
        [SerializeField] private CharacterStatFacade m_characterStatFacade;


        public void SetupEnemyStats(RuntimeEnemyInformation enemyData)
        {
            
            if (enemyData.BaseData.Tags is null || enemyData.BaseData.Tags.Count == 0)
            {
                this.m_tags.text = string.Empty;
            }
            else
            {
                this.m_tags.text = String.Join(", ", enemyData.BaseData.Tags.Select(t => t.Name).ToList());
            }
            
            var attackSpeedStat = enemyData.Stats[this.m_characterStatFacade.AtkSpeed];
            var attacksPerSecond = 30f/attackSpeedStat;
            
            this.m_hpStatUi.Setup(this.m_characterStatFacade.Hp.Icon, enemyData.Stats[this.m_characterStatFacade.Hp]);
            this.m_attackStatUi.Setup(this.m_characterStatFacade.Attack.Icon, enemyData.Stats[this.m_characterStatFacade.Attack]);
            this.m_defenseStatUi.Setup(this.m_characterStatFacade.Defense.Icon, enemyData.Stats[this.m_characterStatFacade.Defense]);
            this.m_speedStatUi.Setup(this.m_characterStatFacade.AtkSpeed.Icon, $"{attacksPerSecond:F1}/s");
            this.m_bountyStatUi.Setup(this.m_characterStatFacade.Bounty.Icon, String.Format(REWARD_FORMAT, enemyData.Stats[this.m_characterStatFacade.Money]));
            this.m_distanceStatUi.Setup(this.m_characterStatFacade.Distance.Icon, String.Format(DISTANCE_FORMAT, enemyData.Stats[this.m_characterStatFacade.Distance]));
            this.m_powerStatUi.Setup(null, enemyData.Power);
        }
    }
}