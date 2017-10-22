using DialogueTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsAccumulator : MonoBehaviour
{
    [SerializeField]
    private CharacterInfoScript PlayerScr;

    [SerializeField]
    [Tooltip("Conditions checked when deciding, whether the DialogueOption should be displayed or not.")]
    private Condition[] OptionsEntryConditions;

    [SerializeField]
    [Tooltip("Conditions checked after choosing the DialogueOption, determining the outcome of the choice.")]
    private Condition[] OptionsExitConditions;

    [SerializeField]
    [Tooltip("Conditions checked after entering the Immidiate DialogueNode, determining, which DialogueNode should be displayed next.")]
    private Condition[] NodesExitConditions;
        
    public void InjectConditions(Dialogue dialogue)
    {
        foreach(Condition cond in OptionsEntryConditions)
        {
            cond.SetPlayerObject(PlayerScr);
            dialogue.GetOption(cond.TargetID).SetEntryCondition(cond);
        }

        foreach (Condition cond in OptionsExitConditions)
        {
            cond.SetPlayerObject(PlayerScr);
            dialogue.GetOption(cond.TargetID).SetExitCondition(cond);
            dialogue.GetOption(cond.TargetID).Text =
                dialogue.GetOption(cond.TargetID).Text;
        }

        foreach(Condition cond in NodesExitConditions)
        {
            cond.SetPlayerObject(PlayerScr);
            dialogue.GetNode(cond.TargetID).SetExitCondition(cond);
        }
    } 
    
    public Condition GetOptionExit(int index)
    {
        Condition result = null;

        for(int i = 0; i < OptionsExitConditions.Length; i++)
        {
            if(OptionsExitConditions[i].TargetID == index)
            {
                result = OptionsExitConditions[i];
                break;
            }
        }

        return result;
    }

    void Awake()
    {
        PlayerScr = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();
    }   
}
