using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFader : MonoBehaviour
{
    public AnimationCurve curve;

    private CanvasGroup canvasGroup;

    public bool stopFading;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowUp()
    {
        stopFading = true;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        canvasGroup.alpha = 1.0f;
    }

    public void Hide()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        stopFading = false;
        float t = 1f;

        while (!stopFading && t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            canvasGroup.alpha = a;

            yield return 0;
        }

        if (!stopFading)
        {
            gameObject.SetActive(false);
        }
    }
}
