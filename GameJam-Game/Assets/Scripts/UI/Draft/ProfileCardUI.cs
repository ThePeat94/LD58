using System;
using System.Linq;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Draft
{
    public class ProfileCardUI : MonoBehaviour
    {
        /// <summary>
        /// Displays it as %NAME%, %LEVEL%
        /// </summary>
        private const string NAME_FORMAT = "{0}, {1}";
        
        private const string REWARD_FORMAT = "{0:D}g";
        
        private const string DISTANCE_FORMAT = "{0:D}km";
        
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_description;
        [SerializeField] private TextMeshProUGUI m_tags;
        
        [SerializeField] private Image m_profilePicture;
        [SerializeField] private Image m_profileBackground;
        [SerializeField] private Sprite m_defaultBackground;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;

        [SerializeField] private ProfileStatUI m_hpStatUi;
        [SerializeField] private ProfileStatUI m_attackStatUi;
        [SerializeField] private ProfileStatUI m_defenseStatUi;
        [SerializeField] private ProfileStatUI m_speedStatUi;
        [SerializeField] private ProfileStatUI m_bountyStatUi;
        [SerializeField] private ProfileStatUI m_distanceStatUi;
        [SerializeField] private ProfileStatUI m_powerStatUi;
        
        public void DisplayEnemy(RuntimeEnemyInformation enemyData)
        {
            this.m_name.text = String.Format(NAME_FORMAT, enemyData.BaseData.Name, enemyData.BaseData.Age);
            this.m_description.text = enemyData.BaseData.ProfileDescription;
            this.m_profilePicture.sprite = enemyData.BaseData.Icon;
            
            if (enemyData.BaseData.PossibleBackgrounds is null or { Count: 0 })
            {
                this.m_profileBackground.sprite = this.m_defaultBackground;
            }
            else
            {
                this.m_profileBackground.sprite = enemyData.BaseData.PossibleBackgrounds[UnityEngine.Random.Range(0, enemyData.BaseData.PossibleBackgrounds.Count)];
            }
            if (enemyData.BaseData.Tags is null || enemyData.BaseData.Tags.Count == 0)
            {
                this.m_tags.text = "No Tags";
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