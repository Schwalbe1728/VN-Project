using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;
using System;

public delegate void NodeEntered(NodeEnteredEventArgs args);
public delegate void DialogueEnded(DialogueEndedEventArgs args);

public class DialogueScript : MonoBehaviour
{
    public string NPCDisplayedName { get { return npcDisplayedName; } }
    public string SourceDialogueID { get { return SourcePath; } }

    [SerializeField]
    private string npcDisplayedName;

    [SerializeField]
    [Tooltip("Path of the .dlg file displayed by this DialogueScript.")]
    private string SourcePath;

    [SerializeField]    
    private int DialogueStartNode = 0;

    [SerializeField]
    [Tooltip("Identifier of the dialogue started after reaching DIALOGUE EXIT. Usually set in Dialogue Editor, and thus we discourage using this field.")]
    private string NextDialogueString;

    [SerializeField]
    private SpriteElement[] backgroundSprites;
    [SerializeField]
    private SpriteElement[] portraitSprites;
    [SerializeField]
    private AudioClipElement[] backgroundAudio;
    [SerializeField]
    private AudioClipElement[] ambienceAudio;
    [SerializeField]
    private AudioClipElement[] voicedLinesAudio;   

    private NodeIDToSpriteDictionary BackgroundsDictionary;
    private NodeIDToSpriteDictionary NPCPortraitsDictionary;
    private NodeIDToAudioClipDictionary BackMusicDictionary;  
    private NodeIDToAudioClipDictionary AmbienceMusic;
    private NodeIDToAudioClipDictionary VoicedLines;    

    private Dialogue DialogueObject;

    private DialogueNode currentNode;
    private DialogueOption[] currentOptions;

    private event NodeEntered OnNodeEntered;
    private event DialogueEnded OnDialogueEnded;

    private BackgroundScript BackgroundScr;
    private SoundManagerScript SoundManager;

    public void StartDialogue()
    {
        currentNode = DialogueObject.GetNode(DialogueStartNode);
        currentOptions = DialogueObject.GetAvailableOptionsArrayFromNode(currentNode.NodeID);
        NewNodeEntered();
    }

    public void RegisterToNodeEnteredEvent(NodeEntered method)
    {
        OnNodeEntered += method;
    }

