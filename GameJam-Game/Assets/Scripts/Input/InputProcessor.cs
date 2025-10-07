using UnityEngine;
using UnityEngine.InputSystem;

namespace Nidavellir.Input
{
    public class InputProcessor : MonoBehaviour
    {
        private PlayerInput m_playerInput;

        public bool QuitTriggered => this.m_playerInput.Actions.Quit.triggered;
        public bool BackToMainTriggered => this.m_playerInput.Actions.BackToMenu.triggered;
        public bool RetryTriggered => this.m_playerInput.Actions.Retry.triggered;
        
        private void Awake()
        {
            this.m_playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            this.m_playerInput?.Enable();
        }

        private void OnDisable()
        {
            this.m_playerInput?.Disable();
        }
    }
}