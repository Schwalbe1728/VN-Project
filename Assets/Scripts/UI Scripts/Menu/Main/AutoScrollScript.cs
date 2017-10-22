using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollScript : MonoBehaviour
{
    private RectTransform ContentRectTransform;
    private Coroutine coroutine;

    [SerializeField]
    private float PixelPerSecond;

	// Use this for initialization
	void Awake ()
    {
        ContentRectTransform = gameObject.GetComponent<RectTransform>();
	}

    void OnEnable()
    {
        Debug.Log("OnEnabled");

        ContentRectTransform.localPosition = new Vector3(0, 0, 0);

        coroutine = StartCoroutine(ScrollDown());
    }

    void OnDisable()
    {
        Debug.Log("OnDisabled");
        StopCoroutine(coroutine);
    }

    private IEnumerator ScrollDown()
    {
        while (true)
        {
            ContentRectTransform.localPosition =
                ContentRectTransform.localPosition +
                new Vector3(0, PixelPerSecond * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }
    }
}
