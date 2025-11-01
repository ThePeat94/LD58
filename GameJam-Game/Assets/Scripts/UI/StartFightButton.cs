using System;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI
{
    public class StartFightButton : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        
        private Action m_buttonClicked;
        
        public event Action OnButtonClicked
        {
            add => this.m_buttonClicked += value;
            remove => this.m_buttonClicked -= value;
        }

        private void Awake()
        {
            this.m_button ??= this.GetComponent<Button>();
            this.m_button.onClick.AddListener(this.HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            this.m_buttonClicked?.Invoke();
        }

        public void Enable()
        {
            this.m_button.interactable = true;
        }

        public void Disable()
        {
            this.m_button.interactable = false;
        }
    }
}