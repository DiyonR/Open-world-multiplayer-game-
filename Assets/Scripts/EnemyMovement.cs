using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float detectionRange = 10f; // Range at which the enemy detects the player
    public float patrolRange = 20f; // Range within which the enemy selects patrol points
    public float recheckInterval = 2f; // Interval at which the enemy rechecks if the destination is reachable
    public Transform[] patrolPoints; // List of potential patrol points
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private float lastCheckTime; // Time of the last destination check

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Initialize the NavMeshAgent component
        lastCheckTime = Time.time; // Record the initial time
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
                lastCheckTime = Time.time; // Update the last check time
            }
            else if (Time.time - lastCheckTime >= recheckInterval)
            {
                // If the player is out of range for too long, select a new patrol point
                SelectPatrolPoint();
                lastCheckTime = Time.time; // Update the last check time
            }
        }
    }

    void SelectPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            // Select a random patrol point within the specified patrol range
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRange;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas);

            // Set the selected patrol point as the destination for the NavMeshAgent
            agent.SetDestination(hit.position);
        }
    }
}
