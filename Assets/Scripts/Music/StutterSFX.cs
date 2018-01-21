using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StutterSFX : MonoBehaviour {

    private AudioSource audioSource;
    private Coroutine coroutine;

    [SerializeField]
    private float BaseEffectDuration;

    [SerializeField]
    [Range(0,1)]
    private float EffectDurationVariance;

    [SerializeField]
    [Range(0,1)]
    private float OccuranceChance;

    [SerializeField]    
    private float DelayBetweenOccurances;

    // Use this for initialization
    void Awake ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(coroutine == null && Random.Range(0f, 1f) < OccuranceChance)
        {
            coroutine = StartCoroutine(StutterOccured());
        }
	}

    private IEnumerator StutterOccured()
    {
        /*
        yield return
            new WaitForSecondsRealtime(
                Random.Range(1 - EffectDurationVariance, 1 + EffectDurationVariance) * 
                BaseEffectDuration
                );
        */
        float newTime = audioSource.time - Random.Range(1 - EffectDurationVariance, 1 + EffectDurationVariance) *
                                            BaseEffectDuration;

        newTime = (newTime + audioSource.clip.length) %  audioSource.clip.length;
        if (newTime < 0) Debug.LogWarning("Mniejsze od zera!!!");
        audioSource.time = newTime;

        yield return new WaitForSecondsRealtime(DelayBetweenOccurances);
        coroutine = null;        
    }
}
