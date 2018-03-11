using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DialogueEditor
{
    void DrawEditorMenu()
    {
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Create Dialogue Node"))
            {
                CreateDialogueNode();                
            }

            if (GUILayout.Button("Create Dialogue Option"))
            {
                CreateDialogueOption();
            }

            if (GUILayout.Button("Create Condition Node"))
            {
                CreateConditionNode();
            }

            //TODO: Rozważyć akcje
            /*
            if (GUILayout.Button("Sort"))
            {
                WriteDebug("Not Implemented Function");
            }
            */
            
            if (GUILayout.Button("Configure"))
            {
                ConfigurationWindow.ShowConfigMenu(Config, this.Repaint);                
            }
        }
        GUILayout.EndHorizontal();
    }
}
