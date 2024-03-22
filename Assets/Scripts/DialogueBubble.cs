using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    private SpriteRenderer sr;
    private TextMeshPro text;
    private RectTransform textRectTransform;

    private float typeSpeed = 0.08f;

    private Coroutine displayTextCoroutine;
    string message = string.Empty;

    void Awake()
    {
       sr = transform.Find("bg").GetComponent<SpriteRenderer>();
       text = transform.Find("text").GetComponent<TextMeshPro>();
       textRectTransform = text.GetComponent<RectTransform>();
    }

    private void Start()
    {
        text.text = "";
        message = " Um...you shouldn't have finished this fast. We are still constructing the next levels...";
        Vector3 textPosition = text.transform.position;
        sr.transform.position = new Vector3(textPosition.x, textPosition.y, sr.transform.position.z);
        SetSpriteRendererTransparent();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            startDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetDialogue();
        }
    }

    private void startDialogue()
    {
        if (displayTextCoroutine == null)
        {
            displayTextCoroutine = StartCoroutine(DisplayText(message));
        }
    }

    private void ResetDialogue()
    {
        StopCoroutine(displayTextCoroutine);
        displayTextCoroutine = null;
        text.text = "";
        SetSpriteRendererTransparent();
    }

    private IEnumerator DisplayText(string line)
    {
        text.text = "";
        SetSpriteRendererOpaque();

        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            Vector2 textSize = text.GetPreferredValues();
            textRectTransform.sizeDelta = new Vector2(textSize.x, textSize.y);
            sr.size = textRectTransform.sizeDelta;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private void SetSpriteRendererTransparent()
    {
        Color color = sr.color;
        color.a = 0f; 
        sr.color = color;
    }

    private void SetSpriteRendererOpaque()
    {
        Color color = sr.color;
        color.a = 1f; 
        sr.color = color;
    }
}
