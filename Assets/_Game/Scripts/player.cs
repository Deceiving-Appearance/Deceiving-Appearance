using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public healthBar healthBarScript;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarScript.SetHealth(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(10f);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarScript.SetHealth(currentHealth); 
    }
}
