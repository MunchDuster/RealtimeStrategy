using UnityEngine;

namespace Munchy.UI
{
    public class NetworkStatusCheckList : MonoBehaviour
    {
        [Header("Checklist [optional]")]
        [SerializeField] private ChecklistItem connectedServerItem;
        [SerializeField] private ChecklistItem joinedLobbyItem;
        [SerializeField] private ChecklistItem joinedRoomItem;

        private void Start()
        {
            Communication.Listen((OnServerConnectedMessage message) => connectedServerItem.SetCompleted(), this);
            Communication.Listen((OnLobbyJoinedMessage message) => joinedLobbyItem.SetCompleted(), this);
            Communication.Listen((OnRoomJoinedMessage message) => joinedRoomItem.SetCompleted(), this);
        }
    }
}