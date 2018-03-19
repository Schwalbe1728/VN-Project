using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DialogueEditor : EditorWindow
{
    private Dialogue EditedDialogue;
    private DialogueEditorInfo EditorInfo;
    private List<DialogueNode> CurrentNodes;
    private List<DialogueOption> CurrentOptions;
    private List<ConditionNode> CurrentConditions;

    private EditorConfigurationData Config;    

    [MenuItem("Window/Dialogue Editor")]
    static void ShowEditor()
    {
        DialogueEditor editor = EditorWindow.GetWindow<DialogueEditor>();
        editor.wantsMouseMove = true;
        //InitStyles();
        Selection.selectionChanged -= editor.OnEditorSelectionChanged;
        Selection.selectionChanged += editor.OnEditorSelectionChanged;

        editor.OnEditorSelectionChanged();
    }

    void OnFocus()
    {
        Selection.selectionChanged -= OnEditorSelectionChanged;
        Selection.selectionChanged += OnEditorSelectionChanged;
    }

    void OnDestroy()
    {
        SaveChanges("Node Editor On Destroy");
        Selection.selectionChanged -= OnEditorSelectionChanged;
    }

    void SaveChanges(string undoTitle)
    {
        if (EditedDialogue != null)
        {
            //Undo.RecordObject(EditedDialogue, undoTitle);

            EditedDialogue.SetAllNodes(CurrentNodes);
            EditedDialogue.SetAllOptions(CurrentOptions);
            EditedDialogue.SetAllConditions(CurrentConditions);
            EditedDialogue.EditorInfo = EditorInfo;

            EditorUtility.SetDirty(EditedDialogue);
            //AssetDatabase.SaveAssets();
            //Debug.Log("Saving changes");
        }
    }

    void OnEditorSelectionChanged()
    {
        //Debug.Log("ChangedSelection");

        resizing = false;

        if (Selection.activeObject is Dialogue)
        {
            EditedDialogue = Selection.activeObject as Dialogue;
            EditorInfo = EditedDialogue.EditorInfo;

            if (EditorInfo == null)
            {
                EditorInfo = new DialogueEditorInfo();
            }
            else
            {
                EditorInfo.RestoreFoldouts(EditedDialogue.GetAllNodes());
            }

            DialogueNodeActionsFoldouts = null;

            Repaint();
        }
    }

    void OnGUI()
    {
        if (EditedDialogue == null || EditorInfo == null)
        {
            GUILayout.Label("Please, select a Dialogue to edit!", EditorStyles.boldLabel);
            return;
        }

        CurrentNodes = new List<DialogueNode>(EditedDialogue.GetAllNodes());
        CurrentOptions = new List<DialogueOption>(EditedDialogue.GetAllOptions());
        CurrentConditions = new List<ConditionNode>(EditedDialogue.GetAllConditions());

        if(DialogueNodeActionsFoldouts == null)
        {
            DialogueNodeActionsFoldouts = new List<bool>();
            foreach(DialogueNode node in CurrentNodes)
            {
                DialogueNodeActionsFoldouts.Add(false);
            }
        }

        if (Config == null)
        {
            Config = new EditorConfigurationData();
        }

        int margins = 5;
        int editorMenuHeight = 40;
        int debugAreaHeight = 25;

        GUILayout.BeginArea(new Rect(margins, margins, position.width - 2 * margins, editorMenuHeight));
        {
            DrawEditorMenu();
        }
        GUILayout.EndArea();

        DrawDebug(debugAreaHeight, margins);

        GUILayout.BeginArea(
            new Rect(
                margins, editorMenuHeight + 2 * margins, 
                position.width - 2 * margins, position.height - (debugAreaHeight + 2 * margins) - editorMenuHeight), 
            Config.EditorAreaBackgroundStyle);
        {
            DrawEditorArea();
        }
        GUILayout.EndArea();
    }
}


