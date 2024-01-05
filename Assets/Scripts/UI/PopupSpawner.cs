using Munchy.Units;
using UnityEngine;

namespace Munchy.UI
{
    /// <summary>
    /// This is what spawns the popups
    /// </summary>
    public class PopupSpawner : MonoBehaviour
    {
        private void Awake()
        {
            Communication.Listen<PopupOpenRequest>(OpenPopup, this);
        }

        private void OpenPopup(PopupOpenRequest request)
        {
            GameObject instance = Instantiate(request.popupPrefab, transform);
            instance.GetComponent<Popup>().Init(request.unitCaller as Unit);
        }
    }
}