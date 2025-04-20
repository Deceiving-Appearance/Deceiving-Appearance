using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    public GameObject[] tools; // Assign your tool GameObjects in the inspector
    private int currentToolIndex = 0;

    void Start()
    {
        SelectTool();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentToolIndex++;
            if (currentToolIndex >= tools.Length)
                currentToolIndex = 0;
            SelectTool();
        }
        else if (scroll < 0f)
        {
            currentToolIndex--;
            if (currentToolIndex < 0)
                currentToolIndex = tools.Length - 1;
            SelectTool();
        }
    }

    void SelectTool()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(i == currentToolIndex);
        }
    }
}
