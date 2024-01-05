using UnityEngine;
using Photon.Pun;
using Munchy.Networking;
using Cysharp.Threading.Tasks;

namespace Munchy.Units.Entities
{
    /// <summary>
    /// Grid based movement, highly optimized for network using minimum data bandwidth
    /// </summary>
    public abstract class Entity : Unit, IPunObservable
    {
        [Tooltip("For when making this unit in barracks or etc.")]
        public float queueTime = 3;

        private Vector2ShortHandler _positionHandler;

        protected override void Start()
        {
            base.Start();
            _positionHandler = new(RefreshTransform, ref position);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            _positionHandler.HandleStream(stream);
        }


        protected override void OnClicked()
        {
            
        }

        private async UniTask WaitingForDestination()
        {
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        }
    }
}