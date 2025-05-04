using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerInterestedState : StalkerFSMState
{
    private readonly int patrolRadius = 2;
    private readonly int playerDetectionRadius = 10;
    private readonly int moveSpeed = 1;
    private Transform currentTarget;

    public StalkerInterestedState(Stalker stalker) : base(stalker)
    {
        _id = StalkerFSMStateType.INTERESTED;
    }

    public override void Enter()
    {
        base.Enter();
        _stalker.pathController.EnableRotation(false);
        _stalker.pathController.SetMoveSpeed(moveSpeed);
        _stalker.pathController.SetRandomDestination(patrolRadius);
        _stalker.pathController.OnTargetReachedEvent += OnTargetReached;

        // Choose a random player as target
        PlayerManager target = ChooseRandomPlayerAsTarget();
        if (target != null)
        {
            currentTarget = target.transform;
        }
    }

    public override void Update()
    {
        base.Update();

        if (currentTarget != null)
        {
            _stalker.transform.LookAt(currentTarget); // Intimidate by looking
        }

        // If no players nearby, exit this state and start patrolling
        var players = _stalker.playerDetector.GetPlayersWithinRadius(playerDetectionRadius);
        if (players.Count == 0)
        {
            _stalker.stalkerFSM.SetCurrentState(StalkerFSMStateType.PATROLLING);
        }

        // Scan reactions now handled in StalkerScannerReaction.cs
    }

    public override void Exit()
    {
        base.Exit();
        _stalker.pathController.OnTargetReachedEvent -= OnTargetReached;
        currentTarget = null;
    }

    private void OnTargetReached()
    {
        _stalker.pathController.SetRandomDestination(patrolRadius);
    }

    private PlayerManager ChooseRandomPlayerAsTarget()
    {
        var players = _stalker.playerDetector.GetPlayersWithinRadius(playerDetectionRadius);
        if (players.Count == 0) return null;
        int randomIndex = Random.Range(0, players.Count);
        return players[randomIndex];
    }
}
