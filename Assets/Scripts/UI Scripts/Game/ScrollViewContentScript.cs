using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollViewContentScript : MonoBehaviour {

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

        DialogueShown = scr;        
        //DialogueScr.RegisterToDialogueEndedEvent(SelectNextDialogue);
        DialogueShown.StartDialogue();                
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

        if (DialogueShown != null)
        {
            StartCoroutine("StartDialogue");
        }
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        startHeight = gameObject.GetComponent<RectTransform>().rect.height;

        SetDialogue(DialogueShown);       
    }

    private void AddSeparator()
    {
        GameObject separatorObject = Instantiate(DialogueSeparatorPrefab, this.transform);
        //DialogueLog.Insert(0, separatorObject);
        DialogueLog.Add(separatorObject);
    }              
}
