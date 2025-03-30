using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTile : MonoBehaviour
{
    public int tileIndex = -1; // Order in the sequence. Set in Inspector (e.g., 0, 1, 2, ...)
    public bool isCorrectTile = false; // Set to true only for green tiles

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TileSequenceManager.Instance.TileStepped(this);
        }
    }
}

