using UnityEngine;

[System.Serializable]
public class DialogueOption : DialogueElement
{
    [SerializeField]
    private int optionID;
    [SerializeField]
    private int nextID = Dialogue.ExitDialogue;
    [SerializeField]
    private NodeType nextType = NodeType.Exit;

    [SerializeField]
    private bool alreadyVisited = false;
    [SerializeField]
    private ConditionNode entryCondition;
    [SerializeField]
    private bool entryConditionSet = false;

    public int OptionID { get { return optionID; } set { if (value >= 0) optionID = value; } }

    [HideInInspector]
    public string OptionText;

    [HideInInspector]
    public bool VisitOnce;

    [HideInInspector]
    public int NextID { get { return nextID; } }
    public NodeType NextType { get { return nextType; } }

    [SerializeField]
    //private string nextDialogueID;
    //public string NextDialogueID { get { return nextDialogueID; } set { nextDialogueID = value; } }
    private Dialogue nextDialogue;
    public Dialogue NextDialogue { get { return nextDialogue; } set { nextDialogue = value; } }

    public bool CanDisplay
    {
        get
        {
            bool result = !EntryConditionSet || Dialogue.ConditionChainTest(entryCondition.ConditionID);

            return
                (!VisitOnce || !alreadyVisited) && result;
        }
    }

    public void Visit()
    {
        alreadyVisited = true;
        //return OptionText;
    }

    public ConditionNode EntryCondition { get { return entryCondition; } }

    public bool EntryConditionSet { get { return entryConditionSet; } }

    public void SetEntryCondition(ConditionNode condition)
    {
        entryCondition = condition;
        entryConditionSet = entryCondition != null;
    }

    public void ClearEntryCondition() { entryCondition = null; entryConditionSet = false; }

    public void SetNextNodeExit()
    {
        nextID = Dialogue.ExitDialogue;
        nextType = NodeType.Exit;
    }

    public void SetNext(int nID, NodeType type)
    {
        if (nID >= 0 && type != NodeType.Exit && type != NodeType.Option)
        {
            nextID = nID;
            nextType = type;
        }
    }
}
