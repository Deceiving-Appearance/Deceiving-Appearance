using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragmentLogSystem : MonoBehaviour
{
    public static bool[] collectedFragments = new bool[10];

    public GameObject logPanel;
    public Image[] fragmentLogImages;       // Array of images for logs
    public Image invalidAccessImage;

    private bool isLogOpen = false;

    void Start()
    {
        collectedFragments[0] = true;  // Auto-unlock mission log
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLogOpen = !isLogOpen;
            logPanel.SetActive(isLogOpen);

            if (isLogOpen)
            {
                ShowLog(0); // Automatically show mission log
            }
            else
            {
                HideAllLogs(); // Hide logs when log panel is closed
            }
        }

        if (isLogOpen)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    ShowLog(i);
                }
            }
        }
    }


    public static void CollectFragment(int id)
    {
        if (id == 0) return;
        if (id > 0 && id < collectedFragments.Length)
        {
            collectedFragments[id] = true;
        }
    }

    void ShowLog(int id)
    {
        HideAllLogs();

        if (collectedFragments[id])
        {
            fragmentLogImages[id].gameObject.SetActive(true);
        }
        else
        {
            invalidAccessImage.gameObject.SetActive(true);
        }
    }

    void HideAllLogs()
    {
        foreach (var img in fragmentLogImages)
            img.gameObject.SetActive(false);

        invalidAccessImage.gameObject.SetActive(false);
    }
}
