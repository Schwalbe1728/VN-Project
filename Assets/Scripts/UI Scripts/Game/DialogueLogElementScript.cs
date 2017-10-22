using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLogElementScript : MonoBehaviour {

    [SerializeField]
    private Text TestResultText;

    [SerializeField]
    private Text CharNameText;

    [SerializeField]
    private Text DialogueElementText;

    public void SetTestResult(string value)
    {
        TestResultText.gameObject.SetActive(true);
        TestResultText.text = value;
    }

    public void SetCharName(string name)
    {
        CharNameText.gameObject.SetActive(true);
        CharNameText.text = name;
    }

    public void SetDialogueText(string value)
    {
        DialogueElementText.text = value;
    }
}
