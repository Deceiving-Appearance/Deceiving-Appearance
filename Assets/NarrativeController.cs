using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    [Header("Narrative UI")]
    public GameObject openingNarrative;
    public GameObject nextToFinalButton;
    public GameObject finalNarrative;
    public GameObject nextToControlsButton;
    public GameObject controls;
    public GameObject continueToGameButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openingNarrationClip;
    public AudioClip finalNarrationClip;

    void Start()
    {
        openingNarrative.SetActive(true);
        nextToFinalButton.SetActive(true);
        finalNarrative.SetActive(false);
        nextToControlsButton.SetActive(false);
        controls.SetActive(false);
        continueToGameButton.SetActive(false);

        // Play opening narration voice
        if (audioSource != null && openingNarrationClip != null)
        {
            audioSource.clip = openingNarrationClip;
            audioSource.Play();
        }
    }

    public void ShowFinalNarrative()
    {
        openingNarrative.SetActive(false);
        nextToFinalButton.SetActive(false);
        finalNarrative.SetActive(true);
        nextToControlsButton.SetActive(true);
        controls.SetActive(false);
        continueToGameButton.SetActive(false);

        // Play final narration voice
        if (audioSource != null && finalNarrationClip != null)
        {
            audioSource.Stop();
            audioSource.clip = finalNarrationClip;
            audioSource.Play();
        }
    }

    public void ShowControls()
    {
        openingNarrative.SetActive(false);
        nextToFinalButton.SetActive(false);
        finalNarrative.SetActive(false);
        continueToGameButton.SetActive(false);
        controls.SetActive(true);
        continueToGameButton.SetActive(true);
        audioSource.Stop();
    }

    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}