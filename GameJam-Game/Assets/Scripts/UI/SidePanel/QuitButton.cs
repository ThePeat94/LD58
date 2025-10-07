using UnityEngine;

namespace Nidavellir.UI.SidePanel
{
    public class QuitButton : MonoBehaviour
    {
        public void QuitApplication()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                return;
            Application.Quit();
        }
    }
}