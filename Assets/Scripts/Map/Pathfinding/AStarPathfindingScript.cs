using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfindingScript : MonoBehaviour
{
    private Dictionary<Vector2, List<Edge>> EdgeDefinition;

    [SerializeField]
    private Waypoint TestFrom;

    [SerializeField]
    private Waypoint TestTarget;

    [SerializeField]
    public Waypoint LastPlayerWaypoint;    
    public Edge CurrentEdge;
    
    public Vector2 PlayerWorldPosition;

    public void SetPlayerWaypoint(Waypoint playerPos) { LastPlayerWaypoint = playerPos; }
    public void SetPlayerPosition(Vector2 pos, Edge edge) { CurrentEdge = edge; PlayerWorldPosition = pos; }

    public void TestRun()
    {
        if (TestFrom != null && TestTarget != null)
        {
            float totalDistanceOfPath = 0;

            Debug.Log("Start pathfinding from " + TestFrom.WorldCoords + " to " + TestTarget.WorldCoords);
            List<Waypoint> res = CalculatePath(TestFrom, TestTarget, out totalDistanceOfPath);            

            Debug.Log("Path calculated; Path nodes: " + res.Count + ", Path Distance: " + totalDistanceOfPath);

            foreach(Waypoint w in res)
            {
                Debug.Log(w.transform.name);
            }
        }
        else
        {
            Debug.Log("Test failed: not assigned points");
        }
    }

    public void SetEdges(Dictionary<Vector2, List<Edge>> edges)
    {
        EdgeDefinition = edges;
    }
	
    public List<Waypoint> CalculatePath(Waypoint target)
    {
        float dump;

        return CalculatePath(target, out dump);
    }

    public List<Waypoint> CalculatePath(Waypoint target, out float totalDistance)
    {
        List<Waypoint> result;

        if (CurrentEdge != null)
        {
            int DistToA, DistToB;
            CurrentEdge.DistanceMidRoute(PlayerWorldPosition, out DistToA, out DistToB);

            float totalDistFromA, totalDistFromB;

            List<Waypoint> fromEndA = CalculatePath(CurrentEdge.GetWaypoint(CurrentEdge.A), out totalDistFromA);
            List<Waypoint> fromEndB = CalculatePath(CurrentEdge.GetWaypoint(CurrentEdge.B), out totalDistFromB);

            #region Sprawdzenie poprawności wyników i przygotowanie do wyboru
            if (totalDistFromA >= 0)
            {
                totalDistFromA += DistToA;
            }
            else
            {
                totalDistFromA = float.MaxValue;
            }

            if (totalDistFromB >= 0)
            {
                totalDistFromB += DistToB;
            }
            else
            {
                totalDistFromB = float.MaxValue;
            }
            #endregion

            if (totalDistFromA < totalDistFromB)
            {
                result = fromEndA;
                totalDistance = totalDistFromA;
            }
            else
            {
                if (totalDistFromA > totalDistFromB)
                {
                    result = fromEndB;
                    totalDistance = totalDistFromB;
                }
                else
                {
                    result = (DistToA < DistToB) ? fromEndA : fromEndB;
                    totalDistance =
                        ((DistToA < DistToB) ?
                            ((totalDistFromA < float.MaxValue) ? totalDistFromA : DistToA) :
                            ((totalDistFromB < float.MaxValue) ? totalDistFromB : DistToB)
                        );
                }
            }
        }
        else
        {
            result = CalculatePath(LastPlayerWaypoint, target, out totalDistance);
        }

        return result;
    }

    public List<Waypoint> CalculatePath(Waypoint from, Waypoint target, out float totalDistance)
    {
        List<Waypoint> result = CalculatePath(from, target);

        if (result.Count > 0)
        {
            totalDistance = 0;

            for (int i = 1; i < result.Count; i++)
            {
                Waypoint prev = result[i - 1];
                Waypoint curr = result[i];

                //Edge e = EdgeDefinition[prev.WorldCoords].Find(p => p.GetOppositeWaypoint(prev.WorldCoords).Equals(curr.WorldCoords));

                List<Edge> ed = EdgeDefinition[prev.WorldCoords];
                Edge e = ed.Find(p => p.GetOppositeWaypoint(prev.WorldCoords).Equals(curr));

                totalDistance += e.Distance(prev);
            }
        }
        else
        {
            totalDistance = -1;
        }

        return result;
    }

    public List<Waypoint> CalculatePath(Waypoint from, Waypoint target)
    {
        List<Waypoint> result = new List<Waypoint>();

        if (from.Discovered && target.Discovered)
        {
            AStarHeap Open = new AStarHeap();
            HashSet<AStarNodeScript> Closed = new HashSet<AStarNodeScript>();

            Open.Insert(new AStarNodeScript(from, target));

            while (!Open.Empty)
            {
                AStarNodeScript current = Open.GetMax();
                Closed.Add(current);

                if (current.H == 0)
                {
                    ProcessResultPath(ref result, current);
                    break;
                }
               
                List<Edge> edgesDefined = EdgeDefinition[current.WorldCoords];

                foreach (Edge e in edgesDefined)
                {
                    if (e.Discovered)
                    {
                        Waypoint other = e.GetOppositeWaypoint(current.WorldCoords);
                        AStarNodeScript neighbour =
                            new AStarNodeScript(other, target, current, current.G + e.Distance(current.WorldCoords));

                        if (Closed.Contains(neighbour)) continue;

                        if (!Open.Insert(neighbour))
                        {
                            Open.UpdateKey(neighbour);
                        }
                    }
                }
            }
        }

        return result;
    }

    void Awake()
    {
        if(LastPlayerWaypoint != null)
        {
            //PlayerWorldPosition = LastPlayerWaypoint.WorldCoords;
            SetPlayerPosition(LastPlayerWaypoint.WorldCoords, null);
        }
    }

    private void ProcessResultPath(ref List<Waypoint> result, AStarNodeScript finish)
    {
        if (result == null) result = new List<Waypoint>();

        AStarNodeScript Current = finish;

        while (Current != null)
        {
            result.Add(Current.Point);
            Current = Current.Parent;
        }

        result.Reverse();
    }
}
