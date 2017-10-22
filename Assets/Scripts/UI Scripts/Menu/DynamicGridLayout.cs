using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayout : MonoBehaviour
{
    [SerializeField]
    [Range(1, 20)]
    private int Columns;

    [SerializeField]
    [Range(0, 20)]
    private int Rows;

    [SerializeField]
    private int MinWidth;

    [SerializeField]
    private int MinHeight;

    private RectTransform Parent;
    private GridLayoutGroup Grid;

	// Use this for initialization
	void Awake ()
    {
        Parent = gameObject.GetComponent<RectTransform>();
        Grid = gameObject.GetComponent<GridLayoutGroup>();

        SetCells();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnValidate()
    {
        Awake();       
    }

    private void SetCells()
    {
        if (Grid != null)
        {                                     
            float parentWidth = Parent.rect.width - (Columns-1) * Grid.spacing.x - Grid.padding.left - Grid.padding.right;
            float parentHeight = Parent.rect.height - ((Grid.transform.childCount-1)/Columns) * Grid.spacing.y - Grid.padding.bottom - Grid.padding.top;

            float height =
                (Rows >= 1) ?
                    parentHeight / Rows :
                    parentHeight / (1 + (Grid.transform.childCount - 1) / Columns);

            height = Mathf.Max(height, MinHeight);

            Grid.cellSize = new Vector2(parentWidth / Columns, height);            
        }
    }
}
