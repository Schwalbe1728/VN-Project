using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SpriteElement
{
    [SerializeField]
    private int targetID;

    public int TargetID { get { return targetID; } }
    public Sprite TargetSprite;
}

[Serializable]
public class AudioClipElement
{
    [SerializeField]
    private int targetID;

    public int TargetID { get { return targetID; } }
    public AudioClip TargetAudio;
}

[Serializable]
public class ScriptingElement
{
    [SerializeField]
    private int targetID;

    public int TargetID { get { return targetID; } }

    [Multiline]
    public string Code;
}
