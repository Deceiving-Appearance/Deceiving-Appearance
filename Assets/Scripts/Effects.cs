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
    public float radiusDecreasePerHit = 1f;
    public float fadeStepAmount = 0.05f;     // How much to heal per step
    public float fadeDelay = 9f;             // Wait time between fade steps
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


// v4 works too
// using System.Collections;
// using UnityEngine;
// using TMPro;

// public class Effects : MonoBehaviour
// {
//     public Material screenDamageMat;
//     public TextMeshProUGUI deathMessageText;

//     private Coroutine fadeCoroutine;
//     private float currentRadius = 1f; // Start with no redness
//     private bool isDead = false;

//     [Header("Settings")]
//     public float radiusDecreasePerHit = 1f;       // Each hit reduces radius
//     public float fadeSpeed = 0.0000000001f;            // Controls how fast it fades (lower = slower)
//     public float fadeDelay = 999999999f;                 // Delay before fading starts
//     public float damageCooldown = 1f;             // Cooldown between valid hits

//     private float lastDamageTime = -999f;

//     void Start()
//     {
//         currentRadius = 1f;
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

//         if (deathMessageText != null)
//             deathMessageText.enabled = false;
//     }

//     public void OnPlayerHit()
//     {
//         if (isDead) return;

//         // Cooldown between hits
//         if (Time.time - lastDamageTime < damageCooldown)
//             return;

//         lastDamageTime = Time.time;

//         // Apply redness
//         currentRadius -= radiusDecreasePerHit;
//         currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//         Debug.Log("Current Radius: " + currentRadius);

//         // Restart fade coroutine
//         if (fadeCoroutine != null)
//             StopCoroutine(fadeCoroutine);

//         fadeCoroutine = StartCoroutine(FadeBackToSafe());

//         // Trigger death
//         if (currentRadius <= -1f)
//         {
//             Die();
//         }
//     }

//     private IEnumerator FadeBackToSafe()
//     {
//         float fadeStartTime = Time.time + fadeDelay;

//         // Wait before fade begins
//         while (Time.time < fadeStartTime && !isDead)
//         {
//             yield return null;
//         }

//         Debug.Log("Fade started at: " + Time.time);

//         // Slowly fade back toward safe state
//         while (currentRadius < 1f && !isDead)
//         {
//             currentRadius += Time.deltaTime * fadeSpeed;
//             currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//             screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//             yield return null;
//         }

//         fadeCoroutine = null;
//     }

//     private void Die()
//     {
//         isDead = true;
//         currentRadius = -1f;
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

//         if (deathMessageText != null)
//         {
//             deathMessageText.text = "You Died";
//             deathMessageText.enabled = true;
//         }

//         Debug.Log("Player died.");
//     }
// }



// v3 fading too fast
// using System.Collections;
// using UnityEngine;
// using TMPro;

// public class Effects : MonoBehaviour
// {
//     public Material screenDamageMat;
//     public TextMeshProUGUI deathMessageText;

//     private Coroutine fadeCoroutine;
//     private float currentRadius = 1f; // Start with no redness
//     private bool isDead = false;

//     [Header("Settings")]
//     public float radiusDecreasePerHit = 1f;   // Each hit reduces radius
//     public float fadeSpeed = 0.00001f;             // Controls how fast it fades (higher = faster)
//     public float fadeDelay = 99f;                // Delay before fading starts
//     public float damageCooldown = 1f;           // Cooldown between hits

//     private float lastDamageTime = -999f;
//     private bool isFading = false;

//     void Start()
//     {
//         currentRadius = 1f;
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

//         if (deathMessageText != null)
//             deathMessageText.enabled = false;
//     }

//     public void OnPlayerHit()
//     {
//         if (isDead) return;

//         // Damage cooldown
//         if (Time.time - lastDamageTime < damageCooldown)
//             return;

//         lastDamageTime = Time.time;

//         // Decrease redness
//         currentRadius -= radiusDecreasePerHit;
//         currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//         Debug.Log("Current Radius: " + currentRadius);

//         // Start fade coroutine only once
//         if (!isFading)
//         {
//             fadeCoroutine = StartCoroutine(FadeBackToSafe());
//         }

