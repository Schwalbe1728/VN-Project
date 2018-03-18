using UnityEngine;

[System.Serializable]
public abstract class ConditionNodeBase
{
    public virtual bool ConditionTest()
    {
        return true;
    }
}

[System.Serializable]
public partial class ConditionNode : ConditionNodeBase
{
    [SerializeField]
    protected int conditionID;

    [SerializeField]
    protected int targetIDIfPassed = Dialogue.ExitDialogue;
    [SerializeField]
    protected NodeType targetTypeIfPassed = NodeType.Exit;

    [SerializeField]
    protected int targetIDIfFailed = Dialogue.ExitDialogue;
    [SerializeField]
    protected NodeType targetTypeIfFailed = NodeType.Exit;

    public int ConditionID { get { return conditionID; } set { if (value >= 0) conditionID = value; } }

    public int SuccessTarget { get { return targetIDIfPassed; } }
    public NodeType SuccessTargetType { get { return targetTypeIfPassed; } }

    public int FailureTarget { get { return targetIDIfFailed; } }
    public NodeType FailureTargetType { get { return targetTypeIfFailed; } }

    [SerializeField]
    //private string nextDialogueIDIfPassed;
    //public string NextDialogueIDIfPassed { get { return nextDialogueIDIfPassed; } set { nextDialogueIDIfPassed = value; } }
    private Dialogue nextDialogueIfPassed;
    public Dialogue NextDialogueIfPassed { get { return nextDialogueIfPassed; } set { nextDialogueIfPassed = value; } }

    [SerializeField]
    //private string nextDialogueIDIfFailed;
    //public string NextDialogueIDIfFailed { get { return nextDialogueIDIfFailed; } set { nextDialogueIDIfFailed = value; } }
    private Dialogue nextDialogueIfFailed;
    public Dialogue NextDialogueIfFailed { get { return nextDialogueIfFailed; } set { nextDialogueIfFailed = value; } }


    public bool ConditionChainElementTest(out int nextConditionID)
    {
        NodeType targType;
        bool result = ConditionTest(out nextConditionID, out targType);

        if (targType == NodeType.Exit)
        {

        }
        else
        {
            if (targType != NodeType.Condition)
            {
                throw new System.AccessViolationException("Illegal operation - only conditions are allowed");
            }
        }

        Debug.Log(result + ", " + nextConditionID);

        return result;
    }

    public bool ConditionTest(out int targetID, out NodeType targetType)
    {
        bool result = ConditionTest();

        targetID =
            (result) ?
                targetIDIfPassed : targetIDIfFailed;

        targetType =
            (result) ?
                targetTypeIfPassed : targetTypeIfFailed;

        return result;
    }

    public void SetSuccessTarget(int nID, NodeType nType)
    {
        targetIDIfPassed = nID;
        targetTypeIfPassed = nType;
    }

    public void SetFailureTarget(int nID, NodeType nType)
    {
        targetIDIfFailed = nID;
        targetTypeIfFailed = nType;
    }
}
