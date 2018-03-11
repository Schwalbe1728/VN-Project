using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public partial class DialogueEditor
{
    private List<bool> DialogueNodeActionsFoldouts = new List<bool>();

    void CreateDialogueNode()
    {
        DialogueNode newNode = new DialogueNode();
        newNode.NodeID = EditorInfo.Nodes;
        CurrentNodes.Add(newNode);

        EditorInfo.Windows.Add(new Rect(10 + scrollPosition.x, 10 + scrollPosition.y, 200, 5));
        EditorInfo.WindowTypes.Add(NodeType.Node);
        EditorInfo.NodeTypesIDs.Add(EditorInfo.Nodes++);
        EditorInfo.NodesIndexes.Add(EditorInfo.Windows.Count - 1);

        DialogueNodeActionsFoldouts.Add(false);

        WriteDebug("Adding node");

        SaveChanges("Create Dialogue Node");
    }

    void RequestNodeConnection(int typeid)
    {
        nodeToNodeToAttach.Add(typeid);
        nodeToOptionToAttach.Add(typeid);
        nodeToConditionToAttach.Add(typeid);
    }

    void DrawNodeWindow(int id)
    {
        int typeid = EditorInfo.NodeTypesIDs[id];
        DialogueNode currentNode = CurrentNodes[typeid];

        bool save = false;

        #region Connecting Buttons

        if (AnyConditionAwaitingConnection())
        {
            if (conditionFailToNodeToAttach.Count == 1 && GUILayout.Button("Connect Node To Condition Failure"))
            {
                conditionFailToNodeToAttach.Add(typeid);
            }

            if (conditionSuccessToNodeToAttach.Count == 1 && GUILayout.Button("Connect Node To Condition Success"))
            {
                conditionSuccessToNodeToAttach.Add(typeid);                
            }
        }

        if (optionToNodeToAttach.Count == 0)
        {
            if (nodeToNodeToAttach.Count == 0 && nodeToOptionToAttach.Count == 0)
            {
                if (!currentNode.ImmediateNode)
                {
                    if (!AnyConditionAwaitingConnection() && GUILayout.Button("Connect Node"))
                    {                        
                        RequestNodeConnection(typeid);
                    }
                }
                else
                {
                    bool nextIsExit = false;

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Next Node:");

                        int nxt;
                        NodeType nxtType;
                        currentNode.GetTarget(out nxt, out nxtType);

                        nextIsExit = nxt == Dialogue.ExitDialogue;
                        string nextString = (nextIsExit) ? "[EXIT]" : nxt.ToString();

                        GUILayout.Label(nextString);

                        if (!nextIsExit)
                        {
                            Rect focus =
                                EditorInfo.Windows[RetrieveWindowIndex(nxtType, nxt)];
                            DrawJumpToButton("Go To", focus, GUILayout.Width(50));
                        }

                        if (GUILayout.Button("Clear"))
                        {
                            //nodeToNodeAttached.Remove(typeid);
                            currentNode.RevertToRegularNode();
                            save = true;
                        }
                    }

                    if (nextIsExit)
                    {
                        DrawExitField(currentNode);
                    }
                }
            }
            else
            {
                if (nodeToNodeToAttach.Count == 1)
                {
                    int nextId; NodeType nextType;
                    currentNode.GetTarget(out nextId, out nextType);

                    if (nodeToNodeToAttach[0] != typeid &&
                        (!currentNode.ImmediateNode || nextId != nodeToNodeToAttach[0]))
                    {
                        if (GUILayout.Button("Connect As Immediate Node"))
                        {
                            nodeToNodeToAttach.Add(typeid);
                        }
                    }
                    else
                    {
                        if (nodeToNodeToAttach[0] == typeid)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                DrawCancelConnectionButton();

                                if (GUILayout.Button("Exits Dialogue"))
                                {
                                    nodeToNodeToAttach.Add(-1);
                                }
                            }                              
                        }
                    }
                }
                else
                {
                    DrawCancelConnectionButton();
                }
            }
        }
        else
        {
            if (optionToNodeToAttach.Count == 1)
            {
                if (GUILayout.Button("Make Option's Target"))
                {
                    optionToNodeToAttach.Add(typeid);
                }
            }
        }
        #endregion

        DrawSetStartingPointButton(id);

        string prevText = currentNode.NodeText;

        DrawTextArea(ref currentNode.NodeText, id);

        save = save || (currentNode.NodeText != null && !currentNode.NodeText.Equals(prevText));

        #region Opcje Dialogowe

        if (!currentNode.ImmediateNode)
        {
            bool restoreFoldout = !EditorInfo.NodesOptionsFoldouts.ContainsKey(typeid);

            foreach (int optionIndex in currentNode.OptionsAttached)
            {
                if (restoreFoldout)
                {
                    EditorInfo.RestoreFoldouts(CurrentNodes.ToArray());
                    break;
                }

                restoreFoldout |= !EditorInfo.NodesOptionsFoldouts[typeid].ContainsKey(optionIndex);
            }

            foreach (int optionIndex in currentNode.OptionsAttached)
            {
                if (!EditorInfo.NodesOptionsFoldouts.ContainsKey(typeid) || !EditorInfo.NodesOptionsFoldouts[typeid].ContainsKey(optionIndex))
                {
                    Debug.Log("Zawiera type id " + typeid + ": " + EditorInfo.NodesOptionsFoldouts.ContainsKey(typeid));
                    Debug.Log("Zawiera option id " + optionIndex + ": " + EditorInfo.NodesOptionsFoldouts[typeid].ContainsKey(optionIndex));
                }

                EditorInfo.NodesOptionsFoldouts[typeid][optionIndex] =
                    EditorGUILayout.Foldout(
                        EditorInfo.NodesOptionsFoldouts[typeid][optionIndex],
                        "Option: " + optionIndex, true
                        );

                if (EditorInfo.NodesOptionsFoldouts[typeid][optionIndex])
                {
                    Rect foldoutRect = EditorGUILayout.BeginHorizontal(Config.FoldoutInteriorStyle);
                    {
                        DialogueOption currentOption = CurrentOptions[optionIndex];

                        GUILayout.BeginVertical();
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Option: ", GUILayout.Width(75));
                                GUILayout.Label(optionIndex.ToString());
                                GUILayout.FlexibleSpace();
                                DrawJumpToButton("Go To", EditorInfo.Windows[EditorInfo.OptionsIndexes[optionIndex]], GUILayout.Width(50));
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Destination: ", GUILayout.Width(75));
                                if (currentOption.NextType == NodeType.Node)
                                {
                                    int to = currentOption.NextID; //optionToNodeAttached[optionIndex];
                                    GUILayout.Label(to.ToString());
                                    GUILayout.FlexibleSpace();
                                    DrawJumpToButton("Go To", EditorInfo.Windows[EditorInfo.NodesIndexes[to]], GUILayout.Width(50));
                                }
                                else
                                {
                                    GUILayout.Label("[EXIT]");
                                    GUILayout.FlexibleSpace();
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Text: ", GUILayout.Width(75));
                                StringBuilder textToShow = new StringBuilder(currentOption.OptionText);

                                if (textToShow.Length > Config.MaxQuotasLength)
                                {
                                    textToShow.Length = Config.MaxQuotasLength - 3;
                                    textToShow.Append("...");
                                }

                                GUILayout.Label("\"" + textToShow + "\"", Config.WrappedLabelStyle);
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();

                        if (GUILayout.Button("x", GUILayout.Width(20)))
                        {
                            List<int> optionsAttachedWithout =
                                new List<int>(currentNode.OptionsAttached);

                            optionsAttachedWithout.Remove(optionIndex);
                            currentNode.OptionsAttached = optionsAttachedWithout.ToArray();

                            EditorInfo.NodesOptionsFoldouts[typeid].Remove(optionIndex);

                            save = true;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        DrawActionsFoldout(id, typeid);

        #endregion        

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        {
            DrawResizeButtons(id);
            if (GUILayout.Button("Delete", GUILayout.Width(80)))
            {
                DeleteNodeWindow(id, typeid);
                SaveChanges("Delete Node");
                return;
            }
        }
        GUILayout.EndHorizontal();

        if (!resizing)
        {
            Vector2 margin = new Vector2(DragAreaMargin, DragAreaMargin);
            GUI.DragWindow(new Rect(Vector2.zero, EditorInfo.Windows[id].size - margin));
        }

        if (save)
        {
            SaveChanges("Draw Node Window");
        }
    }

    void EntityDeleted(int id)
    {
        EditorInfo.Windows.RemoveAt(id);
        EditorInfo.WindowTypes.RemoveAt(id);
        EditorInfo.NodeTypesIDs.RemoveAt(id);
    }

    void DeleteNodeWindow(int id, int idOfType)
    {
        EntityDeleted(id);
        EditorInfo.NodesIndexes.RemoveAt(idOfType);
        EditorInfo.NodesOptionsFoldouts.Remove(idOfType);
        EditorInfo.Nodes--;

        DialogueNodeActionsFoldouts.RemoveAt(idOfType);

        CurrentNodes.RemoveAt(idOfType);
        ClearAllConnectionsPending();        

        DecrementIndexes(id);
        UpdateTargetAfterDeletion(id, idOfType, NodeType.Node);
        

        int[] keys = new int[EditorInfo.NodesOptionsFoldouts.Count];
        EditorInfo.NodesOptionsFoldouts.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; i++)
        {
            int key = keys[i];
            Dictionary<int, bool> tempVal = EditorInfo.NodesOptionsFoldouts[key];

            if (key > idOfType)
            {
                EditorInfo.NodesOptionsFoldouts.Remove(key);
                key--;
                EditorInfo.NodesOptionsFoldouts.Add(key, tempVal);
            }
        }               

        WriteDebug("Deleting node " + idOfType + " and it's associations.");
    }    

    void UpdateTargetAfterDeletion(int id, int idOfType, NodeType deletedType)
    {
        for (int i = 0; i < EditorInfo.NodeTypesIDs.Count; i++)
        {
            if (EditorInfo.WindowTypes[i] == deletedType && EditorInfo.NodeTypesIDs[i] > idOfType)
            {
                EditorInfo.NodeTypesIDs[i]--;
            }
        }

        foreach (DialogueNode dialNode in CurrentNodes)
        {
            if (deletedType == NodeType.Node && dialNode.NodeID > idOfType)
            {
                dialNode.NodeID--;
            }

            if (dialNode.ImmediateNode)
            {
                int targID; NodeType targType;
                dialNode.GetTarget(out targID, out targType);

                if (targType == deletedType)
                {
                    if (targID > idOfType)
                    {
                        dialNode.SetImmediateNodeTarget(targID - 1, targType);
                    }
                    else
                    {
                        if (targID == idOfType)
                        {
                            dialNode.RevertToRegularNode();
                        }
                    }
                }
            }
            else
            {
                if (dialNode.OptionsAttached != null)
                {
                    List<int> optionsAttached = new List<int>(dialNode.OptionsAttached);
                    optionsAttached.Remove(idOfType);

                    for (int i = 0; i < optionsAttached.Count; i++)
                    {
                        if (optionsAttached[i] > idOfType)
                        {
                            optionsAttached[i]--;
                        }
                    }

                    dialNode.OptionsAttached = optionsAttached.ToArray();
                }
            }
        }

        foreach (DialogueOption opt in CurrentOptions)
        {
            if(deletedType == NodeType.Option && opt.OptionID > idOfType)
            {
                opt.OptionID--;
            }

            if(deletedType == NodeType.Condition && opt.EntryConditionSet && opt.EntryCondition.ConditionID == idOfType)
            {
                opt.ClearEntryCondition();
            }

            if (opt.NextType == deletedType)
            {
                if (opt.NextID > idOfType)
                {
                    opt.SetNext(opt.NextID - 1, opt.NextType);
                }
                else
                {
                    if (opt.NextID == idOfType)
                    {
                        opt.SetNextNodeExit();
                    }
                }
            }
        }

        foreach (ConditionNode cond in CurrentConditions)
        {
            if(deletedType == NodeType.Condition && cond.ConditionID > idOfType)
            {
                cond.ConditionID--;
            }

            if (cond.SuccessTargetType == deletedType)
            {
                if (cond.SuccessTarget == idOfType)
                {
                    cond.SetSuccessTarget(Dialogue.ExitDialogue, NodeType.Exit);
                }
                else
                {
                    if (cond.SuccessTarget > idOfType)
                    {
                        cond.SetSuccessTarget(cond.SuccessTarget - 1, deletedType);
                    }
                }
            }

            if (cond.FailureTargetType == deletedType)
            {
                if (cond.FailureTarget == idOfType)
                {
                    cond.SetFailureTarget(Dialogue.ExitDialogue, NodeType.Exit);
                }
                else
                {
                    if (cond.FailureTarget > idOfType)
                    {
                        cond.SetFailureTarget(cond.FailureTarget - 1, deletedType);
                    }
                }
            }
        }
    }

    void DecrementIndexes(int id)
    {
        for (int i = EditorInfo.NodesIndexes.FindIndex(x => x > id); i < EditorInfo.NodesIndexes.Count && i >= 0; i++)
        {
            EditorInfo.NodesIndexes[i]--;
        }

        for (int i = EditorInfo.OptionsIndexes.FindIndex(x => x > id); i < EditorInfo.OptionsIndexes.Count && i >= 0; i++)
        {
            EditorInfo.OptionsIndexes[i]--;
        }

        for (int i = EditorInfo.ConditionsIndexes.FindIndex(x => x > id); i < EditorInfo.ConditionsIndexes.Count && i >= 0; i++)
        {
            EditorInfo.ConditionsIndexes[i]--;
        }
    }

    bool DrawActionsFoldout(int id, int idOfType)
    {
        bool save = false;
        DialogueAction currentAction = CurrentNodes[idOfType].Action;        

        DialogueNodeActionsFoldouts[idOfType] =
            EditorGUILayout.Foldout(DialogueNodeActionsFoldouts[idOfType], "Defined Actions: ");

        if(DialogueNodeActionsFoldouts[idOfType])
        {
            Rect dummy = EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
            {
                currentAction.HurtPlayerSet =
                    EditorGUILayout.ToggleLeft("Hurt Player", currentAction.HurtPlayerSet);

                if(currentAction.HurtPlayerSet)
                {
                    //rysuj kontrolkę HurtPlayer
                    save |= DrawHurtPlayerActionInterior(currentAction);
                }

                currentAction.HurtPlayerSanitySet =
                    EditorGUILayout.ToggleLeft("Hurt Player's Sanity", currentAction.HurtPlayerSanitySet);

                if (currentAction.HurtPlayerSanitySet)
                {
                    save |= DrawHurtPlayerSanityInterior(currentAction);
                }
            }
            EditorGUILayout.EndVertical();
        }

        return save;
    }    
}
