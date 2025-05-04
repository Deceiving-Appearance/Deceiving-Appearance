using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StalkerPathController : MonoBehaviour
{
    [SerializeField] private StalkerAIPath aiPath;

    public Action OnTargetReachedEvent;

    private void Start()
    {
        aiPath.OnTargetReachedEvent += OnTargetReached;
    }

    private Vector3 PickRandomPoint(int radius)
    {
        var point = UnityEngine.Random.insideUnitSphere * radius;
        point.y = 0;
        point += aiPath.position;
        return point;
    }

    public void SetRandomDestination(int radius)
    {
        Vector3 randomDestination = PickRandomPoint(radius);
        SetDestination(randomDestination);
    }

    private void OnTargetReached()
    {
        OnTargetReachedEvent?.Invoke();
    }

    public void SetDestination(Vector3 target)
    {
        aiPath.destination = target;
        aiPath.SearchPath();
    }

    public void SetMoveSpeed(float speed)
    {
        aiPath.maxSpeed = speed;
    }

    public void EnableRotation(bool value)
    {
        aiPath.enableRotation = value;
    }

}
