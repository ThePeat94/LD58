using Nidavellir.Entity;
using Nidavellir.Player;
using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Fight
{
    public class ProfileFightCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private Image m_background;
        [SerializeField] private Image m_profilePicture;
        [SerializeField] private HpBarDisplay m_hpBarDisplay;
        [SerializeField] private AtkSpeedDisplay m_atkSpeedDisplay;
        [SerializeField] private CharacterStatFacade m_characterStatFacade;
        
        public void DisplayProfile(EntityInformation entityInformation, EntityAttacker attacker)
        {
            this.m_name.text = entityInformation.Name;
            this.m_profilePicture.sprite = entityInformation.ProfilePicture;
            this.m_background.sprite = entityInformation.BackgroundImage;
            this.m_hpBarDisplay.Init(entityInformation.EntityStats[this.m_characterStatFacade.Hp]);
            this.m_atkSpeedDisplay.Init(attacker);
        }
    }
}