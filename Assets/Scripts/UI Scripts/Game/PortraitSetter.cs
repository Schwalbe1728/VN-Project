using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitSetter : MonoBehaviour {

	void Awake()
    {
        CharacterInfoScript player = GameObject.Find("Game Info Component").
                                        GetComponent<CharacterInfoScript>();

        Image portrait = gameObject.GetComponent<Image>();
        portrait.sprite = player.CharacterPortrait;

        Transform temp = transform.Find("Damage Occlusion");

        if (temp != null)
        {
            Image portraitOcclusion = temp.GetComponent<Image>();
            portraitOcclusion.sprite = player.CharacterPortrait;
        }        
    }
}
