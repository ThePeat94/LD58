using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using InputProcessor = Nidavellir.Input.InputProcessor;

namespace Nidavellir
{
    public class ShortCutManager : MonoBehaviour
    {
        [SerializeField] private InputProcessor m_inputProcessor;

        private void Awake()
        {
            this.m_inputProcessor = this.GetOrAddComponent<InputProcessor>();
        }

        private void Update()
        {
            if (this.m_inputProcessor.QuitTriggered)
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                    return;
                
                Application.Quit();
                return;
            }
            
            if (this.m_inputProcessor.BackToMainTriggered)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                return;
            }
            
            if (this.m_inputProcessor.RetryTriggered)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                return;
            }
        }
    }
}