using Nidavellir.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nidavellir.UI.Draft
{
    public class ProfileStatUI : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private TextMeshProUGUI m_value;
        
        public void Setup(Sprite icon, int value)
        {
            if (this.m_icon is not null && icon is not null)
                this.m_icon.sprite = icon;
            this.m_value.text = value.ToString();
        }

        public void Setup(Sprite icon, string text)
        {
            if (this.m_icon is not null && icon is not null)
                this.m_icon.sprite = icon;
            this.m_value.text = text;
        }
    }
}