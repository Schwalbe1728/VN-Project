using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ItemScript))]
public class ItemCustomInspector : UnityEditor.Editor
{
    public override void OnInspectorGUI()    
    {        
        ItemScript selected = target as ItemScript;

        bool save = false;

        save |= SerializedStringProperty("name", "Name");
        save |= SerializedIntProperty("value", "Abstract Value", true);
        save |= SerializedEquipmentType("Equipment Type");

        if(selected.IsWeapon)
        {
            save |= DrawPropertiesOfWeapon();
        }

        if(selected.IsEdible)
        {
            save |= DrawPropertiesOfEdible();
        }

        if (save)
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.LabelField("Monetary value", selected.MonetaryValueInCoin());
    }    

    private bool SerializedStringProperty(string name, string label)
    {
        bool result = false;
        string oldString = serializedObject.FindProperty(name).stringValue;
        string newString = EditorGUILayout.TextField(label, oldString);

        if (!newString.Equals(oldString))
        {
            serializedObject.FindProperty(name).stringValue = newString;
            result = true;
        }

        return result;
    }

    private bool SerializedIntProperty(string name, string label, bool positiveOrZero, string notSmallerThanProperty = "")
    {
        int notSmallerThan =
            (notSmallerThanProperty != null && !notSmallerThanProperty.Equals("")) ?
                serializedObject.FindProperty(notSmallerThanProperty).intValue :
                (positiveOrZero ? 0 : int.MinValue);

        bool result = false;
        int oldInt = serializedObject.FindProperty(name).intValue;
        int newInt = EditorGUILayout.IntField(label, oldInt);

        if (newInt < notSmallerThan) newInt = notSmallerThan;

        if (newInt != oldInt)
        {
            serializedObject.FindProperty(name).intValue = newInt;
            result = true;
        }

        return result;
    }

    private bool SerializedEquipmentType(string label)
    {
        bool result = false;

        EquipmentType oldType = (EquipmentType)serializedObject.FindProperty("type").enumValueIndex;
        EquipmentType newType = (EquipmentType)EditorGUILayout.EnumPopup(label, oldType);

        if(oldType != newType)
        {
            serializedObject.FindProperty("type").enumValueIndex = (int)newType;
            result = true;
        }

        return result;
    }

    private bool DrawPropertiesOfWeapon()
    {
        bool result = false;

        result |= SerializedIntProperty("bonusDamageMin", "Min Damage", true);
        result |= SerializedIntProperty("bonusDamageMax", "Max Damage", true, "bonusDamageMin");

        return result;
    }

    private bool DrawPropertiesOfEdible()
    {
        bool result = false;

        result |= SerializedIntProperty("timeConsumed", "Consumption Time", true);
        result |= SerializedIntProperty("vitalityHealed", "Vitality Heal", false);
        result |= SerializedIntProperty("sanityHealed", "Sanity Heal", false);


        return result;
    }
}
