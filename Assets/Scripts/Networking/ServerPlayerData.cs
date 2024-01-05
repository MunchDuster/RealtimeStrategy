using Photon.Pun;
using UnityEngine;

namespace Munchy.Networking
{
    public class ServerPlayerData : MonoBehaviour, IPunObservable
    {
        public static ServerPlayerData Local;
        
        private static Color32[] primaryColors = 
        {
            new Color32(066, 120, 138, 255),
            new Color32(153, 058, 051, 255),
            new Color32(105, 051, 153, 255),
            new Color32(051, 153, 100, 255),
            new Color32(153, 126, 051, 255)
        };
        private static Color32[] secondaryColors =
        {
            new Color32(066, 120, 138, 255),
            new Color32(153, 058, 051, 255),
            new Color32(105, 051, 153, 255),
            new Color32(051, 153, 100, 255),
            new Color32(153, 126, 051, 255)
        };

        [HideInInspector] public PhotonView photonView;

        public Color primaryColor => primaryColors[playerIndex];
        public Color secondaryColor => secondaryColors[playerIndex];

        [HideInInspector] public short playerIndex;
        public string playerName;

        private ShortHandler _colorHandler;

        private void Awake()
        {
            _colorHandler = new(null, ref playerIndex);
            _colorHandler.Update();

            if (photonView.IsMine)
            {
                Local = this;
                Communication.Call(new PlayerNameChangedMessage(playerName));
                Communication.Call(new PlayerColorChangedMessage(primaryColor, secondaryColor));
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) => _colorHandler.HandleStream(stream);
    }
}