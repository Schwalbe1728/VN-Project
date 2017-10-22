using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCodedButtonHighlight : MonoBehaviour
{
    [SerializeField]
    private CharacterStat Stat;

    private Button tempButton;
    private Button abbrButton;
    private Button valButton;

    void Awake()
    {
        if(gameObject.GetComponent<Button>() == null)
        {
            AwakeOnPanel();
        }
        else
        {
            AwakeOnButton();
        }        
    }

    private void AwakeOnButton()
    {
        tempButton = gameObject.GetComponent<Button>();        

        ColorCodedAttributes attributes = GameObject.Find("Settings").GetComponent<ColorCodedAttributes>();

        ColorBlock temp = tempButton.colors;

        temp.highlightedColor = attributes.GetColor(Stat);
        tempButton.colors = temp;
    }

    private void AwakeOnPanel()
    {
        Button abbrButton = transform.Find("Abbr Button").gameObject.GetComponent<Button>();
        Button valButton = transform.Find("Value Button").gameObject.GetComponent<Button>();

        ColorCodedAttributes attributes = GameObject.Find("Settings").GetComponent<ColorCodedAttributes>();

        ColorBlock temp = valButton.colors;

        temp.highlightedColor = attributes.GetColor(Stat);
        valButton.colors = temp;

        temp = abbrButton.colors;

        temp.highlightedColor = attributes.GetColor(Stat);
        abbrButton.colors = temp;

        //abbrButton.on
    }
}
