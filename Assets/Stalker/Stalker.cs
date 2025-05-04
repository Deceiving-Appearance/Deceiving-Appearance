using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StalkerPathController))]
[RequireComponent(typeof(PlayerDetector))]
public class Stalker : MonoBehaviour
{
    public StalkerFSM stalkerFSM;
    public StalkerPathController pathController;
    public PlayerDetector playerDetector;
    public AudioSource stalkerAudioSource;


    private void Awake()
    {
        pathController = GetComponent<StalkerPathController>();
        playerDetector = GetComponent<PlayerDetector>();
    }
    private void Start()
    {
        stalkerFSM = new();

        stalkerFSM.Add(new StalkerPatrollingState(this));
        stalkerFSM.Add(new StalkerInterestedState(this));
        stalkerFSM.Add(new StalkerShyState(this));
        stalkerFSM.Add(new StalkerAggressiveState(this));

        stalkerFSM.SetCurrentState(StalkerFSMStateType.PATROLLING);

    }

    private void Update()
    {
        stalkerFSM.Update();
    }

    private void FixedUpdate()
    {
        stalkerFSM.FixedUpdate();
    }
}
