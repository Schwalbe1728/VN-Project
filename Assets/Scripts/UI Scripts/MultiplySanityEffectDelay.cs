using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplySanityEffectDelay : MonoBehaviour
{
    [SerializeField]
    private MultiplierScale ScaleType;

    [SerializeField]
    private float MaxMultiplier;

    private float BaseDelayTime;
    private AnimationControlRefireScript RefireScript;

    private Coroutine coroutine;

	// Use this for initialization
	void Awake ()
    {
        RefireScript = gameObject.GetComponent<AnimationControlRefireScript>();
        BaseDelayTime = RefireScript.Delay;

        Update();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(coroutine == null)
        {
            coroutine = StartCoroutine(UpdateMultipliedTime());
        }
	}

    private IEnumerator UpdateMultipliedTime()
    {
        switch(ScaleType)
        {
            case MultiplierScale.Logarithmic:
                RefireScript.Delay = BaseDelayTime * Mathf.Pow(MaxMultiplier, PlayerPrefs.GetFloat("Last Session Sanity", 1));
                break;
        }

        yield return new WaitForSeconds(1f);

        coroutine = null;
    }
}

public enum MultiplierScale
{
    Logarithmic
}
