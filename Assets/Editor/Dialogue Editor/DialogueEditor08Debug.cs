using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DialogueEditor
{
    private List<string> debugMessages = new List<string>();
    private int selectedDebugMessage = -1;

    void WriteDebug(string message, bool forceScroll = true)
    {
        debugMessages.Add(debugMessages.Count.ToString() + ": " + message);

        if (forceScroll)
        {
            selectedDebugMessage = debugMessages.Count - 1;
        }
    }

    void DrawDebug(int areaHeight, int margin)
    {
        if (debugMessages.Count > 0 && selectedDebugMessage >= 0)
        {
            GUILayout.BeginArea(new Rect(2 * margin, position.height - areaHeight + margin, position.width - 4 * margin, areaHeight));
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(debugMessages[selectedDebugMessage], GUILayout.Width(0.7f * (position.width - 4 * margin)), GUILayout.MaxWidth(position.width - 4 * margin - 40));

                    if (GUILayout.Button("+", GUILayout.Height(15)) && selectedDebugMessage < debugMessages.Count - 1)
                    {
                        selectedDebugMessage++;
                    }

                    if (GUILayout.Button("-", GUILayout.Height(15)) && selectedDebugMessage > 0)
                    {
                        selectedDebugMessage--;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
