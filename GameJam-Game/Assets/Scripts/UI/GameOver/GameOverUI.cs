using System;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.EventBindings;
using Nidavellir.GameEventBus.Events;
using Nidavellir.GameState;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nidavellir.UI.GameOver
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_reasonText;
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private Button m_quitButton;
        [SerializeField] private GameObject m_panel;
        

        private void Awake()
        {
            this.m_panel.SetActive(false);
            this.m_restartButton.onClick.AddListener(this.ReloadScene);
            this.m_mainMenuButton.onClick.AddListener(this.GoToMainMenu);
            this.m_quitButton.onClick.AddListener(this.QuitApplication);
        }

        public void Hide()
        {
            this.m_panel.SetActive(false);
        }

        public void ShowGameOverPanel(GameOverReason reason)
        {
            this.m_panel.SetActive(true);
            this.m_reasonText.text = reason switch
            {
                GameOverReason.BountyNotFulFilled => "You did not fulfill your bounty requirement!",
                GameOverReason.Died => "You have died in battle!",
                _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
            };
        }

        public void QuitApplication()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            
            Application.Quit();
        }
        
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}