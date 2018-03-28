using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OptionButtonScript : MonoBehaviour {

    [SerializeField]
    private GameObject TestPanel;

    [SerializeField]
    private Text NumberText;

    [SerializeField]
    private Text TestDefinitionText;

    [SerializeField]
    private Text TestPercentageText;

    [SerializeField]
    private Text OptionText;
        
    private string KeyID;
    private ScrollViewContentScript LogArea;

    private DialogueOption Option;

    public void OnClick()
    {
        LogArea.OptionHasBeenChosen(Option);
    }

    public void SetOption(DialogueOption option)
    {
        Option = option;
    }

    public void SetNumber(int n)
    {
        NumberText.text = n.ToString() + ":";
        KeyID = "Option " + n.ToString();
    }

    public void SetTest(CharacterAttribute definition, int percentage, int modifier = 0)
    {
        TestPanel.SetActive(true);

        StringBuilder sb = new StringBuilder();
        sb.Append(definition);
        if(modifier != 0)
        {
            sb.Append(" ");
            if (modifier > 0) sb.Append("+");
            sb.Append(modifier);
        }

        TestDefinitionText.text = sb.ToString();
        TestPercentageText.text = "(" + percentage.ToString() + "%)";

        Color testColor = 
            GameObject.Find("Settings").
                        GetComponent<ColorCodedAttributes>().
                        GetColor(definition);

        TestDefinitionText.color = testColor;
        TestPercentageText.color = testColor;
    }

    public void SetText(string text)
    {
        OptionText.text = text;
    }

    void Awake()
    {
        LogArea = GameObject.Find("Content").GetComponent<ScrollViewContentScript>();
    }
    
    void FixedUpdate()
    {
        if(VNInputManager.Instantiated && VNInputManager.Instance.GetKeyDown(KeyID))
        {
            OnClick();
        }
    } 
}
