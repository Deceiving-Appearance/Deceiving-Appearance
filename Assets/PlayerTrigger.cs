using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public Effects effectsScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Red")) // or "Alien" or whatever tag your enemy uses
        {
            effectsScript.OnPlayerHit();
        }
    }
}
