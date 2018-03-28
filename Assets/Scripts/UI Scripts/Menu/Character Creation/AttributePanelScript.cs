using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributePanelScript : MonoBehaviour
{

    [SerializeField]
    private CharacterAttribute Attribute;

    private Text AttributeValueText;
    private CharacterCreationScript creationScript;

    public void UpdateDisplayedValue()
    {
        AttributeValueText.text = creationScript.StatValue(Attribute).ToString();

        GameObject.Find("Points Left Value").GetComponent<Text>().text =
                creationScript.StatPointsLeft.ToString();
    }

    public void DecreseStat()
    {
        bool temp;

        if (creationScript.DecreaseStat(Attribute, out temp))
        {
            GameObject.Find("Points Left Value").GetComponent<Text>().text =
                creationScript.StatPointsLeft.ToString();
        }
    }

    public void IncreaseStat()
    {
        bool temp;

        if (creationScript.IncreaseStat(Attribute, out temp))
        {
            GameObject.Find("Points Left Value").GetComponent<Text>().text =
                creationScript.StatPointsLeft.ToString();            
        }
    }

    // Use this for initialization
    void Start()
    {
        AttributeValueText = transform.Find("Value").GetComponent<Text>();
        creationScript = GameObject.Find("Game Info Component").GetComponent<CharacterCreationScript>();

        if (creationScript != null)
        {
            creationScript.RegisterToNotifyStatChange(UpdateDisplayedValue);
        }

        UpdateDisplayedValue();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

