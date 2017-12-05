using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public static float MinHeight = 0;
    public static float MaxHeight = 10;

    private static float CoordToMeter = 1;
    public static void SetScaleOfCoordinates(float val) { CoordToMeter = val; }


    public bool Discovered { get { return discovered; } }
    public int Index { get { return VertexIndex; } }    

    public Vector2 WorldCoords { get { return worldCoords; } }
    public float HeightPosition { get { return heightPosition; } }

    [SerializeField]
    private bool discovered;

    [SerializeField]
    private Vector2 worldCoords;
    [SerializeField]
    [Range(0, 10)]
    private float heightPosition;

    private RectTransform displayTransform;
    private int VertexIndex;

    public Vector2 DisplayCoords { get { return (displayTransform != null)? displayTransform.anchoredPosition : Vector2.zero; } }

    protected MapGraphScript Map;

    public static Vector2 LerpDisplayCoords(Waypoint From, Waypoint To, float delta)
    {
        return Vector2.Lerp(From.DisplayCoords, To.DisplayCoords, delta);
    }

    public void Discover()
    {
        discovered = true;
    }

    public virtual void Visit()
    {
        Discover();
        
        /*
        if(Map != null)
        {
            Map.Entered(this);
        }
        */
    }

    public void SetMap(MapGraphScript map) { Map = map; }

    void Awake()
    {
        // oblicz WorldCoords na podstawie displayTransform
        displayTransform = gameObject.GetComponent<RectTransform>();
        worldCoords = DisplayCoords / CoordToMeter;

        //Debug.Log(DisplayCoords);
    }

    void OnValidate()
    {
        worldCoords = DisplayCoords / CoordToMeter;
    }
}
