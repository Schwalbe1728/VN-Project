using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class DialogueEditor
{
    public void CreateConditionNode()
    {
        ConditionNode newCondition = new ConditionNode();
        newCondition.ConditionID = EditorInfo.Conditions;
        CurrentConditions.Add(newCondition);

        EditorInfo.Windows.Add(new Rect(10 + scrollPosition.x, 10 + scrollPosition.y, 200, 5));
        EditorInfo.WindowTypes.Add(NodeType.Condition);
        EditorInfo.NodeTypesIDs.Add(EditorInfo.Conditions++);
        EditorInfo.ConditionsIndexes.Add(EditorInfo.Windows.Count - 1);

        newCondition.SetAttributeCheckCondition(CharacterStat.Agility, 10, InequalityTypes.Equal);

        SaveChanges("Create Condition Node");
    }

    bool AnyConditionAwaitingConnection()
    {
        return
            conditionToEntryOption.Count == 1 ||
            conditionSuccessToNodeToAttach.Count == 1 ||
            conditionSuccessToConditionToAttach.Count == 1 ||
            conditionFailToConditionToAttach.Count == 1 ||
            conditionFailToNodeToAttach.Count == 1;
    }

    bool ThisConditionAwaitingConnection(int idOfType)
    {
        return
            (conditionToEntryOption.Count == 1 && conditionToEntryOption[0] == idOfType) ||
            (conditionSuccessToNodeToAttach.Count == 1 && conditionSuccessToNodeToAttach[0] == idOfType) ||
            (conditionSuccessToConditionToAttach.Count == 1 && conditionSuccessToConditionToAttach[0] == idOfType) ||
            (conditionFailToConditionToAttach.Count == 1 && conditionFailToConditionToAttach[0] == idOfType) ||
            (conditionFailToNodeToAttach.Count == 1 && conditionFailToNodeToAttach[0] == idOfType);
    }    

    bool ThisConditionSuccessAwaitingConnection(int idOfType)
    {
        return
            (conditionSuccessToNodeToAttach.Count == 1 && conditionSuccessToNodeToAttach[0] == idOfType) ||
            (conditionSuccessToConditionToAttach.Count == 1 && conditionSuccessToConditionToAttach[0] == idOfType);
    }

    bool ThisConditionFailAwaitingConnection(int idOfType)
    {
        return
            (conditionFailToConditionToAttach.Count == 1 && conditionFailToConditionToAttach[0] == idOfType) ||
            (conditionFailToNodeToAttach.Count == 1 && conditionFailToNodeToAttach[0] == idOfType);
    }

    void RequestConditionConnection(int idOfType, bool isSuccess)
    {
        if (isSuccess)
        {
            conditionSuccessToConditionToAttach.Add(idOfType);
            conditionSuccessToNodeToAttach.Add(idOfType);
        }
        else
        {
            conditionFailToConditionToAttach.Add(idOfType);
            conditionFailToNodeToAttach.Add(idOfType);
        }

        conditionToEntryOption.Add(idOfType);
    }

    void DrawConditionWindow(int id)
    {
        bool save = false;

        int typeID = EditorInfo.NodeTypesIDs[id];
        ConditionNode currentCondition = CurrentConditions[typeID];

        DrawConnectToConditionButtons(typeID);
        DrawSetStartingPointButton(id);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Target If Success: ", GUILayout.Width(110));

            DrawTargetValue(currentCondition, currentCondition.SuccessTargetType, currentCondition.SuccessTarget, true);

            if (!AnyConditionAwaitingConnection() && GUILayout.Button("Set", GUILayout.Width(50)))
            {
                RequestConditionConnection(typeID, true);
            }

            if (ThisConditionSuccessAwaitingConnection(typeID) && GUILayout.Button("Set [EXIT]"))
            {
                ClearConditionConnectionsPending();
                currentCondition.SetSuccessTarget(Dialogue.ExitDialogue, NodeType.Exit);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Target If Failure: ", GUILayout.Width(110));

            DrawTargetValue(currentCondition, currentCondition.FailureTargetType, currentCondition.FailureTarget, false);

            if (!AnyConditionAwaitingConnection() && GUILayout.Button("Set", GUILayout.Width(50)))
            {
                RequestConditionConnection(typeID, false);
            }

            if (ThisConditionFailAwaitingConnection(typeID) && GUILayout.Button("Set [EXIT]"))
            {
                ClearConditionConnectionsPending();
                currentCondition.SetFailureTarget(Dialogue.ExitDialogue, NodeType.Exit);
            }
        }
        GUILayout.EndHorizontal();

        if (ThisConditionAwaitingConnection(typeID) && GUILayout.Button("Cancel Connection"))
        {
            ClearConditionConnectionsPending();
        }

        ConditionTypes prevType = currentCondition.ConditionType;

        ConditionTypes newType =
            (ConditionTypes)EditorGUILayout.EnumPopup(currentCondition.ConditionType);        

        switch (newType)
        {
            case ConditionTypes.AttributeCheck:                
                save |= DrawAttributeCheckInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.AttributeTest:
                save |= DrawAttributeTestInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.SkillPossessed:
                save |= DrawSkillPossessedInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.PlayerHasItem:
                save |= DrawPlayerHasItemInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.StoryStateHappened:
                save |= DrawStoryStateHappenedInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.WorldDate:
                save |= DrawWorldDateInterior(currentCondition, prevType != newType);
                break;

            case ConditionTypes.WithinTimeRange:
                save |= DrawWithinTimeRangeInterior(currentCondition, prevType != newType);
                break;
        }

        GUILayout.BeginHorizontal();
        {
            DrawResizeButtons(id);

            if (GUILayout.Button("Delete Condition"))
            {
                DeleteConditionWindow(id, typeID);
                SaveChanges("Delete Condition");
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
            SaveChanges("Condition Node Window");
        }
    }

    void DeleteConditionWindow(int id, int idOfType)
    {
        EntityDeleted(id);
        EditorInfo.ConditionsIndexes.RemoveAt(idOfType);
        EditorInfo.Conditions--;

        CurrentConditions.RemoveAt(idOfType);
        ClearAllConnectionsPending();

        DecrementIndexes(id);
        UpdateTargetAfterDeletion(id, idOfType, NodeType.Condition);

        WriteDebug("Deleting condition " + idOfType + " and it's associations.");
    }

    void DrawConnectToConditionButtons(int typeID)
    {
        if ((AnyConditionAwaitingConnection() || nodeToConditionToAttach.Count == 1 || optionToConditionToAttach.Count == 1) && !ThisConditionAwaitingConnection(typeID))
        {
            if (nodeToConditionToAttach.Count == 1 && GUILayout.Button("Connect Node To Condition"))
            {
                nodeToConditionToAttach.Add(typeID);
            }

            if (optionToConditionToAttach.Count == 1 && GUILayout.Button("Connect Option To Condition"))
            {
                optionToConditionToAttach.Add(typeID);
            }

            if (conditionFailToConditionToAttach.Count == 1 && GUILayout.Button("Connect Failure To Condition"))
            {
                conditionFailToConditionToAttach.Add(typeID);
            }

            if (conditionSuccessToConditionToAttach.Count == 1 && GUILayout.Button("Connect Success To Condition"))
            {
                conditionSuccessToConditionToAttach.Add(typeID);
            }
        }
    }

    void DrawTargetValue(ConditionNode currentCondition, NodeType targetType, int targetID, bool success)
    {
        if (targetType == NodeType.Exit)
        {
            DrawExitField(currentCondition, success);
        }
        else
        {
            string nodeType = targetType.ToString(true);
            string targetId = targetID.ToString();

            //GUILayout.Label();
            int windowIndex =
                RetrieveWindowIndex(targetType, targetID);

            DrawJumpToButton(nodeType + " " + targetId, EditorInfo.Windows[windowIndex]);
        }

    }

    int RetrieveWindowIndex(NodeType targetType, int targetIndex)
    {
        switch(targetType)
        {
            case NodeType.Condition:
                return EditorInfo.ConditionsIndexes[targetIndex];

            case NodeType.Node:
                return EditorInfo.NodesIndexes[targetIndex];

            case NodeType.Option:
                return EditorInfo.OptionsIndexes[targetIndex];

            default:
                return -1;
        }
    }
}
