using Nidavellir.EventBus;
using Nidavellir.EventBus.Events.Location;
using Nidavellir.Scriptables.Location;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Nidavellir.UI.Location
{
    public class LocationCardUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI m_locationName;
        [SerializeField] private Image m_locationImage;
        
        private BaseLocationData m_locationData;
        
        public void ShowLocation(BaseLocationData locationData)
        {
            this.m_locationData = locationData;
            this.m_locationName.text = this.m_locationData.Name;
            this.m_locationImage.sprite = this.m_locationData.BackgroundImage;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameEventBus<LocationSelectedEvent>.Invoke(this, new (this.m_locationData));
        }
    }
}