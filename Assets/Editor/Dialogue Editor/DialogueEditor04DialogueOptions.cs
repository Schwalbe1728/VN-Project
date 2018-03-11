using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public partial class DialogueEditor
{
    public void CreateDialogueOption()
    {
        DialogueOption newOption = new DialogueOption();
        newOption.OptionID = EditorInfo.Options;
        CurrentOptions.Add(newOption);

        EditorInfo.Windows.Add(new Rect(10 + scrollPosition.x, 10 + scrollPosition.y, 200, 5));
        EditorInfo.WindowTypes.Add(NodeType.Option);
        EditorInfo.NodeTypesIDs.Add(EditorInfo.Options++);
        EditorInfo.OptionsIndexes.Add(EditorInfo.Windows.Count - 1);

        WriteDebug("Adding option");

        SaveChanges("Create Dialogue Option");
    }

    void RequestOptionConnection(int typeid)
    {
        optionToNodeToAttach.Add(typeid);
        optionToConditionToAttach.Add(typeid);
    }

    void DrawOptionWindow(int id)
    {
        int typeid = EditorInfo.NodeTypesIDs[id];
        DialogueOption currentOption = CurrentOptions[typeid];
        //bool save = false;

        #region Option's Target Setting
        if (nodeToOptionToAttach.Count == 1)
        {
            DialogueNode nodeAwaiting = CurrentNodes[nodeToOptionToAttach[0]];
            List<int> optionsAttached = new List<int>(nodeAwaiting.OptionsAttached);

            if (!optionsAttached.Contains(typeid) && GUILayout.Button("Connect"))
            {
                nodeToOptionToAttach.Add(typeid);
                if (nodeToNodeToAttach.Count > 0)
                {
                    nodeAwaiting.RevertToRegularNode();
                }
            }
        }

        bool tempCont = currentOption.NextType != NodeType.Exit;

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Next: ");
            DrawTargetValue(currentOption, currentOption.NextType, currentOption.NextID);

            if (tempCont)
            {
                if (GUILayout.Button("Clear"))
                {
                    currentOption.SetNextNodeExit();
                }
            }
            else
            {
                if (optionToNodeToAttach.Count == 0 && optionToConditionToAttach.Count == 0)
                {
                    if (!tempCont)
                    {
                        if (GUILayout.Button("Set"))
                        {
                            RequestOptionConnection(typeid);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Clear"))
                        {
                            currentOption.SetNextNodeExit();
                        }
                    }
                }
                else
                {
                    DrawCancelConnectionButton();
                }
            }
        }
        GUILayout.EndHorizontal();        

        #endregion
        #region Visit Once

        currentOption.VisitOnce = EditorGUILayout.Toggle("Visit Once: ", currentOption.VisitOnce);

        #endregion
        #region Connect Entry Condition

        if (!AnyConditionAwaitingConnection())
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Entry Condition: ");

                if (!currentOption.EntryConditionSet)
                {
                    GUILayout.Label("[none]");
                }
                else
                {
                    DrawJumpToButton("Go To", EditorInfo.Windows[EditorInfo.ConditionsIndexes[currentOption.EntryCondition.ConditionID]]);

                    if (GUILayout.Button("x"))
                    {
                        currentOption.ClearEntryCondition();
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (!currentOption.EntryConditionSet && GUILayout.Button("Connect As Entry Condition"))
            {
                conditionToEntryOption.Add(typeid);
            }
        }
        #endregion
        #region Option Text        
        string prev = currentOption.OptionText;

        DrawTextArea(ref currentOption.OptionText, id);

        //save = save || ( currentOption.OptionText != null && !currentOption.OptionText.Equals(prev));
        #endregion

        GUILayout.BeginHorizontal();
        {
            DrawResizeButtons(id);

            if (GUILayout.Button("Delete", GUILayout.Width(80)))
            {
                DeleteOptionWindow(id, typeid);
                SaveChanges("Delete dialogue option");
                return;
            }
        }
        GUILayout.EndHorizontal();

        if (!resizing)
        {
            Vector2 margin = new Vector2(DragAreaMargin, DragAreaMargin);
            GUI.DragWindow(new Rect(Vector2.zero, EditorInfo.Windows[id].size - margin));
        }

        if (/*save*/ GUI.changed)
        {
            SaveChanges("Draw Option Window");
        }
    }    

    void DeleteOptionWindow(int id, int idOfType)
    {
        EntityDeleted(id);
        EditorInfo.OptionsIndexes.RemoveAt(idOfType);
        EditorInfo.Options--;

        CurrentOptions.RemoveAt(idOfType);

        ClearAllConnectionsPending();

        DecrementIndexes(id);
        UpdateTargetAfterDeletion(id, idOfType, NodeType.Option);        

        int[] keys = new int[EditorInfo.NodesOptionsFoldouts.Count];
        EditorInfo.NodesOptionsFoldouts.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; i++)
        {
            int[] optionsKeys = new int[EditorInfo.NodesOptionsFoldouts[keys[i]].Count];
            EditorInfo.NodesOptionsFoldouts[keys[i]].Keys.CopyTo(optionsKeys, 0);

            for (int j = 0; j < optionsKeys.Length; j++)
            {
                int key = optionsKeys[j];

                if (key > idOfType)
                {
                    bool value = EditorInfo.NodesOptionsFoldouts[keys[i]][key];
                    EditorInfo.NodesOptionsFoldouts[keys[i]].Remove(key);
                    key--;
                    EditorInfo.NodesOptionsFoldouts[keys[i]].Add(key, value);
                }
            }
        }

        WriteDebug("Deleting option " + idOfType + " and it's associations.");
    }
}
