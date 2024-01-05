using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Munchy.Units.Buildings
{
    public class Mine : Building
    {
        [Header("Mine")]
        [SerializeField] private int moneyPerGive = 15;
        [SerializeField] private float secondsPerGive = 2;

        protected override void Awake()
        {
            base.Awake();
            Work(destroyCancellationToken).Forget();
        }

        private async UniTask Work(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(secondsPerGive, cancellationToken: token);
                money += moneyPerGive;
            }
        }
    }

}