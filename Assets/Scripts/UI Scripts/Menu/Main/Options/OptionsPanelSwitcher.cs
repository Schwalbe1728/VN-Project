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
    private Button[] Switches;

    [SerializeField]
    private Text MenuTitleText;

    [SerializeField]
    private string[] MenuTitlesArray;

    [SerializeField]
    private Color SelectedTabColor;

    [SerializeField]
    private Color InactiveTabColor;

    private int CurrentChoice = 0;

    public void SwitchPanel(int n)
    {        
        if (n >= 0 && n < Panels.Length)
        {
            if (CurrentChoice >= 0 && CurrentChoice < Panels.Length)
            {
                Panels[CurrentChoice].SetActive(false);
            }

            CurrentChoice = n;
            Panels[n].SetActive(true);

            if(MenuTitleText != null)
            {
                MenuTitleText.text = MenuTitlesArray[CurrentChoice];
            }

            for(int i = 0; i < Switches.Length; i++)
            {
                ColorBlock temp = Switches[i].colors;
                    
                    temp.normalColor =
                    (i == n) ?
                        SelectedTabColor :
                        InactiveTabColor;

                Switches[i].colors = temp;
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
