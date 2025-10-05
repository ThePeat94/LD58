using System;
using Nidavellir.Draft;
using Nidavellir.Entity;
using Nidavellir.EventArgs;
using Nidavellir.GameEventBus;
using Nidavellir.GameEventBus.Events.Draft;
using Nidavellir.Scriptables;
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

        private void Awake()
        {
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

        private void OnButtonClick()
        {
            GameEventBus<ProfileDislikedEvent>.Invoke(this, new ProfileDislikedEvent(this.m_draftManager.CurrentProfile));
        }
    }
}