using System.Collections;
using System;
using UnityEngine;
using Pathfinding;

public class StalkerAIPath : AIPath
{
    public Action OnTargetReachedEvent;

    public override void OnTargetReached()
    {
        base.OnTargetReached();
        OnTargetReachedEvent?.Invoke();
    }
}
