using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [TextArea(3, 10)] public string fullText;
    public float letterDelay = 0.06f;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        typingCoroutine = StartCoroutine(RevealText());
    }

    void Update()
    {
        if (isTyping && Input.GetMouseButtonDown(0)) // Left click
        {
            SkipToFullText();
        }
    }

    IEnumerator RevealText()
    {
        isTyping = true;
        textDisplay.text = "";

        foreach (char letter in fullText)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
    }

    void SkipToFullText()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textDisplay.text = fullText;
        isTyping = false;
    }
}
