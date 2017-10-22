using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributePanelScript : MonoBehaviour
{

    [SerializeField]
    private CharacterStat Attribute;

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
        if (creationScript.DecreaseStat(Attribute))
        {
            GameObject.Find("Points Left Value").GetComponent<Text>().text =
                creationScript.StatPointsLeft.ToString();
        }
    }

    public void IncreaseStat()
    {
        if (creationScript.IncreaseStat(Attribute))
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

