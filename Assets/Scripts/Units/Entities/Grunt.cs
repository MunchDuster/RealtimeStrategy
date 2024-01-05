using UnityEngine;
using UnityEngine.AI;

namespace Munchy.Units.Entities
{
    public class Grunt : Entity
    {
        

public class PlayerController : MonoBehaviour
    {
        public Transform target; // The target the player will navigate towards
        private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

        void Start()
        {
            // Get the NavMeshAgent component attached to this GameObject
            navMeshAgent = GetComponent<NavMeshAgent>();

            // Ensure the target is set
            if (target == null)
            {
                Debug.LogError("Target not set for PlayerController!");
            }
            else
            {
                // Set the destination of the NavMeshAgent to the target's position
                navMeshAgent.SetDestination(target.position);
            }
        }

        void Update()
        {
            // Check if the NavMeshAgent has reached the destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                // Destination reached, do something (e.g., play an animation, trigger an event, etc.)
                Debug.Log("Destination Reached!");
            }
        }
    }
}
}