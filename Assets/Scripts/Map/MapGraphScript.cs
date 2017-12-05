using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraphScript : MonoBehaviour
{
    private TimeManagerScript TimeManager;
    private RectTransform MapDisplay;
    private Dictionary<Vector2, List<Edge>> Edges;
    private AStarPathfindingScript Pathfinder;

    private CharacterInfoScript PlayerStats;

    [Range(0.00001f, float.MaxValue)]
    [SerializeField]
    private float CoordToMeter;

    [SerializeField]
    private Transform EdgesSource;

    [SerializeField]
    private Vector2 CurrentDisplayCoords;

    [SerializeField]
    private RectTransform PlayerPositionSprite;

    private Waypoint NextWaypoint;
    private float metersOfEdgeTraversed;
    private Coroutine RoutingCoroutine;

    public int VertexDegree(Waypoint vertex)
    {
        //return (Edges.ContainsKey(vertex.WorldCoords))? Edges[vertex.WorldCoords].Count : -1;
        return VertexDegree(vertex.WorldCoords);
    }

    public int VertexDegree(Vector2 vertex)
    {
        return (Edges.ContainsKey(vertex)) ? Edges[vertex].Count : -1;
    }

    public void Entered(Waypoint vertex)
    {
        //vertex.Visit();        
        //Debug.Log("Entering " + vertex.WorldCoords);

        foreach(Edge e in Edges[vertex.WorldCoords])
        {
            e.Discover();
        }

        CenterOnPlayerPosition(Pathfinder.LastPlayerWaypoint);
    }

    public void GoTo(Waypoint target)
    {
        if (RoutingCoroutine == null)
        {
            float dist;
            List<Waypoint> path = Pathfinder.CalculatePath(target, out dist);

            //Debug.Log("Traversing path with distance " + dist);

            RoutingCoroutine = StartCoroutine(TraversePath(path));
        }
        else
        {
            /*
            StopCoroutine(RoutingCoroutine);
            RoutingCoroutine = null;
            GoTo(target);
            */
        }
    }

    public IEnumerator TraversePath(List<Waypoint> waypoints)
    {
        Debug.Log("Traverse Path");
        TimeManager.AccelerateTime(100);
        TimeManager.OnSecondPassed += OnSecondPassed;  

        foreach(Waypoint vertex in waypoints)
        {
            NextWaypoint = vertex;

            //Debug.Log("Czekam");
            //Debug.Log(Vector2.Distance(Pathfinder.PlayerWorldPosition, NextWaypoint.WorldCoords));

            yield return 
                new WaitWhile(
                    () => Vector2.Distance(Pathfinder.PlayerWorldPosition, NextWaypoint.WorldCoords) > 1f
                    );

            Pathfinder.SetPlayerWaypoint(vertex);
            Pathfinder.CurrentEdge = null;
            Entered(vertex);                        
            //Debug.Log("Name: " + vertex.transform.name + ", Degree: " + VertexDegree(vertex));
            //TODO: Player porusza się po mapie z prędkością i w czasie
        }
        NextWaypoint = null;
        TimeManager.OnSecondPassed -= OnSecondPassed;
        TimeManager.RestoreTimeFlow();
        RoutingCoroutine = null;       
    }

    void Start()
    {
        MapDisplay = GetComponent<RectTransform>();
        TimeManager = GameObject.Find("Time Manager").GetComponent<TimeManagerScript>();

        Edges = new Dictionary<Vector2, List<Edge>>();
        Edge[] edges = EdgesSource.GetComponentsInChildren<Edge>();

        foreach(Edge e in edges)
        {            
            if (!Edges.ContainsKey(e.A))
            {
                e.SetMap(this);
                Edges.Add(e.A, new List<Edge>());
            }

            if (!Edges.ContainsKey(e.B))
            {
                e.SetMap(this);
                Edges.Add(e.B, new List<Edge>());
            }

            Edges[e.A].Add(e);
            Edges[e.B].Add(e);

            /*
            int totalEdges = 0;

            foreach(KeyValuePair<Vector2, List<Edge>> pair in Edges)
            {
                totalEdges += pair.Value.Count;
            }

            Debug.Log("Dodaje do mapy krawędź: " + e.transform.name + " From: " + e.From + " To: " + e.To + ", Vertex Count: " + Edges.Count + " Edge Count: " + totalEdges);
            */
        }

        Pathfinder = GetComponent<AStarPathfindingScript>();
        Pathfinder.SetEdges(Edges);

        if(Pathfinder.LastPlayerWaypoint != null)
        {
            Entered(Pathfinder.LastPlayerWaypoint);
            Pathfinder.PlayerWorldPosition = Pathfinder.LastPlayerWaypoint.WorldCoords;
        }

        PlayerStats = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();
    }

    void Update()
    {
        if (NextWaypoint != null && !NextWaypoint.Equals(Pathfinder.LastPlayerWaypoint))
        {
            if (Pathfinder.CurrentEdge == null)
            {
                Pathfinder.CurrentEdge =
                  Edges[Pathfinder.LastPlayerWaypoint.WorldCoords].
                      Find(w => w.GetOppositeWaypoint(Pathfinder.LastPlayerWaypoint.WorldCoords).Equals(NextWaypoint));

                metersOfEdgeTraversed -= Pathfinder.CurrentEdge.Distance(Pathfinder.LastPlayerWaypoint.WorldCoords);
            }

            //Debug.Log(metersOfEdgeTraversed);

            Pathfinder.
                PlayerWorldPosition =
                    Vector2.Lerp(
                        Pathfinder.LastPlayerWaypoint.WorldCoords,
                        NextWaypoint.WorldCoords,
                        1 + Mathf.Min(metersOfEdgeTraversed / Pathfinder.CurrentEdge.Distance(Pathfinder.LastPlayerWaypoint), 0)
                    );


            CenterOnPlayerPosition(
                Waypoint.LerpDisplayCoords(
                    Pathfinder.LastPlayerWaypoint, 
                    NextWaypoint, 
                    1 + Mathf.Min(metersOfEdgeTraversed / Pathfinder.CurrentEdge.Distance(Pathfinder.LastPlayerWaypoint), 0)
                    )
            );
        }
    }

    void OnValidate()
    {
        Waypoint.SetScaleOfCoordinates(CoordToMeter);
    }

    private void CenterOnPlayerPosition(Waypoint point)
    {
        CenterOnPlayerPosition(point.WorldCoords);
        //CurrentDisplayCoords = point.DisplayCoords;
        //PlayerPositionSprite.anchoredPosition = CurrentDisplayCoords;
        //MapDisplay.anchoredPosition = -CurrentDisplayCoords;
    }

    private void CenterOnPlayerPosition(Vector2 point)
    {
        CurrentDisplayCoords = point;
        PlayerPositionSprite.anchoredPosition = CurrentDisplayCoords;
        MapDisplay.anchoredPosition = -CurrentDisplayCoords;
    }

    private void OnSecondPassed()
    {
        float metersPerHour = StatsRuleSet.WalkSpeedPerHour(PlayerStats);
        float metersPerSecond = metersPerHour / 3600;

        metersOfEdgeTraversed += metersPerSecond;

    }
}
