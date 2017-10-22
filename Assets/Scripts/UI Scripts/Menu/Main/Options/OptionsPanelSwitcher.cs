using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelSwitcher : MonoBehaviour
{
    /*
    [SerializeField]
    private Button[] Buttons;
    */
    [SerializeField]
    private GameObject[] Panels;

    [SerializeField]
    private Text MenuTitleText;

    [SerializeField]
    private string[] MenuTitlesArray;

    private int CurrentChoice = 0;

    public void SwitchPanel(int n)
    {
        if(CurrentChoice >= 0 && CurrentChoice < Panels.Length)
        {
            Panels[CurrentChoice].SetActive(false);            
        }

        if (n >= 0 && n < Panels.Length)
        {
            CurrentChoice = n;
            Panels[n].SetActive(true);

            if(MenuTitleText != null)
            {
                MenuTitleText.text = MenuTitlesArray[CurrentChoice];
            }
        }                
    }

    void Awake()
    {
        for(int i = 0; i < Panels.Length; i++)
        {
            Panels[i].SetActive(false);
        }

        SwitchPanel(0);
    }
	
}
