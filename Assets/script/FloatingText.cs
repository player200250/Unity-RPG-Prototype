using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;

    public void Setup(string message, Color color)
    {
        text.text = message;
        text.color = color;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Color startColor = text.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = startPos + Vector3.up * floatSpeed * elapsed;
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - elapsed / fadeDuration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
