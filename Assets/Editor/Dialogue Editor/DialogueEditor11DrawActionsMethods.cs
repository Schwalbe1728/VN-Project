using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DialogueEditor
{
    bool DrawAdvanceTimeField(int prevSeconds, int labelWidth, out int newSeconds)
    {
        bool result = false;

        WorldDate temp = new WorldDate(prevSeconds, 0, 0, 0);
        int seconds = temp.Seconds;
        int minutes = temp.Minutes;
        int hours = temp.Hours;
        int days = temp.Days;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericIntField(days, labelWidth, "Days", out days, 0);
            result |= DrawGenericIntField(hours, labelWidth, "Hours", out hours, 0);
            result |= DrawGenericIntField(minutes, labelWidth, "Minutes", out minutes, 0);
            result |= DrawGenericIntField(seconds, labelWidth, "Seconds", out seconds, 0);
        }
        EditorGUILayout.EndVertical();

        newSeconds = TimeManagerScript.ToSeconds(days, hours, minutes, seconds);

        return result;
    }

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

        ItemScript item;
        int quantity;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            //GUILayout.Label("TODO: EquipmentItem : ScriptableObject");
            result |= DrawGenericAssetField<ItemScript>(currentAction.GiveItem.Item, 90, "Item To Give", out item);
            result |= DrawGenericIntField(currentAction.GiveItem.Quantity, 90, "Quantity", out quantity);

        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.GiveItem == null)
        {
            currentAction.SetGiveItemAction(item, quantity);
            Debug.Log("Set Give Item");
        }

        return result;
    }

    bool DrawTakeItemInterior(DialogueAction currentAction)
    {
        bool result = false;

        ItemScript item;
        int quantity;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            //GUILayout.Label("TODO: EquipmentItem : ScriptableObject");
            result |= DrawGenericAssetField<ItemScript>(currentAction.TakeItem.Item, 90, "Item To Take", out item);
            result |= DrawGenericIntField(currentAction.GiveItem.Quantity, 90, "Quantity", out quantity);
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.TakeItem == null)
        {
            currentAction.SetTakeItemAction(item, quantity);
            Debug.Log("Set Take Item");
        }

        return result;
    }

    bool DrawUseItemInterior(DialogueAction currentAction)
    {
        bool result = false;

        ItemScript item;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericAssetField<ItemScript>(currentAction.UseItem.Item, 90, "Item To Use", out item);
        }
        EditorGUILayout.EndVertical();

        if(result || currentAction.UseItem == null)
        {
            currentAction.SetUseItemAction(item);
            Debug.Log("Set Use Item");
        }

        return result;
    }

    bool DrawUpdateJournalInterior(DialogueAction currentAction)
    {
        bool result = false;

        JournalEntryScript entry;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericAssetField<JournalEntryScript>(currentAction.UpdateJournal.Entry, 90, "Journal Entry", out entry);
        }
        EditorGUILayout.EndVertical();

        if (result || currentAction.UpdateJournal == null)
        {
            currentAction.SetUpdateJournalAction(entry);
            Debug.Log("Set Update Journal");
        }


        return result;
    }

    bool DrawAdvanceTimeActionInterior(DialogueAction currentAction)
    {
        bool result = false;

        int totalSeconds = 0;
        bool allowVariance;
        float varianceValue;

        float maxVariance = 0.5f;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawAdvanceTimeField(currentAction.AdvanceTime.Seconds, 90, out totalSeconds);
            result |= DrawGenericBoolField(currentAction.AdvanceTime.VarianceSet, 90, "Allow Variance", out allowVariance);

            if (allowVariance)
            {
                int temp;

                result |= DrawGenericIntField(
                    Mathf.RoundToInt(100 * currentAction.AdvanceTime.VarianceValue), 
                    90, 
                    "Variance (%)", 
                    out temp, 0);

                varianceValue = Mathf.Min(temp / 100.0f, maxVariance);
            }
            else
            {
                varianceValue = 0;
            }
        }
        EditorGUILayout.EndVertical();

        if(result)
        {
            currentAction.SetAdvanceTimeAction(totalSeconds, allowVariance, varianceValue);
            Debug.Log("Set Advance Time");
        }

        return result;
    }
}
