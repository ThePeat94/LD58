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
        private const string NAME_FORMAT = "{0},{1}";
        
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_description;
        [SerializeField] private Image m_profilePicture;

        [SerializeField] private EnemyData m_debugData;
        
        
        private void Start()
        {
            this.DisplayEnemy(this.m_debugData);
        }

        public void DisplayEnemy(EnemyData enemyData)
        {
            this.m_name.text = String.Format(NAME_FORMAT, enemyData.Name, 35);
            this.m_description.text = enemyData.ProfileDescription;
            this.m_profilePicture.sprite = enemyData.Icon;
        }
    }
}