using System.Collections.Generic;
using UnityEngine;

public class TileSequenceManager : MonoBehaviour
{
    public static TileSequenceManager Instance;

    public List<int> correctSequence = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    private int currentStepIndex = 0;

    public AudioSource correctAudioSource;
    public AudioSource wrongAudioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void TileStepped(StepTile tile)
    {
        if (!tile.isCorrectTile || tile.tileIndex != correctSequence[currentStepIndex])
        {
            // Wrong step
            PlaySound(wrongAudioSource, tile.transform.position);
            currentStepIndex = 0;
            return;
        }

        // Correct step
        PlaySound(correctAudioSource, tile.transform.position);
        currentStepIndex++;

        if (currentStepIndex >= correctSequence.Count)
        {
            Debug.Log("Puzzle Complete!");
            // TODO: Unlock reward
        }
    }

    private void PlaySound(AudioSource source, Vector3 position)
    {
        if (source != null)
        {
            AudioSource temp = Instantiate(source, position, Quaternion.identity);
            temp.Play();
            Destroy(temp.gameObject, temp.clip.length);
        }
    }
}
