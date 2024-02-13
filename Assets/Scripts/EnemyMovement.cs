using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float detectionRange = 10f; // Range at which the enemy detects the player
    public float roamRange = 30f; // Range within which the enemy roams
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Vector3 roamingDestination; // Current roaming destination

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Initialize the NavMeshAgent component
        SetRandomRoamingDestination();
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate distance between enemy and player
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // Check if player is within detection range
            if (distanceToPlayer <= detectionRange)
            {
                // Set destination for the NavMeshAgent to the player's position
                agent.SetDestination(target.position);
            }
            else
            {
                // If the player is not within detection range, roam around
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    SetRandomRoamingDestination();
                }
            }
        }
    }

    void SetRandomRoamingDestination()
    {
        // Select a random destination within the defined roam range
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * roamRange;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, roamRange, NavMesh.AllAreas);

        // Set the selected destination as the target for the NavMeshAgent
        agent.SetDestination(hit.position);
    }
}
