using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;

    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
