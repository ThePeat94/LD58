using System;
using System.Collections.Generic;
using Nidavellir.Entity;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Shop;
using Nidavellir.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_upgradeStack;
        [SerializeField] private UpgradeCardUI m_upgradeCardPrefab;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private Button m_startDraftButton;
        
        private List<UpgradeCardUI> m_upgradeCards;

        private void Awake()
        {
            this.m_startDraftButton.onClick.AddListener(this.OnStartDraftClick);
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
        }

        public void Show(List<UpgradeData> data)
        {
            if (this.m_upgradeCards == null)
            {
                this.m_upgradeCards = new List<UpgradeCardUI>();
            }
            
            foreach (var upgradeCard in this.m_upgradeCards)
            {
                Destroy(upgradeCard.gameObject);
            }
            
            this.m_upgradeCards.Clear();
            
            foreach (var upgradeData in data)
            {
                var upgradeCardObj = Instantiate(this.m_upgradeCardPrefab, this.m_upgradeStack.transform);
                upgradeCardObj.Show(upgradeData, this.m_playerStats);
                this.m_upgradeCards.Add(upgradeCardObj);
                upgradeCardObj.BuyButton.onClick.AddListener(() => this.OnBuyUpgrade(upgradeData, upgradeCardObj));
            }
        }

        private void OnBuyUpgrade(UpgradeData data, UpgradeCardUI upgradeCardUI)
        {
            GameEventBus<PurchaseUpgradeEvent>.Invoke(this, new PurchaseUpgradeEvent(data));
            this.m_upgradeCards.Remove(upgradeCardUI);
            Destroy(upgradeCardUI.gameObject);
        }
        
        private void OnStartDraftClick()
        {
            GameEventBus<StartDraftEvent>.Invoke(this, new StartDraftEvent());
        }
    }
}