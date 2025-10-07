using System;
using Nidavellir.Audio;
using Nidavellir.Scriptables.Audio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI
{
    public class ButtonSfxPlayer : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        [SerializeField] private SfxData m_sfxData;

        private SfxPlayer m_sfxPlayer;
        
        private void Awake()
        {
            this.m_sfxPlayer = this.GetOrAddComponent<SfxPlayer>();
            this.m_button ??= this.GetComponent<Button>();
            this.m_button.onClick.AddListener(this.OnButtonClick);
        }

        private void OnDisable()
        {
            this.m_sfxPlayer?.StopPlaying();
        }

        private void OnButtonClick()
        {
            this.m_sfxPlayer?.PlayOneShot(this.m_sfxData);
        }
    }
}