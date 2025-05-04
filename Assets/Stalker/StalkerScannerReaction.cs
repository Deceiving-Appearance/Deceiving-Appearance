using System.Collections;
using UnityEngine;
public class StalkerScannerReaction : MonoBehaviour
{
    private int scanCount = 0;
    private float scanResetTime = 3f;
    private float lastScanTime;
    private float lastReactionTime = -999f;

    [SerializeField] private float reactionCooldown = 2f;

    [Header("Aggressive If Not Seen")]
    [SerializeField] private float proximityThreshold = 7f;
    [SerializeField] private float unseenDuration = 2f;

    private float timeUnseen = 0f;
    private Camera playerCamera;
    private Transform player;
    private StalkerFSM stalkerFSM;

    [Header("Audio")]
    [SerializeField] private AudioSource aggressiveAudioSource;

    private void Start()
    {
        stalkerFSM = GetComponent<Stalker>().stalkerFSM;
        var playerManager = FindObjectOfType<PlayerManager>();
        playerCamera = playerManager.playerCamera;
        player = playerManager.transform;
    }

    public void RegisterScanHit()
    {
        if (Time.time - lastReactionTime < reactionCooldown) return;

        scanCount++;
        lastScanTime = Time.time;

        if (scanCount >= 3)
        {
            BecomeAggressive();
        }
        else if (scanCount >= 1)
        {
            stalkerFSM.SetCurrentState(StalkerFSMStateType.SHY);
            ResetScanState();
        }
    }

    private void Update()
    {
        if (Time.time - lastScanTime > scanResetTime)
        {
            scanCount = 0;
        }

        if (player == null || playerCamera == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= proximityThreshold && !IsVisibleToCamera(playerCamera))
        {
            timeUnseen += Time.deltaTime;

            if (timeUnseen >= unseenDuration)
            {
                BecomeAggressive();
            }
        }
        else
        {
            timeUnseen = 0f;
        }
    }

    private void BecomeAggressive()
    {
        if (!aggressiveAudioSource.isPlaying)
        {
            aggressiveAudioSource.loop = true;
            aggressiveAudioSource.Play();
        }

        stalkerFSM.SetCurrentState(StalkerFSMStateType.AGGRESSIVE);
        ResetScanState();
    }
    public void FadeOutAggressiveSound(float fadeDuration = 1.5f)
    {
        if (aggressiveAudioSource != null && aggressiveAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutCoroutine(aggressiveAudioSource, fadeDuration));
        }
    }

    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume for future use
    }



    private void ResetScanState()
    {
        scanCount = 0;
        lastReactionTime = Time.time;
        timeUnseen = 0f;
    }

    private bool IsVisibleToCamera(Camera cam)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(transform.position);
        return viewportPoint.z > 0 &&
               viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1;
    }
}
