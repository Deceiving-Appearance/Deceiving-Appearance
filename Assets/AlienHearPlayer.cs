using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienHearPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float hearingDistance = 10f;
    [SerializeField] private float growlCooldown = 5f;

    [SerializeField] private AudioSource growlAudioSource; 
    private float nextGrowlTime;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= hearingDistance && Time.time >= nextGrowlTime)
        {
            if (!growlAudioSource.isPlaying)
            {
                growlAudioSource.Play();
                nextGrowlTime = Time.time + growlCooldown;
            }
        }
    }
}
