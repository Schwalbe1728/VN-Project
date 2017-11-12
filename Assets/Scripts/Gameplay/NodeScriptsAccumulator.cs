using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueScript))]
public class NodeScriptsAccumulator : MonoBehaviour {

    [SerializeField]
    private CharacterCreationScript PlayerScr;

    [SerializeField]
    private PlotSpecificValuesScript PlotValues;

    [SerializeField]
    private ScriptingElement[] scriptedNodes;
    
    [SerializeField]
    private ScriptingElement[] scriptedOptions;

    void Awake()
    {
        GameObject gic = GameObject.Find("Game Info Component");

        PlayerScr = gic.GetComponent<CharacterCreationScript>();
        PlotValues = gic.GetComponent<PlotSpecificValuesScript>();

        gameObject.GetComponent<DialogueScript>().RegisterToNodeEnteredEvent(EnteredNode);
    }

    private void EnteredNode(NodeEnteredEventArgs args)
    {
        for(int i = 0; i < scriptedNodes.Length; i++)
        {
            if(scriptedNodes[i].TargetID == args.NodeID)
            {
                InterpretScript(scriptedNodes[i].Code);
            }
        }
    }

    private void InterpretScript(string scriptText)
    {
        string[] lines = SplitIntoLines(scriptText);

        foreach(string line in lines)
        {
            Debug.Log(line);

            string[] wordsInLine = line.Split(new char[] { '\n', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            switch(wordsInLine[0].ToUpper())
            {
                case "PLOTVALUE":
                    InterpretPlotValueCommand(wordsInLine);
                    break;

                case "CHARACTER":
                    break;

                case "EQUIPMENT":
                    break;
            }
        }
    }

    private void InterpretPlotValueCommand(string[] command)
    {
        string commandType = command[1].ToUpper();
        string targetMachine = command[2];
        string trigger = command[3];

        switch(commandType)
        {
            case "TRIGGER":
                //PlotValues.MachineEnteredState(targetMachine, trigger);
                PlotValues.SetTrigger(targetMachine, trigger);
                break;
        }
    }

    private string[] SplitIntoLines(string scriptText)
    {
        return scriptText.Split(new char[] { '\n', '\t', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
