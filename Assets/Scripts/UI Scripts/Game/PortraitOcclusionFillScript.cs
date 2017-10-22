using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitOcclusionFillScript : MonoBehaviour
{
    private Image OcclusionImage;
    private CharacterInfoScript PlayerInfo;

    void Awake()
    {
        OcclusionImage = gameObject.GetComponent<Image>();
        PlayerInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();

        PlayerInfo.RegisterToHealthChanged( UpdateByHealth );

        UpdateByHealth(PlayerInfo.HealthPercentage);
    }

    void Update()
    {        
        if(Input.GetKeyDown(KeyCode.U))
        {
            PlayerInfo.ChangeHealth(-1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerInfo.ChangeSanity(-1);
        }
    }

    private void UpdateByHealth(float percentage)
    {
        OcclusionImage.fillAmount = 1 - percentage;
    }
}
