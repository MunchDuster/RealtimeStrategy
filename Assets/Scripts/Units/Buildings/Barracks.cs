using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Munchy.Units.Entities;

namespace Munchy.Units.Buildings
{
    public class Barracks : Building
    {
        [Header("Barracks")]
        [SerializeField] private int maxQueued = 5;
        [SerializeField] private Transform[] spawnPoints;

        public List<Entity> queue = new();

        private bool _queueRunning;
        private int lastSpawnIndex;

        public bool TryAddToQueue(Entity entity)
        {
            if (!entity.CanSpawn())
                return false;

            if (queue.Count >= maxQueued)
            {
                Communication.Call(new ErrorMessage("Too many items in queue."));
                return false;
            }

            queue.Add(entity);
            money -= entity.cost; //Apply cost now
            Communication.Call(new LocalUnitQueuedMessage());

            if (!_queueRunning)
                RunQueue(destroyCancellationToken).Forget();

            return true;
        }

        private async UniTask RunQueue(CancellationToken token)
        {
            Debug.Log("Running barracks queue");
            _queueRunning = true;

            while (queue.Count > 0 && !token.IsCancellationRequested)
            {
                Transform spawnPoint = spawnPoints[lastSpawnIndex];
                PhotonNetwork.Instantiate(queue[0].gameObject.name, spawnPoint.position, spawnPoint.rotation);
                
                lastSpawnIndex = lastSpawnIndex >= spawnPoints.Length ? 0 : spawnPoints.Length + 1;

                await UniTask.WaitForSeconds(queue[0].queueTime, cancellationToken: token);
                queue.RemoveAt(0);
            }
            _queueRunning = false;
        }
    }
}
