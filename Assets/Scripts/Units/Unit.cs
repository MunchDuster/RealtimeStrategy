using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Munchy.Networking;
using UnityEngine.EventSystems;

namespace Munchy.Units
{
    /// <summary>
    /// The basic building block of all units or entities in the game
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public abstract class Unit : MonoBehaviour, IPointerClickHandler
    {
        private static readonly int primaryColorID = Shader.PropertyToID("Primary");
        private static readonly int secondaryColorID = Shader.PropertyToID("Secondary");

        private static int _money;

        public static int money
        {
            get => _money;
            set { _money = value; Communication.Call(new MoneyChangedMessage(_money)); }
        }
        public static int food { get; protected set; } = 10;
        public static int foodUsed { get; protected set; }

        private static bool hasInit;

        protected virtual void Init()
        {
            hasInit = true;
            Communication.Call(new MoneyChangedMessage(_money));
            Communication.Call(new FoodChangedMessage(food, foodUsed));
        }

        public static List<Unit> LocalUnits = new();

        [HideInInspector] public bool isLocal;
        [HideInInspector] public Vector2Short position;

        [Header("Unit")]
        public string displayName = "UnitName";
        public int foodCost = 1;
        public int cost = 20;
        public int level = 1;
        [SerializeField] private GameObject popup;
        [SerializeField] private MeshRenderer[] renderers;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Communication.Call(new PopupOpenRequest(popup, this));
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked();
            }
        }

        protected virtual void OnClicked() { }

        protected PhotonView photonView;
        protected Health health;

        private static void AddUnit(Unit unit)
        {
            LocalUnits.Add(unit);
            Communication.Call(new LocalUnitCreatedMessage());
        }
        private static void RemoveUnit(Unit unit)
        {
            LocalUnits.Remove(unit);
            Communication.Call(new LocalUnitDestroyedMessage());
        }

        protected virtual void Awake()
        {
            if (!enabled) return;

            photonView = GetComponent<PhotonView>();

            isLocal = photonView.IsMine;
            position = new Vector2Short(transform.position);

            if (isLocal)
            {
                if (!hasInit)
                    Init();
                AddUnit(this);
                Communication.Listen<PlayerColorChangedMessage>(RefreshColors, this);
                RefreshColors(new PlayerColorChangedMessage(ServerPlayerData.Local.primaryColor, ServerPlayerData.Local.secondaryColor)); // In case already setup
            }
        }

        private void OnDestroy()
        {
            if (isLocal) RemoveUnit(this);
        }

        protected virtual void Start()
        {
        }

        /// <summary>
        /// Updates the primary and secondary colors to match the player (if materials named present)
        /// </summary>
        /// <param name="message"></param>
        private void RefreshColors(PlayerColorChangedMessage message)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetColor(primaryColorID, message.primaryColor);
                renderer.material.SetColor(secondaryColorID, message.secondaryColor);
            }
        }

        /// <summary>
        /// To set the material, used when showing placement outline
        /// </summary>
        /// <param name="material"></param>
        public void SetMaterial(Material material)
        {
            foreach (Renderer renderer in renderers)
                renderer.material = material;
        }

        /// <summary>
        /// Updates a transform to match the position
        /// </summary>
        protected void RefreshTransform()
        {
            transform.position = position;
        }

        public virtual bool CanSpawn()
        {
            if (food - (foodUsed + foodCost) < 0)
            {
                Communication.Call(new ErrorMessage("Not enough food"));
                return false;
            }

            if (money - cost < 0)
            {
                Communication.Call(new ErrorMessage("Not enough money"));
                return false;
            }

            return true;
        }

        public static implicit operator bool(Unit instance) => instance != null;
    }
}