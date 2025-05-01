using System.Collections;
using UnityEngine;
using TMPro;

public class Effects : MonoBehaviour
{
    public Material screenDamageMat;
    public TextMeshProUGUI deathMessageText;

    private Coroutine fadeCoroutine;
    private float currentRadius = 1f; // Start with no redness
    private bool isDead = false;

    [Header("Settings")]
    public float radiusDecreasePerHit = 0.6f;
    public float fadeStepAmount = 0.5f;     // How much to heal per step
    public float fadeDelay = 1f;             // Wait time between fade steps
    public float damageCooldown = 1f;

    private float lastDamageTime = -999f;

    void Start()
    {
        currentRadius = 1f;
        screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

        if (deathMessageText != null)
            deathMessageText.enabled = false;
    }

    public void OnPlayerHit()
    {
        if (isDead) return;

        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        currentRadius -= radiusDecreasePerHit;
        currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
        screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
        Debug.Log("Current Radius: " + currentRadius);

        // Restart fade loop from the beginning
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeBackToSafeRepeated());

        if (currentRadius <= -0.8f)
        {
            Die();
        }
    }

    private IEnumerator FadeBackToSafeRepeated()
    {
        while (currentRadius < 1f && !isDead)
        {
            // Wait before each fade step
            yield return new WaitForSeconds(fadeDelay);

            // Then apply fade step
            currentRadius += fadeStepAmount;
            currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
            screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

            Debug.Log("Faded up to: " + currentRadius);
        }

        fadeCoroutine = null;
    }

    private void Die()
    {
        isDead = true;
        currentRadius = -0.8f;
        screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

        if (deathMessageText != null)
        {
            deathMessageText.text = "You Died";
            deathMessageText.enabled = true;
        }

        Debug.Log("Player died.");
    }
}
