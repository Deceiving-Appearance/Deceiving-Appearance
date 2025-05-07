using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIDHolder : MonoBehaviour
{
   public bool hasID = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IDCard"))
        {
            hasID = true;
            other.transform.SetParent(transform); 
            other.transform.localPosition = new Vector3(0.5f, 0f, 0.5f); // Offset 
            other.GetComponent<Collider>().enabled = false; // Prevent re-trigger
        }
    }
}
