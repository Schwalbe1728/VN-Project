using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MapPoint : Waypoint {

    public bool Visited { get { return visited; } }

    [SerializeField]
    private string NotVisitedName;

    [SerializeField]
    private string VisitedName;    

    [SerializeField]  
    private Text DisplayText;

    private bool visited;

    public override void Visit()
    {
        visited = true;
        DisplayText.text = VisitedName;

        Discover();        
    }

    public void ToggleClicked(bool value)
    {
        //Visit();

        if (Map != null)
        {
            //Map.Entered(this);
            Map.GoTo(this);
        }
        else
        {
            Debug.Log("Chuj");
        }
    }

    void Start()
    {
        //DisplayText = GetComponent<Text>();
        DisplayText.text = NotVisitedName;
    }   
}
