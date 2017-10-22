using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSpecificValuesScript : MonoBehaviour {
    
    [SerializeField]    
    private PlotValueStateMachine[] StateMachines;

    private Animator animatorShell;
    private Dictionary<string, PlotValueStateMachine> NameToPlotMachine;    

    public string CheckState(string name)
    {
        SetAnimatorController(name);

        return NameToPlotMachine[name].CurrentState;
    }

    public void SetTrigger(string plotName, string trigger)
    {
        SetAnimatorController(plotName);
        animatorShell.SetTrigger(trigger);
    }

    public void MachineEnteredState(string machineName, string state)
    {
        NameToPlotMachine[machineName].CurrentState = state;
        Debug.Log(NameToPlotMachine[machineName].CurrentState);
    }

    public bool MachineVisitedState(string machineName, string state)
    {
        return NameToPlotMachine[machineName].CheckForVisitedState(state);
    }

    void Awake()
    {
        NameToPlotMachine = new Dictionary<string, PlotValueStateMachine>();

        foreach (PlotValueStateMachine pvsm in StateMachines)
        {
            NameToPlotMachine.Add(pvsm.Controller.name, pvsm);
        }

        animatorShell = GetComponent<Animator>();
    }     

    private void SetAnimatorController(string name)
    {
        animatorShell.runtimeAnimatorController
            = NameToPlotMachine[name].Controller;
    }
}

[Serializable]
public class PlotValueStateMachine
{
    public string Name { get { return name; } }
    public RuntimeAnimatorController Controller { get { return controller; } }
   
    public string CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;

            if (VisitedStates == null) VisitedStates = new List<string>();

            VisitedStates.Add(value);
        }
    }

    [SerializeField]
    private string name;
    [SerializeField]
    private RuntimeAnimatorController controller;

    private string currentState;
    private List<string> VisitedStates;

    public bool CheckForVisitedState(string stateName)
    {
        return VisitedStates.Contains(stateName);
    }
}

/*
[Serializable]
public class PlotValueStateMachine
{
    public string Name { get { return MachineName; } }
    public string CurrentState { get { return currentState.StateName; } }
    public StateTransition[] AvailableTransitions
    {
        get
        {
            List<StateTransition> result = new List<StateTransition>();

            foreach(TransitionDefinition trans in currentState.Transitions)
            {
                result.Add(trans.Transition);
            }

            return result.ToArray();
        }
    }

    [SerializeField]
    private string MachineName;

    [SerializeField]
    private PossibleTransitionsDefinition currentState;

    [SerializeField]
    private PossibleTransitionsDefinition[] TransDefinitions;

    public void GoTo(StateTransition trans)
    {
        bool notFound = true;

        for(int i = 0; i < currentState.Transitions.Length && notFound; i++)
        {
            if(currentState.Transitions[i].Transition == trans)
            {               
                for(int j = 0; j < TransDefinitions.Length; j++)
                {
                    if(TransDefinitions[j].StateName.Equals(currentState.Transitions[i].TargetStateName))
                    {
                        currentState = TransDefinitions[j];
                        notFound = false;
                        break;
                    }
                }
            }
        }
    }
}

[Serializable]
public class TransitionDefinition
{
    public StateTransition Transition;
    public string TargetStateName;
}

[Serializable]
public class PossibleTransitionsDefinition
{
    public string StateName;
    public TransitionDefinition[] Transitions;
}
*/
public enum QuestProgress
{
    NotTaken,
    Impossible,
    Taken,
    InProgress,
    Failed,
    AwaitingConclusion,
    Success
}

public enum StateTransition
{
    A, B, C, D, E,
    F, G, H, I, J,
    K, L, M, N, O, 
    P, Q, R, S, T,
    U, V, W, X, Y,
    Z
}

