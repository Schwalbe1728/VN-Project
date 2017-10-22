using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueTree;


public class ScrollViewContentScript : MonoBehaviour {

    [SerializeField]
    private int LoggedMessagesMax;

    [SerializeField]
    private float BetweenLogsSpacing;

    [SerializeField]
    private DialogueScript DialogueScr;
    private ConditionsAccumulator Conditions;

    [SerializeField]
    private GameObject DialogueLogElementPrefab;

    [SerializeField]
    private GameObject DialogueSeparatorPrefab;

    [SerializeField]
    private GameObject OptionButtonPrefab;

    [SerializeField]
    private GameObject ExitDialogueButton;

    private DialogueOption[] currentOptions;
    private List<GameObject> DialogueLog;

    private DialogueOption chosenOption;

    private CharacterInfoScript PlayerInfo;

    private float startHeight;

    public void SetDialogue(DialogueScript scr)
    {        
        //DialogueLog = new List<GameObject>();

        DialogueScr = scr;
        Conditions = scr.gameObject.GetComponent<ConditionsAccumulator>();
        //DialogueScr.RegisterToDialogueEndedEvent(SelectNextDialogue);
        DialogueScr.StartDialogue();                
    }

    public void OptionHasBeenChosen(int optInd)
    {
        bool dialogueFinished = !DialogueScr.NextNode(currentOptions[optInd]);        

        for(int i = 0; i <= currentOptions.Length; i++)
        {
            Destroy(DialogueLog[DialogueLog.Count - 1]);
            DialogueLog.RemoveAt(DialogueLog.Count - 1);   
        }

        //AddToLog("Player: " + currentOptions[optInd].Text, Color.white, true);
        AddToLog(currentOptions[optInd].Text, PlayerInfo.Name);


        currentOptions = null;
        
        if(dialogueFinished)
        {
            //ShowExitButton();
        }
        
        ShowDialogue();        
    }

    public bool WriteCurrentNode()
    {
        DialogueNode node = DialogueScr.CurrentNode;

        if (node == null || node.Text == null || node.Text.Equals(""))
        {            
            return false;
        }

        //AddToLog(DialogueScr.NPCDisplayedName + ": " + node.Text);
        AddToLog(node, DialogueScr.NPCDisplayedName);
        return true;
    }

    public bool WriteCurrentOptions()
    {        
        currentOptions = DialogueScr.CurrentOptions;

        if (currentOptions != null && currentOptions.Length > 0)
        {
            AddSeparator();

            for (int i = 0; i < currentOptions.Length; i++)
            {
                AddToLog(currentOptions[i], i + 1);                
            }
            return true;
        }

        return false;
    }

    public void AlignLogElements()
    {
        // If log contains too many elements, remove oldest messages
        if(LoggedMessagesMax > 0)
        {
            // +1 to account for separator between options and log
            int temp = LoggedMessagesMax + 1 + ((currentOptions != null) ? currentOptions.Length : 0);

            while(DialogueLog.Count > temp)
            {
                Destroy(DialogueLog[DialogueLog.Count - 1]);
                DialogueLog.RemoveAt(DialogueLog.Count - 1);
            }
        }

        Canvas.ForceUpdateCanvases();

        float sum = 0;

        float prevHeight = 0;

        int elInd = 0;

        foreach(GameObject logElement in DialogueLog)
        {            
            RectTransform rekt = logElement.GetComponent<RectTransform>();
            rekt.anchoredPosition = Vector2.zero;

            float spacing = (currentOptions == null || elInd >= currentOptions.Length + 1) ? BetweenLogsSpacing : BetweenLogsSpacing / 3;

            sum += spacing + prevHeight / 2 + rekt.rect.height / 2;
            prevHeight = rekt.rect.height;

            rekt.Translate(0, -sum, 0);

            elInd++;
        }

        RectTransform tempLogArea = gameObject.GetComponent<RectTransform>();
        tempLogArea.offsetMax = 
            new Vector2(tempLogArea.offsetMax.x, -Mathf.Min(startHeight - sum - prevHeight, 0) );        
    }

    void Awake()
    {        
        DialogueLog = new List<GameObject>();

        PlayerInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();

        if (DialogueScr != null)
        {
            StartCoroutine("StartDialogue");
        }
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        startHeight = gameObject.GetComponent<RectTransform>().rect.height;

        SetDialogue(DialogueScr);
        ShowDialogue();        
    }

    private void ShowDialogue()
    {   
        if(DialogueScr.CurrentNode != null)
        {
            bool ends = false;

            do
            {
                WriteCurrentNode();

                if (!WriteCurrentOptions())
                {
                    ends = !DialogueScr.NextNode();
                }
            }
            while (!ends && (currentOptions == null || currentOptions.Length == 0));

            if(ends)
            {
                
            }
        }

        AlignLogElements();
    }

    private void AddToLog(string text, string name = null)
    {
        GameObject dialogueLogObject = Instantiate(DialogueLogElementPrefab, this.transform);
        DialogueLogElementScript script = dialogueLogObject.GetComponent<DialogueLogElementScript>();

        if (name != null && !name.Equals(""))
        {
            script.SetCharName(name);
        }

        script.SetDialogueText(text);

        //DialogueLog.Insert(0, dialogueLogObject);
        DialogueLog.Add(dialogueLogObject);
    }

    private void AddToLog(DialogueNode node, string name = null)
    {
        GameObject dialogueLogObject = Instantiate(DialogueLogElementPrefab, this.transform);
        DialogueLogElementScript script = dialogueLogObject.GetComponent<DialogueLogElementScript>();

        if (name != null && !name.Equals(""))
        {
            script.SetCharName(name);
        }

        script.SetDialogueText(node.Text);

        //DialogueLog.Insert(0, dialogueLogObject);
        DialogueLog.Add(dialogueLogObject);
    }

    private void AddToLog(DialogueOption option, int number)
    {
        GameObject optionObject = Instantiate(OptionButtonPrefab, this.transform);
        OptionButtonScript script = optionObject.GetComponent<OptionButtonScript>();
        Condition cond = Conditions.GetOptionExit(option.OptionID);

        script.SetNumber(number);
        script.SetText(option.Text);        

        //script.SetTest("character", 95, 3);

        if (cond != null && !cond.ChanceToPass.Equals("null"))
        {
            script.SetTest(
                cond.AttributeDefinition,
                int.Parse(cond.ChanceToPass),
                cond.Modifier
            );
        }
        optionObject.name = "Option " + number.ToString();

        

        //DialogueLog.Insert(0, optionObject);
        DialogueLog.Add(optionObject);

    }

    private void AddSeparator()
    {
        GameObject separatorObject = Instantiate(DialogueSeparatorPrefab, this.transform);
        //DialogueLog.Insert(0, separatorObject);
        DialogueLog.Add(separatorObject);
    }

    /*
    private void AddToLog(string toLog)
    {
        GameObject dialogueLogObject = Instantiate(LogElementPrefab, this.transform);
        dialogueLogObject.GetComponent<Text>().text = toLog;
        DialogueLog.Insert(0, dialogueLogObject);
    }

    private void AddToLog(string toLog, Color col, bool bold = false)
    {
        GameObject dialogueLogObject = Instantiate(LogElementPrefab, this.transform);
        dialogueLogObject.GetComponent<Text>().text = toLog;
        dialogueLogObject.GetComponent<Text>().color = col;
        dialogueLogObject.GetComponent<Text>().fontStyle = (bold) ? FontStyle.Bold : FontStyle.Normal;
        DialogueLog.Insert(0, dialogueLogObject);
    }*/           
}
