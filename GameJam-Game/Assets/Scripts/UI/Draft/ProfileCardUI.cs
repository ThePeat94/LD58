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
        
        private const string REWARD_FORMAT = "Reward: {0} Gold";
        
        private const string DISTANCE_FORMAT = "{0} km away";
        
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_description;
        [SerializeField] private TextMeshProUGUI m_distance;
        [SerializeField] private TextMeshProUGUI m_reward;
        [SerializeField] private Image m_profilePicture;
        [SerializeField] private Image m_profileBackground;
        [SerializeField] private Sprite m_defaultBackground;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        
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
            
            var rewardAmount = enemyData.Stats[this.m_characterStatFacade.Money];
            this.m_reward.text = String.Format(REWARD_FORMAT, rewardAmount);
            
            var distanceAmount = enemyData.Stats[this.m_characterStatFacade.Distance];
            this.m_distance.text = String.Format(DISTANCE_FORMAT, distanceAmount);
        }
    }
}