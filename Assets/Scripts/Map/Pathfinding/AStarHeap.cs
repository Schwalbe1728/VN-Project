using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarHeap
{
    private AStarNodeScript[] heapArray;
    private int length;
    private HeapType type;

    private System.Random rand;
    private Dictionary<Vector2, int> NodeToIndex;

    public AStarHeap(HeapType _type = HeapType.Min)
    {
        heapArray = new AStarNodeScript[2];
        length = 0;
        type = _type;

        rand = new System.Random();
        NodeToIndex = new Dictionary<Vector2, int>();
    }

    public bool Empty { get { return length == 0; } }

    public AStarNodeScript PeekMax { get { return heapArray[1]; } }

    public AStarNodeScript GetMax()
    {

        {
            if (length == 0) return null;

            AStarNodeScript result = PeekMax;

            Swap(1, length);
            length--;
            if (length > 1)
            {
                CorrectDownwards(1);
                //NodeToIndex[(GraphNode)heapArray[1]] = 1;
            }

            if (length < heapArray.Length / 2)
            {
                Array.Resize<AStarNodeScript>(ref heapArray, heapArray.Length / 2);
            }

            NodeToIndex.Remove(result.WorldCoords);

            return result;
        }
    }

    public void UpdateKey(AStarNodeScript node)
    {
        int index = NodeToIndex[node.WorldCoords];

        if (CompareNodes(heapArray[index], node) == 1)
        {
            heapArray[index] = node;

            CorrectUpwards(index);
        }
    }

    public bool Insert(AStarNodeScript node)
    {
        //sprawdzenie przynależności?
        if (!Contains(node))
        {
            length++;

            if (length == heapArray.Length)
            {
                Array.Resize<AStarNodeScript>(ref heapArray, heapArray.Length * 2);
            }

            heapArray[length] = node;
            NodeToIndex.Add(node.WorldCoords, length);
            CorrectUpwards(length);

            return true;
        }

        return false;
    }

    private bool Contains(AStarNodeScript node)
    {
        return NodeToIndex.ContainsKey(node.WorldCoords);
    }

    private void CorrectUpwards(int index)
    {
        if (!IsCorrectOrder(index / 2, index))
        {
            Swap(index, index / 2);
            //NodeToIndex[(GraphNode)heapArray[index / 2]] = index / 2;
            CorrectUpwards(index / 2);
        }
    }

    private void CorrectDownwards(int index)
    {
        int childIndex = index * 2;

        if (childIndex <= length && index > 0)
        {
            int childMin = childIndex;

            if (childIndex + 1 <= length && CompareNodes(childIndex, childIndex + 1) == 1)
            {
                childMin = childIndex + 1;
            }

            if (CompareNodes(index, childMin) == 1)
            {
                Swap(index, childMin);
                //NodeToIndex[(GraphNode)heapArray[childMin]] = childMin;
                CorrectDownwards(childMin);
            }
        }
    }

    private bool IsCorrectOrder(int indexParent, int indexChild)
    {
        bool isChild = indexChild / 2 == indexParent;

        return
            indexParent == 0 ||
            !isChild ||
            CompareNodes(heapArray[indexParent], heapArray[indexChild]) == -1;

    }

    private int CompareNodes(int A, int B) { return CompareNodes(heapArray[A], heapArray[B]); }
    private int CompareNodes(AStarNodeScript A, AStarNodeScript B)
    {
        bool isEqual = A.F /*+ A.MovementPenalty*/ == B.F /*+ B.MovementPenalty*/;

        return
        (isEqual) ?
                    (
                        (type == HeapType.Min) ?
                            ((A.H < B.H) ?
                                -1 :
                                ((A.H > B.H) ? 1 : -1 + 2 * rand.Next(2))
                                ) :
                            ((A.H > B.H) ?
                                -1 :
                                ((A.H < B.H) ? 1 : -1 + 2 * rand.Next(2))
                                )
                    )
                    :
                    (
                        (type == HeapType.Min) ?
                            ((A.F < B.F) ? -1 : 1) :
                            ((A.F > B.F) ? -1 : 1)
                    );
    }

    private void Swap(int indexA, int indexB)
    {
        AStarNodeScript A = heapArray[indexA];
        AStarNodeScript B = heapArray[indexB];

        heapArray[indexA] = B;
        heapArray[indexB] = A;

        NodeToIndex[B.WorldCoords] = indexA;
        NodeToIndex[A.WorldCoords] = indexB;
    }
}

public enum HeapType
{
    Min,
    Max
}