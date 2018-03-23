using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DialogueReachedEnd(Dialogue next);

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public const int ExitDialogue = -1;

    private static Dialogue currentDialogue;
    private static event DialogueReachedEnd OnDialogueEnded;

    public static void RegisterToOnDialogueEnded(DialogueReachedEnd onDialogueEnded)
    {
        OnDialogueEnded -= onDialogueEnded;
        OnDialogueEnded += onDialogueEnded;
    }

    private static void FireOnDialogueEnded(Dialogue arg)
    {
        if (OnDialogueEnded != null)
        {
            Debug.Log("Fire On Dialogue Ended");
            OnDialogueEnded(arg);
        }
    }

    public string Name { get { return this.name; } }

    [SerializeField]
    private int startID = 0;

    [SerializeField]
    private NodeType startType;

    [SerializeField]
    private int currentNodeID;

    [SerializeField]
    private List<DialogueOption> Options;
    [SerializeField]
    private List<DialogueNode> Nodes;
    [SerializeField]
    private List<ConditionNode> Conditions;

    [SerializeField]
    private DialogueEditorInfo editorInfo;

    public DialogueEditorInfo EditorInfo { get { return editorInfo; } set { editorInfo = value; } }

    public int StartPointId { get { return startID; } }
    public NodeType StartPointType { get { return startType; } }
    public void SetStartPoint(int sID, NodeType sType)
    {
        if (sType == NodeType.Node || sType == NodeType.Condition)
        {
            startID = sID;
            startType = sType;
        }
    }
    public bool IsStartPoint(int sID, NodeType sType)
    {
        return StartPointId == sID && StartPointType == sType;
    }

    #region Options
    public DialogueOption[] GetAllOptions()
    {
        if (Options == null)
        {
            Options = new List<DialogueOption>();
        }

        return Options.ToArray();
    }

    public void SetAllOptions(DialogueOption[] opts)
    {
        Options = new List<DialogueOption>(opts);
    }

    public void SetAllOptions(List<DialogueOption> opts)
    {
        Options = opts;
    }

    public DialogueOption GetOption(int id)
    {
        return
            (id >= 0 && id < Options.Count) ?
                Options[id] : null;
    }

    public void DeleteOption(int nr)
    {
        if (Options != null && Options.Count < nr && Options.Count >= 0)
        {
            Options.RemoveAt(nr);

            for (int i = nr; i < Options.Count; i++)
            {
                Options[i].OptionID = i;
            }
        }
    }
    #endregion
    #region Nodes
    public DialogueNode[] GetAllNodes()
    {
        if (Nodes == null)
        {
            Nodes = new List<DialogueNode>();
        }

        return Nodes.ToArray();
    }

    public void SetAllNodes(DialogueNode[] nods)
    {
        Nodes = new List<DialogueNode>(nods);
    }

    public void SetAllNodes(List<DialogueNode> nods)
    {
        if (nods.Count == 0) Debug.Log("nods == 0!!!");

        Nodes = nods;
    }

    public DialogueNode GetNode(int id)
    {
        return
            (id >= 0 && id < Nodes.Count) ?
                Nodes[id] : null;
    }

    public void DeleteNode(int nr)
    {
        if (Nodes != null && Nodes.Count < nr && Nodes.Count >= 0)
        {
            Nodes.RemoveAt(nr);

            for (int i = nr; i < Nodes.Count; i++)
            {
                Nodes[i].NodeID = i;
            }
        }
    }
    #endregion
    #region Conditions
    public ConditionNode[] GetAllConditions()
    {
        if (Conditions == null)
        {
            Conditions = new List<ConditionNode>();
        }

        return Conditions.ToArray();
    }

    public void SetAllConditions(ConditionNode[] conds)
    {
        SetAllConditions(new List<ConditionNode>(conds));
    }

    public void SetAllConditions(List<ConditionNode> conds)
    {
        Conditions = conds;
    }
    public ConditionNode GetCondition(int id)
    {
        return
            (id >= 0 && id < Conditions.Count) ?
                Conditions[id] : null;
    }

    public static bool ConditionChainTest(int id)
    {
        bool result;

        do
        {
            result = currentDialogue.Conditions[id].ConditionChainElementTest(out id);
        }
        while (id != Dialogue.ExitDialogue);

        return result;
    }

    public bool ConditionChainValidation(int startId)
    {        
        NodeType targetFailureType = Conditions[startId].FailureTargetType;
        NodeType targetSuccessType = Conditions[startId].SuccessTargetType;

        bool resultSuccess =
            targetSuccessType == NodeType.Exit ||
            (targetSuccessType == NodeType.Condition && 
                ConditionChainValidation(Conditions[startId].SuccessTarget) )
            ;
        bool resultFailure = 
            targetFailureType == NodeType.Exit ||
            (targetFailureType == NodeType.Condition &&
                ConditionChainValidation(Conditions[startId].FailureTarget))
            ;

        return resultSuccess && resultFailure;
    }

    public void DeleteCondition(int nr)
    {
        if (Conditions != null && Conditions.Count < nr && Conditions.Count >= 0)
        {
            Conditions.RemoveAt(nr);

            for (int i = nr; i < Conditions.Count; i++)
            {
                Conditions[i].ConditionID = i;
            }
        }
    }
    #endregion

    // TODO: Dodać obiekt kontroli dialogów, który będzie mieć event OnDialogueEnded(string nextDialogue)
    public bool StartDialogue()
    {
        int tempID = startID;
        NodeType dummy;

        if (StartPointType == NodeType.Condition && !CycleThroughConditions(tempID, out tempID, out dummy))
        {
            return false;
        }

        currentNodeID = tempID;
        currentDialogue = this;
        CurrentNode.Visit();

        return true;
    }

    public bool DialogueFinished { get { return currentNodeID == Dialogue.ExitDialogue; } }
    public DialogueNode CurrentNode { get { return Nodes[currentNodeID]; } }

    public DialogueOption[] CurrentNodeOptions
    {
        get
        {
            List<DialogueOption> result = new List<DialogueOption>();

            if (!CurrentNode.ImmediateNode)
            {
                foreach (int optIndex in CurrentNode.OptionsAttached)
                {
                    if (Options[optIndex].CanDisplay)
                    {
                        result.Add(Options[optIndex]);
                    }
                }
            }

            return result.ToArray();
        }
    }

    /// <summary>
    /// Sets next node, assuming that the current node is an immediate node.
    /// </summary>
    public bool Next()
    {
        if (!CurrentNode.ImmediateNode)
        {
            throw new System.ArgumentException("Node is not an immediate node!");
        }

        NodeType targetType;
        int targetID;
        
        CurrentNode.GetTarget(out targetID, out targetType);

        if(targetType == NodeType.Condition)
        {
            if(!CycleThroughConditions(targetID, out targetID, out targetType))
            {
                return false;
            }
        }

        if (targetType == NodeType.Node)
        {
            currentNodeID = targetID;
            CurrentNode.Visit();
        }
        else
        {
            if (targetType == NodeType.Exit)
            {
                // Fire event that load a new dialogue
                return false;
            }
            else
            {
                throw new System.ArgumentException("Illegal node type");
            }
        }

        return true;
    }

    /// <summary>
    /// Sets next node, assuming that the current node is a normal node, and one of the available
    /// dialogue options has been chosen as an answer to what has been said in the current node.
    /// </summary>
    /// <param name="chosenAnswer"></param>
    public bool Next(DialogueOption chosenAnswer)
    {
        if (CurrentNode.ImmediateNode)
        {
            throw new System.ArgumentException("Node is an immediate node - operation illegal!");
        }

        chosenAnswer.Visit();

        int targetID = chosenAnswer.NextID;
        NodeType targetType = chosenAnswer.NextType;

        if (targetType == NodeType.Exit)
        {
            // Wywołaj event ładujący nowy dialog
            FireOnDialogueEnded(chosenAnswer.NextDialogue);
            return false;
        }

        if (targetType == NodeType.Condition && !CycleThroughConditions(targetID, out targetID, out targetType))
        {
            return false;
        }

        if (targetType == NodeType.Node)
        {
            currentNodeID = targetID;
            CurrentNode.Visit();            
        }

        return true;
    }

    /// <summary>
    /// Returns true if the dialogue did not reach an end
    /// </summary>
    /// <param name="startId"></param>
    /// <param name="targetID"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    private bool CycleThroughConditions(int startId, out int targetID, out NodeType targetType)
    {
        int prevID = startID;
        bool res = false;

        do
        {
            res = Conditions[prevID].ConditionTest(out targetID, out targetType);

            if (targetType != NodeType.Exit)
            {
                prevID = targetID;
            }
            else
            {
                FireOnDialogueEnded(
                    (res) ?
                        Conditions[prevID].NextDialogueIfPassed :
                        Conditions[prevID].NextDialogueIfFailed
                    );

                return false;
            }
        }
        while (targetType == NodeType.Condition);

        return true;
    }
}
