using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DialogueEditor
{
    bool DrawHurtPlayerActionInterior(DialogueAction currentAction)
    {
        bool result = false;

        int damage;        

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericIntField(currentAction.HurtPlayer.Damage, 90, "Damage", out damage);
        }
        EditorGUILayout.EndVertical();

        if(result || currentAction.HurtPlayer == null)
        {
            currentAction.SetHurtPlayerAction(damage);
            Debug.Log("Set Hurt Player");
        }

        return result;
    }

    bool DrawHurtPlayerSanityInterior(DialogueAction currentAction)
    {
        bool result = false;

        int damage;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericIntField(currentAction.HurtPlayerSanity.Damage, 90, "Damage", out damage);
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.HurtPlayer == null)
        {
            currentAction.SetHurtPlayerSanityAction(damage);
            Debug.Log("Set Hurt Player's Sanity");
        }

        return result;
    }
}
