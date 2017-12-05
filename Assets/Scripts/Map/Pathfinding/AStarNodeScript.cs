using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNodeScript
{
    private Waypoint point;
    private AStarNodeScript parent;

    private float h;
    private float g;

    public AStarNodeScript(Waypoint point, Waypoint target, AStarNodeScript _parent = null, float _g = 0)
    {
        this.point = point;

        h = Vector2.Distance(point.WorldCoords, target.WorldCoords);
        g = _g;

        parent = _parent;
    }

    public Vector2 WorldCoords { get { return point.WorldCoords; } }
    public float F { get { return g + h; } }
    public float G { get { return g; } }
    public float H { get { return h; } } 
    
    public AStarNodeScript Parent { get { return parent; } }
    public Waypoint Point { get { return point; } }
}
