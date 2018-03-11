using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public const int ExitDialogue = -1;

    private static Dialogue currentDialogue;

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
    public void StartDialogue()
    {
        int tempID = startID;

        if (StartPointType == NodeType.Condition)
        {
            // oblicz pierwszy wyświetlony Node            
            NodeType nxtType;

            do
            {
                Conditions[tempID].ConditionTest(out tempID, out nxtType);
            }
            while (nxtType == NodeType.Condition);
        }

        currentNodeID = tempID;
        currentDialogue = this;
    }

    public bool DialogueFinished { get { return currentNodeID == Dialogue.ExitDialogue; } }
    public DialogueNode CurrentNode { get { return Nodes[currentNodeID]; } }

    public DialogueOption[] CurrentNodeOptions
    {
        get
        {
            List<DialogueOption> result = new List<DialogueOption>();

            foreach (int optIndex in CurrentNode.OptionsAttached)
            {
                if (Options[optIndex].CanDisplay)
                {
                    result.Add(Options[optIndex]);
                }
            }

            return result.ToArray();
        }
    }

    /// <summary>
    /// Sets next node, assuming that the current node is an immediate node.
    /// </summary>
    public void Next()
    {
        if (!CurrentNode.ImmediateNode)
        {
            throw new System.ArgumentException("Node is not an immediate node!");
        }

        NodeType targetType;
        int targetID;

        CurrentNode.GetTarget(out targetID, out targetType);

        while (targetType == NodeType.Condition)
        {
            if (Conditions[targetID].ConditionTest(out targetID, out targetType))
            {
                //  test success event
            }
            else
            {
                //  test failure event
            }
        }

        if (targetType == NodeType.Node)
        {
            currentNodeID = targetID;
        }
        else
        {
            if (targetType == NodeType.Exit)
            {
                // Fire event that load a new dialogue
            }
            else
            {
                throw new System.ArgumentException("Illegal node type");
            }
        }
    }

    /// <summary>
    /// Sets next node, assuming that the current node is a normal node, and one of the available
    /// dialogue options has been chosen as an answer to what has been said in the current node.
    /// </summary>
    /// <param name="chosenAnswer"></param>
    public void Next(DialogueOption chosenAnswer)
    {
        if (CurrentNode.ImmediateNode)
        {
            throw new System.ArgumentException("Node is an immediate node - operation illegal!");
        }

        int targetID = chosenAnswer.NextID;
        NodeType targetType = chosenAnswer.NextType;

        while (targetType == NodeType.Condition)
        {
            if (Conditions[targetID].ConditionTest(out targetID, out targetType))
            {
                // test success event
            }
            else
            {
                // test failure event
            }
        }

        if (targetType == NodeType.Node)
        {
            currentNodeID = targetID;
        }
        else
        {
            if (targetType == NodeType.Exit)
            {
                // Wywołaj event ładujący nowy dialog
            }
        }
    }
}
