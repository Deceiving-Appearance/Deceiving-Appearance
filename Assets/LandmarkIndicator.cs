using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkIndicator : MonoBehaviour
{
    public List<GameObject> landmarks;
    public GameObject mapCamera;

    // Update is called once per frame
    void Update()
    {
        LandmarkIconBillboard();
    }

    void LandmarkIconBillboard()
    {
        if (landmarks == null) return;
        
        // Vector3 targetPos = mapCamera.transform.position;

        // foreach (GameObject landmark in landmarks)
        // {
        //     targetPos.y = 0;

        //     landmark.transform.LookAt(targetPos);
        // }
    }
}
