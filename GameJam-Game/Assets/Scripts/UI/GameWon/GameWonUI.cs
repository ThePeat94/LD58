using System;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.GameWon
{
    public class GameWonUI : MonoBehaviour
    {
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private Button m_quitButton;
        
        private void Awake()
        {
            this.m_restartButton.onClick.AddListener(this.ReloadScene);
            this.m_mainMenuButton.onClick.AddListener(this.GoToMainMenu);
            this.m_quitButton.onClick.AddListener(this.QuitApplication);
        }
        
        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void QuitApplication()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            
            Application.Quit();
        }
        
        public void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        
        public void GoToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}