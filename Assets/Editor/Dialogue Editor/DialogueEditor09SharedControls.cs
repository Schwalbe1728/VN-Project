using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class DialogueEditor
{
    private float DragAreaMargin = 8f;

    private bool resizing = false;
    private int windowResizedIndex = -1;

    void DrawResizeButtons(int id)
    {
        if (resizing)
        {
            if (windowResizedIndex == id)
            {
                if (Event.current.type == EventType.MouseDrag)
                {
                    Rect tempWindow = EditorInfo.Windows[id];
                    tempWindow.size += Event.current.delta;
                    EditorInfo.Windows[id] = tempWindow;

                    Repaint();
                }

                resizing = EditorGUILayout.ToggleLeft("Resize", resizing, GUILayout.Width(80));
            }
        }
        else
        {
            resizing = EditorGUILayout.ToggleLeft("Resize", resizing, GUILayout.Width(80));

            if (resizing)
            {
                windowResizedIndex = id;
            }
        }
    }

    void DrawSetStartingPointButton(int id)
    {
        NodeType typeNode = EditorInfo.WindowTypes[id];
        int startTypeId = EditorInfo.NodeTypesIDs[id];

        if (!EditedDialogue.IsStartPoint(startTypeId, typeNode))
        {
            if (GUILayout.Button("Set As Start Point"))
            {
                EditedDialogue.SetStartPoint(startTypeId, typeNode);
            }
        }
        else
        {
            GUILayout.Label("Starting Point of the Dialogue", EditorStyles.boldLabel);
        }
    }

    void FocusOnRect(Rect rect)
    {
        scrollPosition = rect.center - new Vector2(position.width / 2, position.height / 2);
    }

    bool DrawJumpToButton(string label, Rect jumpToRect, params GUILayoutOption[] param)
    {
        bool result = GUILayout.Button(label, param);

        if (result)
        {
            FocusOnRect(jumpToRect);
        }

        return result;
    }

    void DrawExitField(DialogueNode node)
    {
        EditorGUILayout.BeginHorizontal();
        {
            node.NextDialogueID =
                EditorGUILayout.TextField(
                    (node.NextDialogueID == null) ?
                        "" : node.NextDialogueID
                    );
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawExitField(DialogueOption option)
    {
        EditorGUILayout.BeginHorizontal();
        {
            option.NextDialogueID =
                EditorGUILayout.TextField(
                    (option.NextDialogueID == null) ?
                        "" : option.NextDialogueID
                    );
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawExitField(ConditionNode condition, bool isSuccess)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("Next Dialogue: ");

            if (isSuccess)
            {
                condition.NextDialogueIDIfPassed =
                  EditorGUILayout.TextField(
                      (condition.NextDialogueIDIfPassed == null) ?
                          "" : condition.NextDialogueIDIfPassed
                      );
            }
            else
            {
                condition.NextDialogueIDIfFailed =
                  EditorGUILayout.TextField(
                      (condition.NextDialogueIDIfFailed == null) ?
                          "" : condition.NextDialogueIDIfFailed
                      );
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawCancelConnectionButton()
    {
        if (GUILayout.Button("Cancel Connection"))
        {
            ClearAllConnectionsPending();
        }
    }

    void DrawTargetValue<T>(T current, NodeType targetType, int targetID) where T : DialogueElement
    {
        if (targetType == NodeType.Exit)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("[EXIT] to ");

                if (current is DialogueNode)
                {
                    DrawExitField(current as DialogueNode);
                }
                else
                {
                    if (current is DialogueOption)
                    {
                        DrawExitField(current as DialogueOption);
                    }
                }
            }
            GUILayout.EndHorizontal();
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

    void DrawTextArea(ref string text, int id)
    {
        text =
            EditorGUILayout.TextArea(text,
                Config.TextAreaStyle,
                GUILayout.ExpandHeight(true), GUILayout.MinHeight(Config.MinTextAreaHeight), GUILayout.MaxHeight(Config.MaxTextAreaHeight),
                GUILayout.ExpandWidth(false), GUILayout.Width(EditorInfo.Windows[id].width - 2 * DragAreaMargin)
                );
    }
}
