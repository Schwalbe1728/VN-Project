using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public partial class DialogueEditor
{
    #region Build Helpers

    bool DrawGenericBoolField(bool prevBool, int labelWidth, string labelText, out bool newBool)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel(labelText/*, GUILayout.Width(labelWidth)*/);
            newBool = EditorGUILayout.Toggle(prevBool);

            result |= prevBool != newBool;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawGenericIntField(int prevInt, int labelWidth, string labelText, out int newInt)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel(labelText/*, GUILayout.Width(labelWidth)*/);
            newInt = EditorGUILayout.IntField(prevInt);

            result |= prevInt != newInt;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawGenericStringField(string prevString, int labelWidth, string labelText, out string newString)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel(labelText/*, GUILayout.Width(labelWidth)*/);
            newString = EditorGUILayout.TextField(prevString);

            result |= !newString.Equals(prevString);
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawCharacterAttributeField(CharacterStat prevStat, int labelWidth, out CharacterStat stat)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Attribute"/*, GUILayout.Width(labelWidth)*/);
            stat = (CharacterStat)EditorGUILayout.EnumPopup(prevStat);

            result |= prevStat != stat;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawInequalityField(InequalityTypes prevType, int labelWidth, out InequalityTypes inequality)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Inequality"/*, GUILayout.Width(labelWidth)*/);
            inequality = (InequalityTypes)EditorGUILayout.EnumPopup(prevType);

            result |= prevType != inequality;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawValueCheckedField(int prevVal, int labelWidth, out int value)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {            
            EditorGUILayout.PrefixLabel("Value Checked"/*, GUILayout.Width(labelWidth)*/);
            value = EditorGUILayout.IntField(prevVal);

            result |= prevVal != value;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawPlayerSkillField(CharacterSkills prevSkill, int labelWidth, out CharacterSkills skill)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Skill"/*, GUILayout.Width(labelWidth)*/);
            skill = (CharacterSkills)EditorGUILayout.EnumPopup(prevSkill);

            result |= prevSkill != skill;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawTestModificatorField(int prevMod, int labelWidth, out int modificator)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Modificator"/*, GUILayout.Width(labelWidth)*/);
            modificator = EditorGUILayout.IntField(prevMod);

            result |= prevMod != modificator;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    #endregion
    #region Conditions Drawers
    bool DrawAttributeCheckInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        CharacterStat stat;
        int value;
        InequalityTypes inequality;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawCharacterAttributeField(currentCondition.AttributeCheck.Attribute, labelWidth, out stat);
            result |= DrawInequalityField(currentCondition.AttributeCheck.InequalityType, labelWidth, out inequality);
            result |= DrawValueCheckedField(currentCondition.AttributeCheck.ValueChecked, labelWidth, out value);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetAttributeCheckCondition(stat, value, inequality);
            Debug.Log("Zmiana attribute check");
        }

        return typeChangedToThis || result;
    }

    bool DrawAttributeTestInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        CharacterStat stat;
        int modificator;
        CharacterSkills skill;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawCharacterAttributeField(currentCondition.AttributeTest.Attribute, labelWidth, out stat);
            result |= DrawTestModificatorField(currentCondition.AttributeTest.AttributeMod, labelWidth, out modificator);
            result |= DrawPlayerSkillField(currentCondition.AttributeTest.Skill, labelWidth, out skill);
        }
        EditorGUILayout.EndVertical();


        if (typeChangedToThis || result)
        {
            currentCondition.SetAttributeTestCondition(stat, modificator, skill);
            Debug.Log("Zmiana attribute test");
        }

        return typeChangedToThis || result;
    }

    bool DrawSkillPossessedInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        CharacterSkills skill;
        bool weWantSkill;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawPlayerSkillField(currentCondition.SkillPossessed.SkillToCheck, labelWidth, out skill);
            result |= DrawGenericBoolField(currentCondition.SkillPossessed.IsNeeded, labelWidth, "Wanted?", out weWantSkill);
        }
        EditorGUILayout.EndVertical();

        if(typeChangedToThis || result)
        {
            currentCondition.SetSkillPossessedCondition(skill, weWantSkill);
            Debug.Log("Zmiana skill possessed");
        }

        return typeChangedToThis || result;
    }
    
    //TO FINISH
    bool DrawPlayerHasItemInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        //Item
        bool weWantItem;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            //Item setting
            result |= DrawGenericBoolField(currentCondition.PlayerHasItem.IsRequired, labelWidth, "Wanted?", out weWantItem);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetPlayerHasItemCondition(weWantItem);
            Debug.Log("Zmiana playerHasItem");
        }

        return typeChangedToThis || result;
    }

    bool DrawStoryStateHappenedInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        string stateMachineName;
        string stateName;
        bool stateHappened;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            //Item setting
            result |= DrawGenericStringField(currentCondition.StoryStateHappenedCondition.PlotName, labelWidth, "Plot Name", out stateMachineName);
            result |= DrawGenericStringField(currentCondition.StoryStateHappenedCondition.StateName, labelWidth, "State Name", out stateName);
            result |= DrawGenericBoolField(currentCondition.StoryStateHappenedCondition.HasHappened, labelWidth, "Happened?", out stateHappened);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetStoryStateHappenedCondition(stateMachineName, stateName, stateHappened);
            Debug.Log("Zmiana story state happened");
        }

        return typeChangedToThis || result;
    }
    #endregion    
}
