using System;
using Nidavellir.Audio;
using Nidavellir.Draft;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.Scriptables;
using Nidavellir.Scriptables.Audio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Draft
{
    public class DislikeButton : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private DraftManager m_draftManager;
        [SerializeField] private SfxData m_dislikeSfxData;
        
        private SfxPlayer m_sfxPlayer;

        private void Awake()
        {
            this.m_sfxPlayer = this.GetOrAddComponent<SfxPlayer>();
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
            this.m_draftManager ??= FindFirstObjectByType<DraftManager>();
            this.m_button.onClick.AddListener(this.OnButtonClick);
        }

        private void Start()
        {
            this.m_playerStats[this.m_characterStatFacade.Dislikes].OnValueChanged += this.OnDislikeChanged;
        }

        private void OnDislikeChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_button.interactable = e.NewValue > 0;
        }
        
        private void OnDisable()
        {
            if (this.m_sfxPlayer is null)
                return;
            
            this.m_sfxPlayer.StopPlaying();
        }

        private void OnButtonClick()
        {
            GameEventBus<ProfileDislikedEvent>.Invoke(this, new ProfileDislikedEvent(this.m_draftManager.CurrentProfile));
            this.m_sfxPlayer.PlayOneShot(this.m_dislikeSfxData);
        }
    }
}