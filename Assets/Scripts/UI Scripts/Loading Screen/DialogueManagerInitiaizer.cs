using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerInitiaizer : MonoBehaviour
{
    [SerializeField]
    private TextAsset PathsFile;

    [SerializeField]
    private string PathBase;

    void Awake()
    {
        DialoguesManagerScript.Initiate();
        StartCoroutine(DialoguesManagerScript.Load(PathsFile, PathBase));
    }
}
