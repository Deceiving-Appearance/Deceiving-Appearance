using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorControl : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(3f, 0, 0);
    public float openSpeed = 2f;
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 targetPosition;

    private AudioSource doorAudio;


    [Header("UI")]
    public TextMeshProUGUI lockedText;
    public float messageDuration = 2f;

    private float messageTimer = 0f;

    private void Start()
    {
        doorAudio = GetComponent<AudioSource>();

        closedPosition = transform.position;
        targetPosition = closedPosition;

        if (lockedText != null)
            lockedText.text = "";
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * openSpeed);

        if (lockedText != null && lockedText.text != "")
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
                lockedText.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerIDHolder>();
        if (player != null)
        {
            if (player.hasID)
            {
                OpenDoor();
            }
            else
            {
                ShowLockedMessage();
            }
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetPosition = closedPosition + openOffset;

            if (doorAudio != null && !doorAudio.isPlaying)
            {
                doorAudio.Play();
            }
            // Destroy the ID card if the player is carrying it
            var player = FindObjectOfType<PlayerIDHolder>();
            if (player != null && player.hasID)
            {
                Transform idCard = player.transform.Find("IDCard"); 
                if (idCard == null)
                    idCard = player.transform.Find("IDCard"); // Fallback for non-clone

                if (idCard != null)
                    Destroy(idCard.gameObject);

                player.hasID = false;
            }
        }
    }

    void ShowLockedMessage()
    {
        if (lockedText != null)
        {
            lockedText.text = "It's locked...";
            messageTimer = messageDuration;
        }
    }
}
