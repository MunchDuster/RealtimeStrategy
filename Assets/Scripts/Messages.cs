using UnityEngine;

namespace Munchy
{
    #region BaseTypes
    // Base type for all messages
    public class Message { }

    public class GenericMessage<T> : Message
    {
        public T data;
        public GenericMessage(T data) => this.data = data;
    }

    /// <summary>
    /// For when a player tries to do something thats nono
    /// </summary>
    public class ErrorMessage : GenericMessage<string>
    {
        public ErrorMessage(string data) : base(data) { }
    }
    #endregion
    #region Networking
    //These are the message types that this namespace uses
    public class OnServerConnectedMessage : Message { }
    public class OnLobbyJoinedMessage : Message { }
    public class OnRoomJoinedMessage : Message { }
    #endregion
    #region UnitMessages
    public class MoneyChangedMessage : GenericMessage<int>
    {
        public MoneyChangedMessage(int data) : base(data) { }
    }

    public class FoodChangedMessage : Message
    {
        public int food;
        public int foodUsed;

        public FoodChangedMessage(int food, int foodUsed)
        {
            this.food = food;
            this.foodUsed = foodUsed;
        }
    }

    public class PlayerNameChangedMessage : GenericMessage<string>
    {
        public PlayerNameChangedMessage(string data) : base(data) { }
    }

    public class PlayerColorChangedMessage : Message
    {
        public Color32 primaryColor;
        public Color32 secondaryColor;

        public PlayerColorChangedMessage(Color32 primaryColor, Color32 secondaryColor)
        {
            this.primaryColor = primaryColor;
            this.secondaryColor = secondaryColor;
        }
    }

    public class LocalUnitCreatedMessage : Message { }

    public class LocalUnitDestroyedMessage : Message { }

    public class LocalUnitQueuedMessage : Message { }

    public class PopupOpenRequest : Message
    {
        public GameObject popupPrefab;
        public MonoBehaviour unitCaller;

        public PopupOpenRequest(GameObject popupPrefab, MonoBehaviour unitCaller)
        {
            this.popupPrefab = popupPrefab;
            this.unitCaller = unitCaller;
        }
    }
    #endregion
}