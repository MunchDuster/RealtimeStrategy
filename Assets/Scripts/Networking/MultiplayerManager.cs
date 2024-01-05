using UnityEngine;
using Photon.Pun;

namespace Munchy.Networking 
{   
    public class MultiplayerManager : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public string Lobby = "test";

        [Header("Settings")]
        [Tooltip("Enable for debugging")]
        [SerializeField] private bool instaConnect = true;

        [Header("Prefabs")]
        [SerializeField] private GameObject serverPlayerData;
        [SerializeField] private GameObject localPlayerData;

        private void Awake()
        {
            if (instaConnect) Connect();
        }

        public void Connect()
        {
            Debug.Log("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            Debug.Log("Connected to server!");
            Communication.Call(new OnServerConnectedMessage());

            Debug.Log($"Joining lobby...");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            Debug.Log("Joined lobby!");
            Communication.Call(new OnLobbyJoinedMessage());

            Debug.Log($"Creating or joining room  ({Lobby})...");
            PhotonNetwork.JoinOrCreateRoom("test", null, null);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Debug.Log("Joined room!");
            Communication.Call(new OnRoomJoinedMessage());

            InstantiatePlayer();
        }

        private void InstantiatePlayer()
        {
            GameObject serverDataGO = PhotonNetwork.Instantiate(serverPlayerData.name, Vector3.zero, Quaternion.identity);
            ServerPlayerData data = serverDataGO.GetComponent<ServerPlayerData>();
            data.playerIndex = (short)PhotonNetwork.PlayerList.Length;

            Instantiate(localPlayerData); // This handles stuff that other players shouldn't know about
        }
    }
}