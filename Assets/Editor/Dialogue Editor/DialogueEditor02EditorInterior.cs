using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class DialogueEditor
{
    private Vector2 scrollPosition;

    void DrawEditorArea()
    {
        Bounds border = new Bounds();

        for (int i = 0; i < EditorInfo.Windows.Count; i++)
        {
            border.Encapsulate(EditorInfo.Windows[i].max);
            border.Encapsulate(EditorInfo.Windows[i].min);
        }

        scrollPosition =
            EditorGUILayout.BeginScrollView(
                    scrollPosition,
                    GUILayout.ExpandWidth(true),
                    GUILayout.ExpandHeight(true)
                );
        {
            GUILayout.Box("", Config.BoundingBoxStyle, GUILayout.Width(border.size.x), GUILayout.Height(border.size.y));

            BeginWindows();
            {
                //GUIUtility.ScaleAroundPivot(Vector2.one * scale, scrollPosition + position.size / 2);

                for (int i = 0; i < EditorInfo.Windows.Count; i++)
                {
                    bool isNode = EditorInfo.WindowTypes[i] == NodeType.Node;
                    bool isOption = EditorInfo.WindowTypes[i] == NodeType.Option;

                    switch(EditorInfo.WindowTypes[i])
                    {
                        case NodeType.Node:
                            EditorInfo.Windows[i] =
                                GUILayout.Window(i, EditorInfo.Windows[i], DrawNodeWindow, CurrentNodes[EditorInfo.NodeTypesIDs[i]].CustomID, Config.ConditionNodeStyle);
                            break;

                        case NodeType.Option:
                            EditorInfo.Windows[i] =
                                GUILayout.Window(i, EditorInfo.Windows[i], DrawOptionWindow, "Option " + EditorInfo.NodeTypesIDs[i], Config.DialogueOptionStyle);
                            break;

                        case NodeType.Condition:
                            EditorInfo.Windows[i] =
                                GUILayout.Window(i, EditorInfo.Windows[i], DrawConditionWindow, "Condition " + EditorInfo.NodeTypesIDs[i], Config.DialogueNodeStyle);
                            break;
                    }
                }

                UpdateCurves();
            }
            EndWindows();
        }
        EditorGUILayout.EndScrollView();
    }
}
