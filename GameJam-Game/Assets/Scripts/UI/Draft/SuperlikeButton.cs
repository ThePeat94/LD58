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
    public class SuperlikeButton : MonoBehaviour
    {
        [SerializeField] private Button m_button;
        [SerializeField] private EntityStats m_playerStats;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        [SerializeField] private DraftManager m_draftManager;
        [SerializeField] private SfxData m_superlikeSfx;
        
        private SfxPlayer m_superlikeSfxPlayer;
        
        private void Awake()
        {
            this.m_superlikeSfxPlayer = this.GetOrAddComponent<SfxPlayer>();
            this.m_playerStats ??= FindFirstObjectByType<EntityStats>(FindObjectsInactive.Include);
            this.m_draftManager ??= FindFirstObjectByType<DraftManager>();
            this.m_button.onClick.AddListener(this.OnButtonClick);
        }

        private void Start()
        {
            this.m_playerStats[this.m_characterStatFacade.SuperLike].OnValueChanged += this.OnSuperlikeChanged;
        }

        private void OnSuperlikeChanged(object sender, CharacterStatValueChangeEventArgs e)
        {
            this.m_button.interactable = e.NewValue > 0;
        }
        
        private void OnDisable()
        {
            if (this.m_superlikeSfxPlayer is null)
                return;
            
            this.m_superlikeSfxPlayer.StopPlaying();
        }
        
        private void OnButtonClick()
        {
            GameEventBus<ProfileSuperLikedEvent>.Invoke(this, new ProfileSuperLikedEvent(this.m_draftManager.CurrentProfile));
            this.m_superlikeSfxPlayer.PlayOneShot(this.m_superlikeSfx);
        }
    }
}