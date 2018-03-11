using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DialogueEditor
{
    bool DrawGenericAudioClipField(AudioClip prevClip, int labelWidth, string labelText, out AudioClip newClip)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            newClip = (AudioClip) EditorGUILayout.ObjectField(labelText, prevClip, typeof(AudioClip), false);

            //result |= prevBool != newBool;

            newClip = prevClip;
        }
        GUILayout.EndHorizontal();

        return result;
    }

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

        if (result || currentAction.HurtPlayerSanity == null)
        {
            currentAction.SetHurtPlayerSanityAction(damage);
            Debug.Log("Set Hurt Player's Sanity");
        }

        return result;
    }

    bool DrawChangeMusicInterior(DialogueAction currentAction)
    {
        bool result = false;

        AudioClip clip;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericAudioClipField(currentAction.ChangeMusic.Clip, 90, "Change To", out clip);
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.ChangeMusic == null)
        {
            currentAction.SetChangeMusicAction(clip);
            Debug.Log("Set Change Music");
        }

        return result;
    }

    bool DrawChangeAmbienceInterior(DialogueAction currentAction)
    {
        bool result = false;

        AudioClip clip;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericAudioClipField(currentAction.ChangeAmbience.Clip, 90, "Change To", out clip);
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.ChangeAmbience == null)
        {
            currentAction.SetChangeAmbienceAction(clip);
            Debug.Log("Set Change Ambience");
        }

        return result;
    }

    bool DrawGiveItemInterior(DialogueAction currentAction)
    {
        bool result = false;

        //EquipmentItem item;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            GUILayout.Label("TODO: EquipmentItem : ScriptableObject");
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.GiveItem == null)
        {
            currentAction.SetGiveItemAction();
            Debug.Log("Set Give Item");
        }

        return result;
    }

    bool DrawTakeItemInterior(DialogueAction currentAction)
    {
        bool result = false;

        //EquipmentItem item;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            GUILayout.Label("TODO: EquipmentItem : ScriptableObject");
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.TakeItem == null)
        {
            currentAction.SetTakeItemAction();
            Debug.Log("Set Take Item");
        }

        return result;
    }
}
