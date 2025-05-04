using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerShyState : StalkerFSMState
{
    private readonly int radius = 20;
    private readonly float moveSpeed = 7f;

    public StalkerShyState(Stalker stalker) : base(stalker)
    {
        _id = StalkerFSMStateType.SHY;
    }

    public override void Enter()
    {
        base.Enter();
        _stalker.pathController.EnableRotation(true);
        _stalker.pathController.SetMoveSpeed(moveSpeed);
        _stalker.pathController.SetFurthestDestination(radius);
        _stalker.pathController.OnTargetReachedEvent += OnTargetReached;
    }

    public override void Exit()
    {
        base.Exit();
        _stalker.pathController.OnTargetReachedEvent -= OnTargetReached;
    }

    private void OnTargetReached()
    {
        // After fleeing, resume patrolling
        _stalker.stalkerFSM.SetCurrentState(StalkerFSMStateType.PATROLLING);
    }
}
