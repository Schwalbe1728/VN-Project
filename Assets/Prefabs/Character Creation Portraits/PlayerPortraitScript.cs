using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Portrait", menuName = "VN-Project/Player Portrait")]
public class PlayerPortraitScript : ScriptableObject
{
    [SerializeField]   
    private Sprite portrait;
	
    public Sprite Portrait { get { return portrait; } }
}
