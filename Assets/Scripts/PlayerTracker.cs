using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemy;
    [SerializeField] private PointRenderer pointRenderer;

    private void Update()
    {
        pointRenderer.SetReferencePosition(player.position);
        pointRenderer.SetReferenceEnemyPosition(enemy.position);
    }
}