using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerPatrollingState : StalkerFSMState
{
    private readonly int patrolRadius = 20;
    private readonly float moveSpeed = 6f;
    private readonly int playerDetectionRadius = 10;
    public StalkerPatrollingState(Stalker stalker): base(stalker)
    {
        _id = StalkerFSMStateType.PATROLLING;
    }

    public override void Enter()
    {
        base.Enter();
        _stalker.pathController.EnableRotation(true);
        _stalker.pathController.SetMoveSpeed(moveSpeed);
        _stalker.pathController.SetRandomDestination(patrolRadius);
        _stalker.pathController.OnTargetReachedEvent += OnTargetReached;

    }

    public override void Update()
    {
        base.Update();
        //If player is nearby, change the state to INTERESTED
        if(_stalker.playerDetector.GetPlayersWithinRadius(playerDetectionRadius).Count > 0)
        {
            _stalker.stalkerFSM.SetCurrentState(StalkerFSMStateType.INTERESTED);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _stalker.pathController.OnTargetReachedEvent -= OnTargetReached;
    }
    private void OnTargetReached()
    {
        _stalker.pathController.SetRandomDestination(patrolRadius);
    }
}
