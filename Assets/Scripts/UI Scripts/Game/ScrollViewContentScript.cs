using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ScrollViewContentScript : MonoBehaviour {

    [SerializeField]
    private Dialogue DialogueShown;
    private DialogueOption[] currentOptions;

    [SerializeField]
    private int LoggedMessagesMax;

    [SerializeField]
    private float BetweenLogsSpacing;

    [SerializeField]
    private GameObject DialogueLogElementPrefab;

    [SerializeField]
    private GameObject DialogueSeparatorPrefab;

    [SerializeField]
    private GameObject OptionButtonPrefab;

    [SerializeField]
    private GameObject ExitDialogueButton;

    private List<GameObject> DialogueLog;

    private CharacterInfoScript PlayerInfo;

    private float startHeight;    

    public void SetDialogue(Dialogue scr)
    {
        //DialogueLog = new List<GameObject>();
        Debug.Log("Set Dialogue");
        DialogueShown = scr;

        if (DialogueShown.StartDialogue())
        {
            DrawNode();
        }
    }

    public void OptionHasBeenChosen(DialogueOption optionChosen)
    {
        for(int i = 0; i < (currentOptions != null? currentOptions.Length : 0) + 2; i++)
        {
            RemoveLogElement(1);
        }

        if (optionChosen.OptionID != int.MaxValue)
        {
            GameObject optionText = Instantiate(DialogueLogElementPrefab, this.transform);
            optionText.GetComponent<DialogueLogElementScript>().SetCharName("Player:");
            optionText.GetComponent<DialogueLogElementScript>().SetDialogueText(optionChosen.OptionText);

            DialogueLog.Insert(0, optionText);
            if (DialogueShown.Next(optionChosen))
            {
                DrawNode();
            }
        }
        else
        {
            if(DialogueShown.Next())
            {
                DrawNode();
            }
        }
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
                RemoveLogElement(DialogueLog.Count - 1);
            }
        }

        Canvas.ForceUpdateCanvases();               
        
        for(int i = 0; i < DialogueLog.Count; i++)
        {
            DialogueLog[i].transform.SetSiblingIndex(i);
        }
    }

    void Awake()
    {        
        DialogueLog = new List<GameObject>();

        PlayerInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();

        Dialogue.RegisterToOnDialogueEnded(onDialogueEnded);

        if (DialogueShown != null)
        {
            StartCoroutine(StartDialogue());
        }
    }

    void onDialogueEnded(Dialogue next)
    {
        this.StopAllCoroutines();
        DialogueShown = next;
        SetDialogue(DialogueShown);
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.25f);

        gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        startHeight = gameObject.GetComponent<RectTransform>().rect.height;

        SetDialogue(DialogueShown);       
    }

    private void DrawNode()
    {
        AddCurrentNodeText();

        if (!DialogueShown.CurrentNode.ImmediateNode)
        {
            AddOptionsFromCurrentNode();
            AddSeparator();
            AlignLogElements();
        }
        else
        {
            if(DialogueShown.CurrentNode.AddContinue)
            {
                AddContinueButton();
                AddSeparator();
                AlignLogElements();
            }
        }        
    }

    private void AddContinueButton(int indexer = 0)
    {
        int nxt = DialogueShown.CurrentNode.TargetID;
        NodeType nxtType = DialogueShown.CurrentNode.TargetType;

        DialogueOption continueOption = new DialogueOption();
        continueOption.OptionText = "[Continue]";
        continueOption.OptionID = int.MaxValue;
        continueOption.SetNext(nxt, nxtType);

        GameObject prefabCreated = Instantiate(OptionButtonPrefab, this.transform);
        OptionButtonScript obs = prefabCreated.GetComponent<OptionButtonScript>();

        obs.SetNumber(++indexer);        

        obs.SetText(continueOption.OptionText);
        obs.SetOption(continueOption);        

        DialogueLog.Insert(indexer, prefabCreated);

        AddSeparator(indexer + 1);
    }

    private void AddOptionsFromCurrentNode()
    {
        currentOptions = DialogueShown.CurrentNodeOptions;
        int indexer = 0;

        List<GameObject> options = new List<GameObject>();

        foreach(DialogueOption option in currentOptions)
        {
            GameObject prefabCreated = Instantiate(OptionButtonPrefab, this.transform);
            OptionButtonScript obs = prefabCreated.GetComponent<OptionButtonScript>();

            obs.SetNumber(++indexer);

            if(option.NextType == NodeType.Condition)
            {
                // opisać testy
            }

            obs.SetText(option.OptionText);
            obs.SetOption(option);
            options.Add(prefabCreated);

            DialogueLog.Insert(indexer, prefabCreated);
        }

        AddSeparator(indexer + 1);
    }

    private void AddCurrentNodeText()
    {
        GameObject prefabCreated = Instantiate(DialogueLogElementPrefab, this.transform);
        DialogueLogElementScript dles = prefabCreated.GetComponent<DialogueLogElementScript>();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(DialogueShown.CurrentNode.NodeText);

        while(DialogueShown.CurrentNode.ImmediateNode && !DialogueShown.CurrentNode.AddContinue)
        {
            if (DialogueShown.Next() && !DialogueShown.CurrentNode.NodeText.Equals(""))
            {
                sb.AppendLine(DialogueShown.CurrentNode.NodeText);
            }
        }

        dles.SetDialogueText(sb.ToString());
        DialogueLog.Insert(0, prefabCreated);
    }

    private void AddSeparator(int atIndex = 1)
    {
        GameObject separatorObject = Instantiate(DialogueSeparatorPrefab, this.transform);
        DialogueLog.Insert(atIndex, separatorObject);
    }           
    
    private void RemoveLogElement(int index)
    {
        Destroy(DialogueLog[index], 0f);
        DialogueLog.RemoveAt(index);
    }   
}