    public void RegisterToDialogueEndedEvent(DialogueEnded method)
    {
        OnDialogueEnded += method;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public bool NextNode(DialogueOption option = null)
    {
        if(currentNode == null)
        {
            return false;
        }

        int nextNodeID = currentNode.GetNext(option);

        currentNode = (nextNodeID == Dialogue.DIALOGUE_EXIT)? null : DialogueObject.GetNode(nextNodeID);
        currentOptions = (nextNodeID == Dialogue.DIALOGUE_EXIT || currentNode.Options == null || currentNode.Options.Count == 0) ?
            null :
            DialogueObject.GetAvailableOptionsArrayFromNode(currentNode.NodeID);

        if(currentNode != null)
        {
            if(currentNode.NextDialogueString != null && !currentNode.NextDialogueString.Equals(""))
            {
                NextDialogueString = currentNode.NextDialogueString;
            }
        }

        NewNodeEntered();

        return currentNode != null;
    }    

    public DialogueNode CurrentNode { get { return currentNode; } }
    public DialogueOption[] CurrentOptions { get { return currentOptions; } }

	// Use this for initialization
	void Start ()
    {
        DialogueObject =
            DialoguesManagerScript.GetDialogue(SourcePath);
            //DialogueTree.DialogueSerializer.LoadDialogue( SourcePath );

        //DialogueObject = Resources.Load<Dialogue>(SourcePath);

        if (DialogueObject == null) Debug.LogWarning("Wrong path");
        //nodes = DialogueObject.GetNodes().ToArray();
        //options = DialogueObject.GetOptions().ToArray();

        CreateDictionaries();

        ConditionsAccumulator accum = GetComponent<ConditionsAccumulator>();
        if (accum != null)
        {
            accum.InjectConditions(DialogueObject);
        }

        //BackgroundScr = GameObject.Find("Surroundings").GetComponent<BackgroundScript>();
        //SoundManager = GameObject.Find("Sound Manager").GetComponent<SoundManagerScript>();        

        RegisterToNodeEnteredEvent(CheckDictionaries);
	}       	

    private void CreateDictionaries()
    {
        BackgroundsDictionary = new NodeIDToSpriteDictionary();

        foreach(SpriteElement spr in backgroundSprites)
        {
            if(!BackgroundsDictionary.ContainsKey(spr.TargetID))
            {
                BackgroundsDictionary.Add(spr.TargetID, spr.TargetSprite);
            }
        }

        NPCPortraitsDictionary = new NodeIDToSpriteDictionary();

        foreach(SpriteElement spr in portraitSprites)
        {
            if(!NPCPortraitsDictionary.ContainsKey(spr.TargetID))
            {
                NPCPortraitsDictionary.Add(spr.TargetID, spr.TargetSprite);
            }
        }

        BackMusicDictionary = new NodeIDToAudioClipDictionary();

        foreach(AudioClipElement clip in backgroundAudio)
        {
            if(!BackMusicDictionary.ContainsKey(clip.TargetID))
            {
                BackMusicDictionary.Add(clip.TargetID, clip.TargetAudio);
            }
        }

        AmbienceMusic = new NodeIDToAudioClipDictionary();

        foreach (AudioClipElement clip in ambienceAudio)
        {
            if (!AmbienceMusic.ContainsKey(clip.TargetID))
            {
                AmbienceMusic.Add(clip.TargetID, clip.TargetAudio);
            }
        }

        VoicedLines = new NodeIDToAudioClipDictionary();

        foreach (AudioClipElement clip in voicedLinesAudio)
        {
            if (!VoicedLines.ContainsKey(clip.TargetID))
            {
                VoicedLines.Add(clip.TargetID, clip.TargetAudio);
            }
        }

    }

    /// <summary>
    /// Fires NodeEntered event, or DialogueEnded event if we reached the end of the dialogue. 
    /// Invoked when system registers change to CurrentNode.
    /// </summary>
    private void NewNodeEntered()
    {
        //TODO: wykrycie końca dialogu

        if (CurrentNode == null || ((CurrentNode.Options == null || CurrentNode.Options.Count == 0) && CurrentNode.ImmidiateNodeID == Dialogue.DIALOGUE_EXIT ))
        {            
            if (OnDialogueEnded != null)
            {
                OnDialogueEnded(new DialogueEndedEventArgs(NextDialogueString));
            }

            if (CurrentNode != null && OnNodeEntered != null)
            {
                OnNodeEntered(new NodeEnteredEventArgs(CurrentNode.NodeID));
            }
        }
        else
        {
            if (OnNodeEntered != null)
            {
                OnNodeEntered(new NodeEnteredEventArgs(CurrentNode.NodeID));
            }            
        }
    }

    private void CheckDictionaries(NodeEnteredEventArgs args)
    {
        if(BackgroundsDictionary.ContainsKey(args.NodeID))
        {
            BackgroundScr.NextBackground(BackgroundsDictionary[args.NodeID]);
        }

        if(NPCPortraitsDictionary.ContainsKey(args.NodeID))
        {
            BackgroundScr.NextNPCPortrait(NPCPortraitsDictionary[args.NodeID]);
        }

        if(BackMusicDictionary.ContainsKey(args.NodeID))
        {
            SoundManager.PlayMusic(BackMusicDictionary[args.NodeID]);
        }

        if(AmbienceMusic.ContainsKey(args.NodeID))
        {
            SoundManager.PlayAmbience(AmbienceMusic[args.NodeID]);
        }

        if(VoicedLines.ContainsKey(args.NodeID))
        {
            SoundManager.PlaySound(VoicedLines[args.NodeID]);
        }
    }
}

public class NodeEnteredEventArgs
{
    public int NodeID { get; private set; }

    public NodeEnteredEventArgs(int ID)
    {
        NodeID = ID;
    }
}

public class DialogueEndedEventArgs
{
    public string NextDialogue { get; private set; }
    public bool CloseDialogueWindow { get; private set; }

    public DialogueEndedEventArgs(string nextDialogue, bool close = false)
    {
        NextDialogue = nextDialogue;
        CloseDialogueWindow = close;
    }
}
