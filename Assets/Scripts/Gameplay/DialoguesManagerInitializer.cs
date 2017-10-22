using System.Collections;
using System.Collections.Generic;
using DialogueTree;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class DialoguesManagerScript
{
    public static bool Ready { get { return isReady; } }
    private static bool isReady = false;

    private static Dictionary<string, Dialogue> Dialogues;
    private static Dictionary<string, GameObject> DialogueIDToGameObject;

    public static Dialogue GetDialogue(string value)
    {   
        return Dialogues[value];
    }

    public static GameObject GetDialogueGameObject(string value)
    {
        return DialogueIDToGameObject[value];
    }

    public static void Initiate()
    {
        Dialogues = new Dictionary<string, Dialogue>();
        DialogueIDToGameObject = new Dictionary<string, GameObject>();
    }    

    public static IEnumerator Load(TextAsset text, string pathBase)
    {
        string[] paths = text.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        string line;

        for(int i = 0; i < paths.Length; i++)
        {
            line = paths[i];

            string[] dialogueFilePathArray = Split(line);
            string dialogueName = dialogueFilePathArray[dialogueFilePathArray.Length - 1].Replace(".dlg", "");
            string fileNameTemp = pathBase + '/' + line;
            StringBuilder fileName = new StringBuilder(string.Join("/", fileNameTemp.Split(Path.GetInvalidFileNameChars())));

            if(fileName[fileName.Length-1] == '/') fileName.Remove(fileName.Length - 1, 1);

            Dialogues.Add(
                dialogueName,
                DialogueSerializer.LoadDialogue(
                    fileName.ToString()
                    )
                );            

            yield return null;
        }

        //yield return CreateDialogueToSceneIndex();        

        isReady = true;
    }    

    private static string GetPath(string[] path, int ignoreFromEnd = 1)
    {
        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < path.Length - ignoreFromEnd; i++)
        {
            sb.Append(path[i] + '/');
        }

        return sb.ToString();
    }

    private static string[] Split(string toSplit)
    {
        return toSplit.Split(new char[] { '/', '\\', '\n', '\r', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
