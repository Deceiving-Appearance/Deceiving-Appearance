using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject[] cameras;
    public GameObject[] grenades;

    public Image slot1Highlight, slot2Highlight, slot3Highlight;

    private int currentTool = 1; // 1: flashlight, 2: camera, 3: grenade
    private int cameraIndex = 0;
    private int grenadeIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToFlashlight();
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCamera();
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToGrenade();

        HighlightSlot();
    }

    void SwitchToFlashlight()
    {
        currentTool = 1;
        flashlight.SetActive(true);
        foreach (var cam in cameras) cam.SetActive(false);
        foreach (var g in grenades) g.SetActive(false);
    }

    void SwitchToCamera()
    {
        currentTool = 2;
        flashlight.SetActive(false);
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].SetActive(i == cameraIndex);

        foreach (var g in grenades) g.SetActive(false);
    }

    void SwitchToGrenade()
    {
        currentTool = 3;
        flashlight.SetActive(false);
        foreach (var cam in cameras) cam.SetActive(false);
        for (int i = 0; i < grenades.Length; i++)
            grenades[i].SetActive(i == grenadeIndex);
    }

    void HighlightSlot()
    {
        slot1Highlight.enabled = currentTool == 1;
        slot2Highlight.enabled = currentTool == 2;
        slot3Highlight.enabled = currentTool == 3;
    }
}
