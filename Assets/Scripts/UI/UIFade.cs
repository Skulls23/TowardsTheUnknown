using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        fadeImage.SetActive(false);
    }

    public void Fade(bool isFade)
    {
        StartCoroutine(FadeEnum(isFade));
    }
    
    public IEnumerator FadeEnum(bool isFade)
    {
        Color objectColor = fadeImage.GetComponent<Image>().color;
        float fadeAmount;
        if (isFade)
        {
            fadeImage.SetActive(true);
            while (fadeImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            fadeImage.SetActive(false);
        }
        else
        {
            while (fadeImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            fadeImage.SetActive(false);
        }
    }
}