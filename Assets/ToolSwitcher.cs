using System.Collections;
using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    public GameObject[] tools; // flashlight, magnifying glass, camera holder
    private int currentToolIndex = 0;

    [Header("Camera Placement")]
    public GameObject cameraPrefab; // Prefab of the placeable camera
    public int availableCameras = 3; // Start with 3
    public Transform cameraSpawnPoint; // Where the camera will appear (e.g., in front of player)

    void Start()
    {
        SelectTool();
    }

    void Update()
    {
        HandleScrollInput();
        HandleKeyInput();

        if (currentToolIndex == 2 && Input.GetKeyDown(KeyCode.E)) // Assuming tool 2 is camera
        {
            PlaceCamera();
        }
    }

    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentToolIndex = (currentToolIndex + 1) % tools.Length;
            SelectTool();
        }
        else if (scroll < 0f)
        {
            currentToolIndex = (currentToolIndex - 1 + tools.Length) % tools.Length;
            SelectTool();
        }
    }

    void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentToolIndex = 0; SelectTool(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) && tools.Length > 1) { currentToolIndex = 1; SelectTool(); }
        if (Input.GetKeyDown(KeyCode.Alpha3) && tools.Length > 2) { currentToolIndex = 2; SelectTool(); }
    }

    void SelectTool()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i == currentToolIndex);
        }
    }

    void PlaceCamera()
    {
        if (availableCameras <= 0) return;

        // You can raycast or spawn in front of player
        Instantiate(cameraPrefab, cameraSpawnPoint.position, cameraSpawnPoint.rotation);
        availableCameras--;

        Debug.Log("Camera placed! Remaining: " + availableCameras);
    }
}

// v2 - adding keys for tool switcher
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ToolSwitcher : MonoBehaviour
// {
//     public GameObject[] tools; // Assign in Inspector: 0 = flashlight, 1 = camera, 2 = magnifying glass
//     private int currentToolIndex = 0;

//     void Start()
//     {
//         SelectTool();
//     }

//     void Update()
//     {
//         HandleScrollInput();
//         HandleKeyInput();
//     }

//     void HandleScrollInput()
//     {
//         float scroll = Input.GetAxis("Mouse ScrollWheel");

//         if (scroll > 0f)
//         {
//             currentToolIndex = (currentToolIndex + 1) % tools.Length;
//             SelectTool();
//         }
//         else if (scroll < 0f)
//         {
//             currentToolIndex = (currentToolIndex - 1 + tools.Length) % tools.Length;
//             SelectTool();
//         }
//     }

//     void HandleKeyInput()
//     {
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             currentToolIndex = 0;
//             SelectTool();
//         }
//         else if (Input.GetKeyDown(KeyCode.Alpha2) && tools.Length > 1)
//         {
//             currentToolIndex = 1;
//             SelectTool();
//         }
//         else if (Input.GetKeyDown(KeyCode.Alpha3) && tools.Length > 2)
//         {
//             currentToolIndex = 2;
//             SelectTool();
//         }
//     }

//     void SelectTool()
//     {
//         for (int i = 0; i < tools.Length; i++)
//         {
//             tools[i].SetActive(i == currentToolIndex);
//         }
//     }
// }

// v1 - original
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ToolSwitcher : MonoBehaviour
// {
//     public GameObject[] tools; // Assign your tool GameObjects in the inspector
//     private int currentToolIndex = 0;

//     void Start()
//     {
//         SelectTool();
//     }

//     void Update()
//     {
//         float scroll = Input.GetAxis("Mouse ScrollWheel");

//         if (scroll > 0f)
//         {
//             currentToolIndex++;
//             if (currentToolIndex >= tools.Length)
//                 currentToolIndex = 0;
//             SelectTool();
//         }
//         else if (scroll < 0f)
//         {
//             currentToolIndex--;
//             if (currentToolIndex < 0)
//                 currentToolIndex = tools.Length - 1;
//             SelectTool();
//         }
//     }

//     void SelectTool()
//     {
//         for (int i = 0; i < tools.Length; i++)
//         {
//             tools[i].SetActive(i == currentToolIndex);
//         }
//     }
// }
