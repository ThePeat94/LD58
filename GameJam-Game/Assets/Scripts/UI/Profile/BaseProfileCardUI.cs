using System;
using System.Linq;
using Nidavellir.EventBus;
using Nidavellir.EventBus.Events.Draft;
using Nidavellir.Scriptables;
using Nidavellir.UI.Draft;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Nidavellir.UI.Profile
{
    public class BaseProfileCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private const string NAME_FORMAT = "{0}, {1}";
        
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_description;
        
        [SerializeField] private Image m_profilePicture;
        [SerializeField] private Image m_profileBackground;
        [SerializeField] private Sprite m_defaultBackground;

        [SerializeField] private ProfileStatsCardUI m_profileStatsCardUI;
        
        private RuntimeEnemyInformation m_displayedEnemy;

        private void Awake()
        {
            // this.m_profileStatsCardUI ??= this.GetComponentInChildren<ProfileStatsCardUI>();
            // this.m_profileStatsCardUI.gameObject.SetActive(false);
        }


        public void DisplayEnemy(RuntimeEnemyInformation enemyData)
        {
            this.m_displayedEnemy = enemyData;
            this.m_name.text = String.Format(NAME_FORMAT, enemyData.BaseData.Name, enemyData.BaseData.Age);
            this.m_description.text = enemyData.BaseData.ProfileDescription;
            this.m_profilePicture.sprite = enemyData.BaseData.Icon;
            
            this.m_profileBackground.sprite = enemyData.BaseData.PossibleBackgrounds is null or { Count: 0 } ? 
                this.m_defaultBackground : 
                enemyData.BaseData.PossibleBackgrounds[UnityEngine.Random.Range(0, enemyData.BaseData.PossibleBackgrounds.Count)];
            
            // this.m_profileStatsCardUI.SetupEnemyStats(enemyData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // this.m_profileStatsCardUI.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // this.m_profileStatsCardUI.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"Selected profile: {this.m_displayedEnemy.BaseData.Name}");
            GameEventBus<ProfileLikedEvent>.Invoke(this, new(this.m_displayedEnemy));
        }
    }
}