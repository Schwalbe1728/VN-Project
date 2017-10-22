using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributePanelScript : MonoBehaviour
{
    [SerializeField]
    private CharacterStat Attribute;

    [SerializeField]
    private Text AttributeName;

    [SerializeField]
    private Text AttributeValue;

    private CharacterInfoScript CharInfo;

    void Awake()
    {
        CharInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();
        OnValidate();
    }       

    void OnValidate()
    {
        if (AttributeName != null)
        {
            AttributeName.text
              = Attribute.ToString();
        }

        if (AttributeValue != null && CharInfo != null)
        {
            AttributeValue.text
              = CharInfo.GetStat(Attribute).ToString();
        }
    }
}
