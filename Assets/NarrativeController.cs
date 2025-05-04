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
        continueToGameButton.SetActive(true);

        // Play final narration voice
        if (audioSource != null && finalNarrationClip != null)
        {
            audioSource.Stop();
            audioSource.clip = finalNarrationClip;
            audioSource.Play();
        }
    }

    public void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}