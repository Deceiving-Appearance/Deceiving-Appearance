using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragmentTrigger : MonoBehaviour
{
    public Image fragmentPopupImage;      // UI Image shown on trigger
    public int fragmentID;                // ID for this fragment

    private bool hasBeenTriggered = false;  // Ensure trigger only runs once

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            fragmentPopupImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (hasBeenTriggered && fragmentPopupImage.gameObject.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            FragmentLogSystem.CollectFragment(fragmentID);
            fragmentPopupImage.gameObject.SetActive(false);
        }
    }
}
