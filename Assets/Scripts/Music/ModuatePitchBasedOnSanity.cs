using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuatePitchBasedOnSanity : MonoBehaviour
{
    [SerializeField]
    private float MaxPitchVariance;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float SanityThreshold;

    private float CurrentDeviation;
    private AudioSource Source;
    private CharacterInfoScript playerInfo;    

    void Awake()
    {
        Source = gameObject.GetComponent<AudioSource>();
        CurrentDeviation = 0.5f;

        StartCoroutine(SetCharacterInfoScript());

        //Debug.Log(PlayerPrefs.GetFloat("Last Session Sanity"));
    }
	
    void Update()
    {
        if(playerInfo != null)
        {
            if (playerInfo.SanityPercentage <= SanityThreshold)
            {
                ManipulatePitch(playerInfo.SanityPercentage);
            }
            else
            {
                CurrentDeviation = 0.5f;
                Source.pitch = 1;
            }
        }
        else
        {
            float temp = PlayerPrefs.GetFloat("Last Session Sanity", 1);

            if (temp <= SanityThreshold)
            {
                ManipulatePitch(temp);
            }
        }

    }

    private void ManipulatePitch(float sanity)
    {
        CurrentDeviation += Time.deltaTime / Speed;        

        float sanityMod = 1 - sanity / SanityThreshold;

        Source.pitch =
            1 -
            (MaxPitchVariance / 2) * sanityMod +
            //Mathf.PingPong(CurrentDeviation, 1) * MaxPitchVariance * sanityMod;
            (0.5f + 0.5f * Mathf.Sin(Mathf.Deg2Rad * 360 * CurrentDeviation)) * MaxPitchVariance * sanityMod;
    }

    private IEnumerator SetCharacterInfoScript()
    {
        GameObject gic;
        
        while( (gic = GameObject.Find("Game Info Component")) == null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        while(gic.GetComponent<CharacterInfoScript>() == null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        playerInfo = gic.GetComponent<CharacterInfoScript>();
    }
}