//         // Trigger death
//         if (currentRadius <= -1f)
//         {
//             Die();
//         }
//     }

//     private IEnumerator FadeBackToSafe()
//     {
//         isFading = true;

//         Debug.Log("Fade will begin in " + fadeDelay + " seconds...");
//         yield return new WaitForSeconds(fadeDelay);

//         Debug.Log("Fade started.");

//         while (currentRadius < 1f && !isDead)
//         {
//             currentRadius += Time.deltaTime * fadeSpeed;
//             currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//             screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//             yield return null;
//         }

//         isFading = false;
//         fadeCoroutine = null;
//     }

//     private void Die()
//     {
//         isDead = true;
//         currentRadius = -1f;
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);

//         if (deathMessageText != null)
//         {
//             deathMessageText.text = "You Died";
//             deathMessageText.enabled = true;
//         }

//         Debug.Log("Player died.");
//     }
// }



// v2 - seems good

// using System.Collections;
// using UnityEngine;
// using TMPro;

// public class Effects : MonoBehaviour
// {
//     public Material screenDamageMat;
//     public TextMeshProUGUI deathMessageText;

//     private Coroutine fadeCoroutine;
//     private float currentRadius = 1f; // Start with no redness
//     private bool isDead = false;

//     [Header("Settings")]
//     public float radiusDecreasePerHit = 0.9f;
//     public float fadeSpeed = 0.000001f;
//     public float fadeDelay = 999f;
//     private float lastDamageTime = -999f;
//     public float damageCooldown = 1f; // seconds between valid hits


//     void Start()
//     {
//         currentRadius = 1f;
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//         if (deathMessageText != null)
//             deathMessageText.enabled = false;
//     }

//     public void OnPlayerHit()
//     {
//         if (isDead) return;

//         if (Time.time - lastDamageTime < damageCooldown)
//             return; // Too soon, ignore hit

//         lastDamageTime = Time.time;

//         currentRadius -= radiusDecreasePerHit;
//         currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//         screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//         Debug.Log("Current Radius: " + currentRadius);

//         if (fadeCoroutine != null)
//             StopCoroutine(fadeCoroutine);
//         fadeCoroutine = StartCoroutine(FadeBackToSafe());

//         if (currentRadius <= -1f)
//         {
//             Die();
//         }
//     }

//     private IEnumerator FadeBackToSafe()
//     {
//         yield return new WaitForSeconds(fadeDelay);

//         while (currentRadius < 1f && !isDead)
//         {
//             currentRadius += Time.deltaTime * fadeSpeed;
//             currentRadius = Mathf.Clamp(currentRadius, -1f, 1f);
//             screenDamageMat.SetFloat("_Vignette_radius", currentRadius);
//             yield return null;
//         }
//     }


//     private void Die()
//     {
//         isDead = true;
//         screenDamageMat.SetFloat("_Vignette_radius", -1f);
//         if (deathMessageText != null)
//         {
//             deathMessageText.text = "You Died";
//             deathMessageText.enabled = true;
//         }
//         Debug.Log("Player died.");
//     }
// }




// v1
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Effects : MonoBehaviour
// {
//     public Material screenDamageMat;
//     private Coroutine screenDamageTask;

//     private void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.T))
//             ScreenDamageEffect(Random.Range(0.1f, 1));
//     }

//     public void ScreenDamageEffect(float intensity)
//     {
//         if(screenDamageTask != null)
//             StopCoroutine(screenDamageTask);
//         screenDamageTask = StartCoroutine(screenDamage(intensity));
//     }

//     private IEnumerator screenDamage(float intensity)
//     {
//         var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
//         float curRadius = 1f;
//         for(float t = 0; curRadius != targetRadius; t += Time.deltaTime)
//         {
//             curRadius = Mathf.Lerp(1, targetRadius, t);
//             screenDamageMat.SetFloat("_Vignette_radius", curRadius);
//             yield return null;
//         }
//         for(float t = 0; curRadius < 1; t += Time.deltaTime)
//         {
//             curRadius = Mathf.Lerp(targetRadius, 1, t);
//             screenDamageMat.SetFloat("_Vignette_radius", curRadius);
//             yield return null;
//         }
//     }

//     private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
//     {
//         return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
//     }
// }
