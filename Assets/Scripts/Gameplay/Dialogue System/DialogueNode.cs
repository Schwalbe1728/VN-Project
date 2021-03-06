﻿using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode : DialogueElement
{
    private static int textLettersPerSecond = 10;

    [SerializeField]
    private int nodeID;

    [SerializeField]
    private bool isImmediateNode;
    [SerializeField]
    private bool addContinue;

    [SerializeField]
    private int nextID = Dialogue.ExitDialogue;
    [SerializeField]
    private NodeType nextType = NodeType.Exit;

    [HideInInspector]
    public string NodeText;
    [SerializeField]
    private string customID;

    [SerializeField]
    private List<int> optionsIndexesList;

    //[SerializeField]
    //private string nextDialogueID;

    [SerializeField]
    private Dialogue nextDialogue;

    [SerializeField]
    private DialogueAction action;
    [SerializeField]
    private bool actionSet;

    [SerializeField]
    private int secondsConsumed;

    //public string NextDialogueID { get { return nextDialogueID; } set { nextDialogueID = value; } }
    public Dialogue NextDialogue { get { return nextDialogue; } set { nextDialogue = value; } }

    public int NodeID { get { return nodeID; } set { if (value >= 0) nodeID = value; } }
    //public string Text { get { return nodeText; } set { nodeText = value; } }
    public string CustomID
    {
        get
        {
            return (customID != null && !customID.Equals("")) ?
                customID :
                "Node " + nodeID.ToString();
        }

        set
        {
            customID = value;
        }
    }
    
    public DialogueAction Action { get { return action; } }    

    public DialogueNode()
    {
        optionsIndexesList = new List<int>();
    }

    public void SetAction(DialogueAction nAction)
    {
        action = nAction;
        actionSet = action != null;
    }

    public void ClearAction()
    {
        action = null;
        actionSet = false;
    }
    
    public int SecondsConsumed
    {
        get
        {
            float temp = NodeText.Length;

            return Mathf.CeilToInt(temp / textLettersPerSecond);
        }
    }       

    #region Options Attached
    public int[] OptionsAttached
    {
        get
        {
            int[] result =
                (!ImmediateNode && optionsIndexesList != null) ?
                    optionsIndexesList.ToArray() : null;

            return result;
        }

        set
        {
            optionsIndexesList = new List<int>(value);
        }
    }

    public void ClearOptionsAttached()
    {
        optionsIndexesList.Clear();
    }
    #endregion
    #region Immediate Nodes
    public bool ImmediateNode { get { return isImmediateNode; } }

    public void RevertToRegularNode()
    {
        isImmediateNode = false;
    }

    public void MakeImmediateNode()
    {
        isImmediateNode = true;
    }

    public bool AddContinue { get { return addContinue; } set { addContinue = value; } }

    public void SetImmediateNodeTarget(int nID, NodeType nType)
    {
        if (isImmediateNode && nID >= 0 && nType != NodeType.Option && nType != NodeType.Exit)
        {
            nextID = nID;
            nextType = nType;
        }
    }

    public void SetImmediateNodeTargetExit()
    {
        if (isImmediateNode)
        {
            nextID = Dialogue.ExitDialogue;
            nextType = NodeType.Exit;
        }
    }
    
    public void GetTarget(out int targetID, out NodeType targetType)
    {
        targetID = this.nextID;
        targetType = this.nextType;
    }

    public void Visit()
    {
        if(Action != null)
        {
            bool advanceTimeArtificiallyCreated = false;

            if (!Action.AdvanceTimeSet && SecondsConsumed > 0)
            {
                // utwórz akcję o czasie trwanie SecondsConsumed 
                float timeVarianceForDefaultNode = 0.075f;

                Action.SetAdvanceTimeAction(SecondsConsumed, true, timeVarianceForDefaultNode);
                advanceTimeArtificiallyCreated = true;
            }             

            Action.DoAction();

            if(advanceTimeArtificiallyCreated)
            {
                Action.ClearAdvanceTimeAction();
            }
        }        
    }

    public int TargetID { get { return nextID; } }
    public NodeType TargetType { get { return nextType; } }

    #endregion
}

