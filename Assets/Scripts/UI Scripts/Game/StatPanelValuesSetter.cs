using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelValuesSetter : MonoBehaviour
{
    [SerializeField]
    private Text SPText;

    [SerializeField]
    private Text HPText;

    private CharacterInfoScript CharInfo;

    void Awake()
    {
        CharInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();
        CharInfo.RegisterToHealthChanged(UpdateHealthText);
        CharInfo.RegisterToHealthChanged(UpdateStressText);
        CharInfo.RegisterToSanityChanged(UpdateStressText);

        UpdateTexts();
    }

    private void UpdateHealthText(float percent)
    {
        HPText.text = CharInfo.CurrentHealth.ToString();        
    }

    private void UpdateStressText(float percent)
    {
        SPText.text = CharInfo.StressPenalty.ToString();
    }

    private void UpdateTexts()
    {
        UpdateHealthText(0);
        UpdateStressText(0);
    }
}
