using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkManager : MonoBehaviour
{
    public List<Landmark> landmarks;
    public GameObject mapCamera;
    public Transform playerPos;
    public GameObject playerCanvas;

    [Header("Map UI Settings")]
    public bool useEaseIn = true;
    public float duration = 1f;
    public AnimationCurve easeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve easeOutCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // Update is called once per frame
    void Update()
    {
        LandmarkDistance();
    }

    void LandmarkDistance()
    {
        if (landmarks == null) return;

        foreach (Landmark landmark in landmarks)
        {
            landmark.distance = (int)Vector3.Distance(landmark.transform.position, playerPos.position);
        }
    }
}
