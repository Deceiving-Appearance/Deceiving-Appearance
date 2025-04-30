using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class StalkerController : MonoBehaviour
{
    public Transform player;
    public float fieldOfView = 60f;
    public float moveCooldown = 3f;
    public float minDistanceToPlayer = 5f;
    public float maxDistanceToPlayer = 15f;
    public LayerMask obstacleMask;
    public float wallCheckDistance = 2.0f;
    public int positionTries = 20;

    private NavMeshAgent agent;
    private float nextMoveTime;
    private bool isHiding;
    private Transform dynamicSpot;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Time.time < nextMoveTime)
            return;

        bool seen = IsPlayerLooking();

        if (seen)
        {
            agent.isStopped = true;
            isHiding = true;
        }
        else
        {
            if (isHiding || !agent.hasPath || agent.remainingDistance < 1f)
            {
                Transform spot = FindBestHidingSpot();
                if (spot != null)
                {
                    if (dynamicSpot != null) Destroy(dynamicSpot.gameObject); // Clean up previous
                    dynamicSpot = spot;

                    agent.SetDestination(spot.position);
                    agent.isStopped = false;
                    nextMoveTime = Time.time + moveCooldown + Random.Range(0.5f, 1.5f);
                    isHiding = false;
                }
            }
        }
    }

    bool IsPlayerLooking()
    {
        Vector3 dirToStalker = (transform.position - player.position).normalized;
        float angle = Vector3.Angle(player.forward, dirToStalker);

        if (angle < fieldOfView)
        {
            if (!Physics.Linecast(player.position + Vector3.up, transform.position + Vector3.up, obstacleMask))
                return true;
        }
        return false;
    }

    Transform FindBestHidingSpot()
    {
        List<Vector3> candidates = new List<Vector3>();
        float radius = Random.Range(minDistanceToPlayer, maxDistanceToPlayer);

        for (int i = 0; i < positionTries; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle.normalized * radius;
            Vector3 candidate = player.position + new Vector3(randCircle.x, 0, randCircle.y);

            if (NavMesh.SamplePosition(candidate, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                Vector3 toPlayer = (player.position - navHit.position).normalized;

                // Not visible?
                if (Physics.Linecast(player.position + Vector3.up, navHit.position + Vector3.up, obstacleMask))
                {
                    // Is it near a wall?
                    if (Physics.Raycast(navHit.position + Vector3.up * 0.5f, -toPlayer, wallCheckDistance, obstacleMask))
                    {
                        candidates.Add(navHit.position);
                    }
                }
            }
        }

        if (candidates.Count > 0)
        {
            Vector3 chosen = candidates[Random.Range(0, candidates.Count)];
            GameObject temp = new GameObject("DynamicSpot");
            temp.transform.position = chosen;
            return temp.transform;
        }

        return null;
    }
}
