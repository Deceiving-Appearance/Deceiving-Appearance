using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WandererAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Aggro / Hearing
    public float baseAggroRange = 10f;
    public float aggroIncreasePerClick = 2f;
    float currentAggroRange;
    int clickCount = 0;
    bool playerMadeNoise = false;

    // Pause after touching player
    public float pauseDuration = 3f;
    private bool isPaused = false;
    private bool waitForClickToResume = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentAggroRange = baseAggroRange;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (waitForClickToResume)
            {
                waitForClickToResume = false;
                agent.isStopped = false;
                playerMadeNoise = true;
                Debug.Log("Mouse clicked: Alien can move again.");
            }
            else if (!isPaused)
            {
                clickCount++;
                currentAggroRange = baseAggroRange + (clickCount * aggroIncreasePerClick);
                playerMadeNoise = true;
            }
        }

        if (isPaused || waitForClickToResume)
        {
            return; // Don't move while paused or waiting
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= currentAggroRange && playerMadeNoise)
        {
            if (distance > agent.stoppingDistance)
            {
                ChasePlayer();
            }
        }
        else
        {
            Patroling();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPaused && !waitForClickToResume)
        {
            StartCoroutine(PauseAfterTouch());
        }
    }

    private void ChasePlayer()
    {
        if (!agent.isStopped)
        {
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator PauseAfterTouch()
    {
        isPaused = true;
        waitForClickToResume = false;
        agent.isStopped = true;
        Debug.Log("Alien collided with player. Pausing...");

        yield return new WaitForSeconds(pauseDuration);

        isPaused = false;
        waitForClickToResume = true;
        playerMadeNoise = false;
        clickCount = 0;
        currentAggroRange = baseAggroRange;

        Debug.Log("Alien waiting for click to resume.");
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet && !agent.isStopped)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
}
