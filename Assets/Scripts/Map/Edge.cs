using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Edge : MonoBehaviour {

    [SerializeField]
    private Waypoint pointA;

    [SerializeField]
    private Waypoint pointB;

    [Range(1, float.MaxValue)]
    [SerializeField]
    private float TerrainMod = 1;
    //private float AtoBTerrain;

    //[SerializeField]
    //private float BtoATerrain;


    private UILineRenderer lineRenderer;

    private static Color lineColor = Color.white * 0.5f;
    private static Color partOfThePathColor = Color.red;

    public int Distance(Waypoint from)
    {
        return (from.Equals(pointA)) ? DistanceAToB : DistanceBToA;
    }

    public int Distance(Vector2 from)
    {
        return (from.Equals(pointA.WorldCoords)) ? DistanceAToB : DistanceBToA;
    }

    public void DistanceMidRoute(Vector2 midpoint, out int ToA, out int ToB)
    {
        float distToA = Vector2.Distance(midpoint, A) * TerrainMod * HeightDifferenceModificator(pointB.HeightPosition, pointA.HeightPosition); //BtoATerrain;
        float distToB = Vector2.Distance(midpoint, B) * TerrainMod * HeightDifferenceModificator(pointA.HeightPosition, pointB.HeightPosition); //AtoBTerrain;

        ToA = Mathf.RoundToInt(distToA);
        ToB = Mathf.RoundToInt(distToB);
    }

    public int DistanceAToB
    {
        get
        {
            return Mathf.RoundToInt((RawDistance * TerrainMod * HeightDifferenceModificator(pointA.HeightPosition, pointB.HeightPosition)  ));
        }
    }

    public int DistanceBToA
    {
        get
        {
            return Mathf.RoundToInt((RawDistance * TerrainMod * HeightDifferenceModificator(pointB.HeightPosition, pointA.HeightPosition)));
        }
    }

    public int RawDistance
    {
        get
        {
            return Mathf.RoundToInt(Vector2.Distance(A, B));
        }
    }

    public bool Discovered
    {
        get
        {
            return pointA.Discovered && pointB.Discovered;
        }
    }

    public Vector2 A { get { return pointA.WorldCoords; } }
    public Vector2 B { get { return pointB.WorldCoords; } }

    public void Discover()
    {
        pointA.Discover();
        pointB.Discover();

        lineRenderer.enabled = true;
    }

    public void SetMap(MapGraphScript map)
    {
        //Debug.Log("Ustawiam mapę krawędzi: " + transform.name);

        pointA.SetMap(map);
        pointB.SetMap(map);
        //to.SetMap(map);
    }

    public Waypoint GetWaypoint(Vector2 coords)
    {
        return (coords.Equals(A)) ? pointA : ((coords.Equals(B)) ? pointB : null);
    }

    public Waypoint GetOppositeWaypoint(Vector2 coords)
    {
        return (coords.Equals(A)) ? pointB : ((coords.Equals(B)) ? pointA : null);
    }
    
    /// <summary>
    /// Returns 0 if A is higher than B (because you get no penalty for going from higher to lower ground)
    /// </summary>
    /// <param name="HeightFrom"></param>
    /// <param name="HeightTo"></param>
    /// <returns></returns>
    private float HeightDifferenceModificator(float HeightFrom, float HeightTo)
    {
        float maxPossibleDiff = Waypoint.MaxHeight - Waypoint.MinHeight;
        float diff = HeightTo - HeightFrom;

        return (maxPossibleDiff > 0) ? 
                    1 + 
                    2 * Mathf.Max( diff / maxPossibleDiff, 0 ) * Mathf.Pow(2, ((1000 - RawDistance)/1000.0f)) : 1;
    }   

    void Awake()
    {
        lineRenderer = GetComponent<UILineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.Points = new Vector2[2] { pointA.DisplayCoords, pointB.DisplayCoords };
            lineRenderer.color = lineColor;
            lineRenderer.enabled = Discovered;            
        }
    }

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(transform.name + " RawDistance: " + RawDistance +", Distance A to B: " + DistanceAToB + ", Distance B to A: " + DistanceBToA);
        }
        

        if(Discovered)
        {
            lineRenderer.enabled = true;
        }
    }
    /*
    void OnPostRender()
    {
        Vector3 mainPointPos = from.transform.position;
        Vector3 pointPos = to.transform.position;

        GL.Begin(GL.LINES);
        //lineMat.SetPass(0);
        GL.Color(lineColor);
        GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
        GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
        GL.End();

        Debug.Log(mainPointPos);
    }*/
}
