using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public Material screenDamageMat;
    private Coroutine screenDamageTask;

    private void Start()
    {
        screenDamageMat.SetFloat("_Vignette_radius", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            float damageIntensity = Random.Range(0.1f, 1f); // You can adjust this
            ScreenDamageEffect(damageIntensity);
        }
    }

    void ScreenDamageEffect(float intensity)
    {
        if (screenDamageTask != null)
            StopCoroutine(screenDamageTask);
        screenDamageTask = StartCoroutine(screenDamage(intensity));
    }

    private IEnumerator screenDamage(float intensity)
    {
        var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.15f);
        float curRadius = 1f;
        for (float t = 0; curRadius != targetRadius; t += Time.deltaTime)
        {
            curRadius = Mathf.Lerp(1, targetRadius, t);
            screenDamageMat.SetFloat("_Vignette_radius", curRadius);
            yield return null;
        }

        for (float t = 0; curRadius < 1f; t += Time.deltaTime)
        {
            curRadius = Mathf.Lerp(targetRadius, 1f, t);
            screenDamageMat.SetFloat("_Vignette_radius", curRadius);
            yield return null;
        }
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
}


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

//     void ScreenDamageEffect(float intensity)
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
