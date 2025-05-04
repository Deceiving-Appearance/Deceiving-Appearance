using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerAggressiveState : StalkerFSMState
{
    private readonly float moveSpeed = 6f;
    private readonly int radius = 20;
    private Transform currentTarget;

    public StalkerAggressiveState(Stalker stalker) : base(stalker)
    {
        _id = StalkerFSMStateType.AGGRESSIVE;
    }

    public override void Enter()
    {
        base.Enter();

        _stalker.pathController.EnableRotation(true);
        _stalker.pathController.SetMoveSpeed(moveSpeed);
        _stalker.pathController.OnTargetReachedEvent += OnTargetReached;

        // Choose a random player to target and attack
        var players = _stalker.playerDetector.GetPlayersWithinRadius(radius);
        if (players.Count > 0)
        {
            int randomIndex = Random.Range(0, players.Count);
            currentTarget = players[randomIndex].transform;
            _stalker.pathController.SetDestination(currentTarget.position);
        }
        else
        {
            _stalker.stalkerFSM.SetCurrentState(StalkerFSMStateType.SHY);
        }
    }

    public override void Update()
    {
        base.Update();

        var players = _stalker.playerDetector.GetPlayersWithinRadius(radius);
        if (players.Count == 0)
        {
            _stalker.stalkerFSM.SetCurrentState(StalkerFSMStateType.SHY);
            return;
        }

        if (currentTarget != null)
        {
            _stalker.pathController.SetDestination(currentTarget.position);

            float distance = Vector3.Distance(_stalker.transform.position, currentTarget.position);
            if (distance < 1.5f)
            {
                OnTouchPlayer();
            }
        }
    }

    private void OnTouchPlayer()
    {
        Debug.Log("Stalker touched the player!");

        Effects effects = GameObject.FindObjectOfType<Effects>();
        if (effects != null)
        {
            effects.OnPlayerHit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _stalker.pathController.OnTargetReachedEvent -= OnTargetReached;
        currentTarget = null;

        StalkerScannerReaction scannerReaction = _stalker.GetComponent<StalkerScannerReaction>();
        if (scannerReaction != null)
        {
            scannerReaction.FadeOutAggressiveSound();
        }
    }

    private void OnTargetReached()
    {
        if (currentTarget != null)
        {
            _stalker.pathController.SetDestination(currentTarget.position);
        }
    }
}
