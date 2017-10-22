using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlRefireScript : MonoBehaviour
{
    private Animation animationControl;
    private Coroutine coroutine;
    private bool Started;

    [SerializeField]
    private float StartDelay;

    [SerializeField]
    private bool DelayRand;

    [SerializeField]
    private float DelayRandAmount;

    [SerializeField]
    private bool StartOnAwake;    

    public float Delay { get { return StartDelay; } set { StartDelay = value; } }

    public void Start()
    {
        Started = true;
    }

    public void Stop()
    {
        Started = false;
    }

	// Use this for initialization
	void Awake ()
    {
        animationControl = gameObject.GetComponent<Animation>();
        coroutine = null;

        if(StartOnAwake)
        {
            Start();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Started && (coroutine == null))
        {
            coroutine = StartCoroutine(FireAnimation());
        }
	}

    private IEnumerator FireAnimation()
    {
        float delayDuration = StartDelay;
        if (DelayRand) delayDuration *= UnityEngine.Random.Range(1 - DelayRandAmount, 1 + DelayRandAmount);

        yield return new WaitForSeconds(delayDuration);

        animationControl.Play();

        yield return new WaitForSeconds(animationControl.clip.length);

        coroutine = null;
    }
}
