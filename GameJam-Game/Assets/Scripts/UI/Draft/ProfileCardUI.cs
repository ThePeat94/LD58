using System;
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
        
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_description;
        [SerializeField] private Image m_profilePicture;
        [SerializeField] private Image m_profileBackground;
        [SerializeField] private Sprite m_defaultBackground;
        
        

        public void DisplayEnemy(EnemyData enemyData)
        {
            this.m_name.text = String.Format(NAME_FORMAT, enemyData.Name, 35);
            this.m_description.text = enemyData.ProfileDescription;
            this.m_profilePicture.sprite = enemyData.Icon;
            
            if (enemyData.PossibleBackgrounds is null or { Count: 0 })
            {
                this.m_profileBackground.sprite = this.m_defaultBackground;
            }
            else
            {
                this.m_profileBackground.sprite = enemyData.PossibleBackgrounds[UnityEngine.Random.Range(0, enemyData.PossibleBackgrounds.Count)];
            }
        }
    }
}