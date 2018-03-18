using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class DialogueEditor
{
    private List<int> nodeToNodeToAttach = new List<int>();                 //Immediate node -> Dialogue Node

    private List<int> nodeToOptionToAttach = new List<int>();               //Dialogue Node -> Option
    private List<int> optionToNodeToAttach = new List<int>();               //Option -> Dialogue Node

    private List<int> nodeToConditionToAttach = new List<int>();            //Immediate node -> Condition
    private List<int> optionToConditionToAttach = new List<int>();          //Option -> Condition
    private List<int> conditionSuccessToConditionToAttach = new List<int>();   //Condition -> [Condition Success] -> Condition
    private List<int> conditionSuccessToNodeToAttach = new List<int>();     //Condition -> [Condition Success] -> Node
    private List<int> conditionFailToConditionToAttach = new List<int>();   //Condition -> [Condition Failure] -> Condition
    private List<int> conditionFailToNodeToAttach = new List<int>();        //Condition -> [Condition Failure] -> Node
    private List<int> conditionToEntryOption = new List<int>();             //Condition -> Entry Condition value of Option    

    #region Connections Management

    void ClearAllConnectionsPending()
    {
        ClearImmediateConnectionsPending();
        ClearOptionConnectionsPending();        
        ClearConditionConnectionsPending();
    }

    void ClearImmediateConnectionsPending()
    {
        nodeToNodeToAttach.Clear();
        nodeToConditionToAttach.Clear();
    }

    void ClearOptionConnectionsPending()
    {
        nodeToOptionToAttach.Clear();
        optionToNodeToAttach.Clear();
        optionToConditionToAttach.Clear();
    }

    void ClearConditionConnectionsPending()
    {
        conditionSuccessToNodeToAttach.Clear();
        conditionSuccessToConditionToAttach.Clear();
        conditionFailToNodeToAttach.Clear();
        conditionFailToConditionToAttach.Clear();
        conditionToEntryOption.Clear();
    }

    void MakeImmediateNodeConnection(int idFrom, int idTo, NodeType targetType)
    {
        DialogueNode node = CurrentNodes[idFrom];
        node.MakeImmediateNode();
        if (targetType == NodeType.Exit)
        {
            node.SetImmediateNodeTargetExit();
        }
        else
        {
            if (targetType != NodeType.Option)
            {
                node.SetImmediateNodeTarget(idTo, targetType);
            }
        }

        ClearAllConnectionsPending();
    }

    void MakeNodeToOptionConnection(int idFrom, int idTo, NodeType targetType)
    {
        if (targetType == NodeType.Option)
        {
            DialogueNode node = CurrentNodes[idFrom];
            List<int> optionsAttachedToNode = new List<int>(node.OptionsAttached);

            if (!EditorInfo.NodesOptionsFoldouts.ContainsKey(node.NodeID))
            {
                EditorInfo.NodesOptionsFoldouts.Add(node.NodeID, new Dictionary<int, bool>());
            }

            if (!optionsAttachedToNode.Contains(idTo))
            {
                optionsAttachedToNode.Add(idTo);
                node.OptionsAttached = optionsAttachedToNode.ToArray();

                EditorInfo.NodesOptionsFoldouts[node.NodeID].Add(idTo, false);
            }
        }
        else
        {
            if(targetType == NodeType.Node)
            {
                DialogueOption option = CurrentOptions[idFrom];

                if (idTo != Dialogue.ExitDialogue)
                {
                    option.SetNext(idTo, NodeType.Node);
                }
                else
                {
                    option.SetNextNodeExit();
                }
            }
        }

        ClearAllConnectionsPending();
    }

    void MakeConditionNodeConnection(int idFrom, int idTo, NodeType targetType, bool isSuccess)
    {
        ConditionNode nodeFrom = CurrentConditions[idFrom];

        if (targetType == NodeType.Option)
        {
            DialogueOption optionTo = CurrentOptions[idTo];
            optionTo.SetEntryCondition(nodeFrom);
        }
        else
        {
            if (isSuccess)
            {
                nodeFrom.SetSuccessTarget(idTo, targetType);
            }
            else
            {
                nodeFrom.SetFailureTarget(idTo, targetType);
            }
        }

        ClearAllConnectionsPending();
    }

    #endregion

    #region Drawing
    void DrawNodeCurve(Rect start, Rect end)
    {
        DrawNodeCurve(start, end, Color.black);
    }

    void DrawNodeCurve(Rect start, Rect end, Color curveColor)
    {
        Vector3 startPos;// = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos;// = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 tanStartMod;
        Vector3 tanEndMod;
        AlignCurve(start, end, out startPos, out endPos, out tanStartMod, out tanEndMod);

        Vector3 startTan = startPos + tanStartMod * 50;
        Vector3 endTan = endPos + tanEndMod * 50;

        float arrowSize = 10f;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 2);
        Handles.color = curveColor;
        Handles.DrawSolidArc(endPos, Vector3.back, Quaternion.AngleAxis(22.5f, Vector3.forward) * tanEndMod, 45f, arrowSize);
    }

    void AlignCurve(Rect start, Rect end, out Vector3 startPos, out Vector3 endPos, out Vector3 startDir, out Vector3 endDir)
    {
        //TODO: zamienić na lepsze wykrywanie punktów na podstawie wzajemnego położenia
        //to jest rozwiązanie na pałę i na szybko

        List<Vector2> startRectPotentials = new List<Vector2>();

        if (Config.DiagonalStartPoints)
        {
            startRectPotentials.Add(start.min);
            startRectPotentials.Add(new Vector2(start.x, start.y + start.height));
            startRectPotentials.Add(new Vector2(start.x + start.width, start.y));
            startRectPotentials.Add(start.max);
        }
        startRectPotentials.Add(new Vector2(start.x, start.center.y));
        startRectPotentials.Add(new Vector2(start.x + start.width, start.center.y));
        startRectPotentials.Add(new Vector2(start.center.x, start.y + start.height));
        startRectPotentials.Add(new Vector2(start.center.x, start.y));

        List<Vector2> endRectPotentials = new List<Vector2>();

        if (Config.DiagonalEndPoints)
        {
            endRectPotentials.Add(end.min);
            endRectPotentials.Add(new Vector2(end.x, end.y + end.height));
            endRectPotentials.Add(new Vector2(end.x + end.width, end.y));
            endRectPotentials.Add(end.max);
        }
        endRectPotentials.Add(new Vector2(end.x, end.center.y));
        endRectPotentials.Add(new Vector2(end.x + end.width, end.center.y));
        endRectPotentials.Add(new Vector2(end.center.x, end.y + end.height));
        endRectPotentials.Add(new Vector2(end.center.x, end.y));


        int startPotCount = startRectPotentials.Count;
        int endPotCount = endRectPotentials.Count;

        float[,] distances = new float[startPotCount, endPotCount];
        float minDist = float.MaxValue;
        int minDistX = 999;
        int minDistY = 999;

        float curveModifier = 1f;

        for (int x = 0; x < startPotCount; x++)
        {
            for (int y = 0; y < endPotCount; y++)
            {
                distances[x, y] =
                    Vector2.Distance(startRectPotentials[x], endRectPotentials[y]);

                if (minDist > curveModifier * distances[x, y])
                {
                    minDist = distances[x, y];
                    minDistX = x;
                    minDistY = y;
                }
            }
        }

        Vector2 startChosen = startRectPotentials[minDistX];
        Vector2 endChosen = endRectPotentials[minDistY];

        startPos = new Vector3(startChosen.x, startChosen.y);
        endPos = new Vector3(endChosen.x, endChosen.y);

        startChosen -= start.center;
        endChosen -= end.center;

        startDir = new Vector3(startChosen.x, startChosen.y);
        endDir = new Vector3(endChosen.x, endChosen.y);

        startDir.Normalize();
        endDir.Normalize();
    }
    #endregion

    void UpdateCurves()
    {
        Event currentEvent = Event.current;
        Rect cursorRect = new Rect(currentEvent.mousePosition, new Vector2(5, 5));

        foreach (Rect win in EditorInfo.Windows)
        {
            if (win.Contains(currentEvent.mousePosition))
            {
                cursorRect = win;
            }
        }

        bool repaint = false;
        bool save = false;

        #region Immediate Node connections creation
        if (nodeToNodeToAttach.Count == 1)
        {
            int from = EditorInfo.NodesIndexes[nodeToNodeToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.ImmidiateNodeConnection);
            repaint = true;
        }
        else
        {
            if (nodeToNodeToAttach.Count == 2)
            {
                MakeImmediateNodeConnection(
                    nodeToNodeToAttach[0], 
                    nodeToNodeToAttach[1], 
                    (nodeToNodeToAttach[1] == Dialogue.ExitDialogue)? 
                        NodeType.Exit : NodeType.Node);
                save = true;
            }
        }
        #endregion
        #region Node To Option Connections creation
        if (nodeToOptionToAttach.Count == 1)
        {
            int from = EditorInfo.NodesIndexes[nodeToOptionToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.NodeToOptionConnection);
            repaint = true;
        }
        else
        {
            if (nodeToOptionToAttach.Count == 2)
            {
                MakeNodeToOptionConnection(nodeToOptionToAttach[0], nodeToOptionToAttach[1], NodeType.Option);
                save = true;
            }
        }
        #endregion
        #region Option To Node Connection creation
        if (optionToNodeToAttach.Count == 1)
        {
            int from = EditorInfo.OptionsIndexes[optionToNodeToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.OptionToNodeConnection);
            repaint = true;
        }
        else
        {
            if (optionToNodeToAttach.Count == 2)
            {
                MakeNodeToOptionConnection(optionToNodeToAttach[0], optionToNodeToAttach[1], NodeType.Node);
                save = true;
            }
        }
        #endregion

        #region Conditions Connections
        #region Immediate Node -> Condition

        if (nodeToConditionToAttach.Count == 1)
        {
            int from = EditorInfo.NodesIndexes[nodeToConditionToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.ToConditionConnection);
            repaint = true;
        }
        else
        {
            if (nodeToConditionToAttach.Count == 2)
            {
                MakeImmediateNodeConnection(
                    nodeToConditionToAttach[0], 
                    nodeToConditionToAttach[1], 
                    (nodeToConditionToAttach[1] == Dialogue.ExitDialogue)? NodeType.Exit : NodeType.Condition
                    );

                save = true;
            }
        }

        #region Option -> Condtion
        if (optionToConditionToAttach.Count == 1)
        {
            int from = EditorInfo.OptionsIndexes[optionToConditionToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.ToConditionConnection);
            repaint = true;
        }
        else
        {
            if (optionToConditionToAttach.Count == 2)
            {
                DialogueOption optionFrom = CurrentOptions[optionToConditionToAttach[0]];
                optionFrom.SetNext(optionToConditionToAttach[1], NodeType.Condition);

                optionToConditionToAttach.Clear();
                optionToNodeToAttach.Clear();
            }
        }
        #endregion

        #endregion
        #region Condition -> [Success] -> Condition

        if (conditionSuccessToConditionToAttach.Count == 1)
        {
            int from = EditorInfo.ConditionsIndexes[conditionSuccessToConditionToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.FromSuccesConnection);
            repaint = true;
        }
        else
        {
            if (conditionSuccessToConditionToAttach.Count == 2)
            {
                MakeConditionNodeConnection(
                    conditionSuccessToConditionToAttach[0],
                    conditionSuccessToConditionToAttach[1],
                    NodeType.Condition,
                    true                    
                    );
                save = true;
            }
        }

        #endregion
        #region Condition -> [Success] -> Node

        if (conditionSuccessToNodeToAttach.Count == 1)
        {
            int from = EditorInfo.ConditionsIndexes[conditionSuccessToNodeToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.FromSuccesConnection);
            repaint = true;
        }
        else
        {
            if (conditionSuccessToNodeToAttach.Count == 2)
            {
                MakeConditionNodeConnection(
                    conditionSuccessToNodeToAttach[0],
                    conditionSuccessToNodeToAttach[1],
                    NodeType.Node,
                    true
                    );
                save = true;
            }
        }

        #endregion
        #region Condition -> [Failure] -> Condition

        if (conditionFailToConditionToAttach.Count == 1)
        {
            int from = EditorInfo.ConditionsIndexes[conditionFailToConditionToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.FromFailureConnection);
            repaint = true;
        }
        else
        {
            if (conditionFailToConditionToAttach.Count == 2)
            {
                MakeConditionNodeConnection(
                    conditionFailToConditionToAttach[0],
                    conditionFailToConditionToAttach[1],
                    NodeType.Condition,
                    false
                    );
                save = true;
            }
        }

        #endregion
        #region Condition -> [Failure] -> Node

        if (conditionFailToNodeToAttach.Count == 1)
        {
            int from = EditorInfo.ConditionsIndexes[conditionFailToNodeToAttach[0]];

            DrawNodeCurve(EditorInfo.Windows[from], cursorRect, Config.FromFailureConnection);
            repaint = true;
        }
        else
        {
            if (conditionFailToNodeToAttach.Count == 2)
            {
                MakeConditionNodeConnection(
                    conditionFailToNodeToAttach[0],
                    conditionFailToNodeToAttach[1],
                    NodeType.Node,
                    false
                    );
                save = true;
            }
        }

        #endregion
        #region Condition -> Entry Option

        if (conditionToEntryOption.Count == 1)
        {
            int to = EditorInfo.ConditionsIndexes[conditionToEntryOption[0]];

            DrawNodeCurve(cursorRect, EditorInfo.Windows[to], Config.EntryConditionConnection);
            repaint = true;
        }
        else
        {
            if (conditionToEntryOption.Count == 2)
            {
                MakeConditionNodeConnection(conditionToEntryOption[0], conditionToEntryOption[1], NodeType.Option, false);
                save = true;
            }
        }

        #endregion
        #endregion

        #region Drawing established connections TOFINISH
        foreach (DialogueNode nodeFrom in CurrentNodes)
        {
            int from = EditorInfo.NodesIndexes[nodeFrom.NodeID];

            if (nodeFrom.ImmediateNode)
            {
                int targID;
                NodeType targType;

                nodeFrom.GetTarget(out targID, out targType);

                if (targID == Dialogue.ExitDialogue) continue;

                if (targType == NodeType.Node)
                {
                    int to = EditorInfo.NodesIndexes[targID];
                    DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.ImmidiateNodeConnection);
                }

                if (targType == NodeType.Condition)
                {
                    int to = EditorInfo.ConditionsIndexes[targID];
                    DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.ToConditionConnection);
                }
            }
            else
            {
                foreach (int optInd in nodeFrom.OptionsAttached)
                {
                    int to = EditorInfo.OptionsIndexes[optInd];
                    DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.NodeToOptionConnection);
                }
            }
        }

        foreach (DialogueOption optionFrom in CurrentOptions)
        {
            int from = EditorInfo.OptionsIndexes[optionFrom.OptionID];

            if (optionFrom.EntryConditionSet)
            {
                int entryCondition = EditorInfo.ConditionsIndexes[optionFrom.EntryCondition.ConditionID];
                DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[entryCondition], Config.EntryConditionConnection);
            }

            int to = -1;

            switch (optionFrom.NextType)
            {
                case NodeType.Node:
                    to = EditorInfo.NodesIndexes[optionFrom.NextID];
                    DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.OptionToNodeConnection);
                    break;

                case NodeType.Condition:
                    to = EditorInfo.ConditionsIndexes[optionFrom.NextID];
                    DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.ToConditionConnection);
                    break;
            }
        }

        foreach (ConditionNode conditionFrom in CurrentConditions)
        {
            int from = EditorInfo.ConditionsIndexes[conditionFrom.ConditionID];
            int to = Dialogue.ExitDialogue;

            switch (conditionFrom.SuccessTargetType)
            {
                case NodeType.Condition:
                    to = EditorInfo.ConditionsIndexes[conditionFrom.SuccessTarget];
                    break;

                case NodeType.Node:
                    to = EditorInfo.NodesIndexes[conditionFrom.SuccessTarget];
                    break;
            }

            if (to != Dialogue.ExitDialogue)
            {
                DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.FromSuccesConnection);
            }

            to = Dialogue.ExitDialogue;

            switch (conditionFrom.FailureTargetType)
            {
                case NodeType.Condition:
                    to = EditorInfo.ConditionsIndexes[conditionFrom.FailureTarget];
                    break;

                case NodeType.Node:
                    to = EditorInfo.NodesIndexes[conditionFrom.FailureTarget];
                    break;
            }

            if (to != Dialogue.ExitDialogue)
            {
                DrawNodeCurve(EditorInfo.Windows[from], EditorInfo.Windows[to], Config.FromFailureConnection);
            }
        }

        #endregion

        if (repaint)
        {
            Repaint();
        }

        if (save)
        {
            SaveChanges("Update Curves");
        }
    }
}
