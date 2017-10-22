using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeInFadeOut : MonoBehaviour
{
    public delegate void FadeFinished();

    public FadeFinished OnFadeFinished;

    private Image FadeOutPanel;
    private Color FadeOutColor;
    private float progress;

    [SerializeField]
    private Color DefaultColor;

    [SerializeField]
    private float fadeDuration;

    [SerializeField]
    private bool FadeInOnAwake;

    public bool FadeInProgress { get { return progress < 1; } }

    public void SetDefaultFadeOut()
    {
        FadeOutColor = DefaultColor;
    }

    public void SetFadeOutColor(Color col)
    {
        FadeOutColor = col;
    }

    public void StartFadeIn(float duration = 0)
    {
        if(!FadeInProgress)
        {
            StartCoroutine(FadeIn((duration > 0) ? duration : fadeDuration));
        }
    }

    public void StartFadeIn(Color color, float duration = 0)
    {
        SetFadeOutColor(color);

        if (!FadeInProgress)
        {
            StartCoroutine(FadeIn((duration > 0) ? duration : fadeDuration));
        }
    }

    public void StartFadeOut(float duration = 0)
    {
        if (!FadeInProgress)
        {
            StartCoroutine(FadeOut((duration > 0) ? duration : fadeDuration));
        }
    }

    public void StartFadeOut(Color color, float duration = 0)
    {
        SetFadeOutColor(color);

        if (!FadeInProgress)
        {
            StartCoroutine(FadeOut((duration > 0) ? duration : fadeDuration));
        }
    }

    public void StartFlash(float duration = 0)
    {
        if (!FadeInProgress)
        {
            StartCoroutine(Flash((duration > 0) ? duration : fadeDuration));
        }
    }

    public void StartFlash(Color color, float duration = 0)
    {
        SetFadeOutColor(color);

        if (!FadeInProgress)
        {
            StartCoroutine(Flash((duration > 0) ? duration : fadeDuration));
        }
    }

    void Awake()
    {
        OnFadeFinished += SetDefaultFadeOut;

        FadeOutPanel = gameObject.GetComponentInChildren<Image>();
        SetDefaultFadeOut();
        progress = 1;

        if(FadeInOnAwake)
        {
            StartFadeIn(fadeDuration);
        }
    }    

    private IEnumerator FadeOut(float duration, bool flash = false)
    {
        progress = 0;
        FadeOutPanel.raycastTarget = true;

        while(progress < 1)
        {
            progress += (duration > 0)? (Time.deltaTime / duration) : 1;
            FadeOutPanel.color =
                new Color(
                    FadeOutColor.r,
                    FadeOutColor.g,
                    FadeOutColor.b,
                    FadeOutColor.a * progress
                    );

            yield return new WaitForEndOfFrame();
        }

        if(!flash) InvokeOnFadeFinished();       
    }

    private IEnumerator FadeIn(float duration, bool flash = false)
    {
        progress = 0;
        FadeOutPanel.raycastTarget = true;

        while(progress < 1)
        {
            progress += (duration > 0) ? (Time.deltaTime / duration) : 1;
            FadeOutPanel.color =
                new Color(
                    FadeOutColor.r,
                    FadeOutColor.g,
                    FadeOutColor.b,
                    FadeOutColor.a * (1 - progress)
                    );

            yield return new WaitForEndOfFrame();
        }

        FadeOutPanel.raycastTarget = false;
        if(!flash) InvokeOnFadeFinished();
    }

    private IEnumerator Flash(float flashTime)
    {
        yield return StartCoroutine(FadeOut(flashTime / 2, true));
        yield return StartCoroutine(FadeIn(flashTime / 2, true));

        InvokeOnFadeFinished();
    }

    private void InvokeOnFadeFinished()
    {
        if(OnFadeFinished != null)
        {
            OnFadeFinished();
        }
    }    
}
