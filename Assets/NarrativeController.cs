using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    public GameObject openingNarrative;
    public GameObject nextToFinalButton;
    public GameObject finalNarrative;
    public GameObject continueToGameButton;

    void Start()
    {
        openingNarrative.SetActive(true);
        nextToFinalButton.SetActive(true);
        finalNarrative.SetActive(false);
        continueToGameButton.SetActive(false);
    }

    public void ShowFinalNarrative()
    {
        openingNarrative.SetActive(false);
        nextToFinalButton.SetActive(false);
        finalNarrative.SetActive(true);
        continueToGameButton.SetActive(true);
    }

    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
