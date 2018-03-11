using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEditorInfo
{
    public List<Rect> Windows;
    public List<NodeType> WindowTypes;
    public List<int> NodeTypesIDs;
    public List<int> OptionsIndexes;
    public List<int> NodesIndexes;
    public List<int> ConditionsIndexes;
    public int Nodes;
    public int Options;
    public int Conditions;
    public Dictionary<int, Dictionary<int, bool>> NodesOptionsFoldouts;

    public DialogueEditorInfo()
    {
        Windows = new List<Rect>();
        WindowTypes = new List<NodeType>();
        NodeTypesIDs = new List<int>();
        OptionsIndexes = new List<int>();
        NodesIndexes = new List<int>();
        ConditionsIndexes = new List<int>();
        Nodes = 0;
        Options = 0;
        Conditions = 0;
        NodesOptionsFoldouts = new Dictionary<int, Dictionary<int, bool>>();
    }

    public void RestoreFoldouts(DialogueNode[] nodes)
    {
        NodesOptionsFoldouts.Clear();

        foreach (DialogueNode node in nodes)
        {
            if (!node.ImmediateNode && node.OptionsAttached != null)
            {
                NodesOptionsFoldouts.Add(node.NodeID, new Dictionary<int, bool>());

                foreach (int option in node.OptionsAttached)
                {
                    NodesOptionsFoldouts[node.NodeID].Add(option, false);
                }
            }
        }
    }

}
