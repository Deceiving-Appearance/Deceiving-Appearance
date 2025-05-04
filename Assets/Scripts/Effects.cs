using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class Effects : MonoBehaviour
{
    public static bool IsPlayerDead = false;

    public Material screenDamageMat;
    public TextMeshProUGUI deathMessageText;

    private Coroutine fadeCoroutine;
    private float currentRadius = 1f;
    private bool isDead = false;

    [Header("Settings")]
    public float radiusDecreasePerHit = 0.6f;
    public float fadeStepAmount = 0.5f;
    public float fadeDelay = 1f;
    public float damageCooldown = 1f;

    private float lastDamageTime = -999f;

    void Start()
    {
        currentRadius = 1f;
        screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

        if (deathMessageText != null)
            deathMessageText.enabled = false;

        IsPlayerDead = false;
    }

    void Update()
    {
        if (IsPlayerDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
            yield return new WaitForSeconds(fadeDelay);

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
        IsPlayerDead = true;

        currentRadius = -0.8f;
        screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

        if (deathMessageText != null)
        {
            deathMessageText.text = "You Died";
            deathMessageText.enabled = true;
        }

        PlayerInput input = FindObjectOfType<PlayerInput>();
        if (input != null) input.enabled = false;

        Debug.Log("Player died.");
    }
}
