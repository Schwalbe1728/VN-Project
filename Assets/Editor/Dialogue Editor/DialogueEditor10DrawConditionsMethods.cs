using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public partial class DialogueEditor
{
    #region Build Helpers

    bool DrawGenericMoneyInterior(int prevAbstractValue, int labelWidth, out int newAbstractValue)
    {
        bool result = false;        

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericIntField(prevAbstractValue, labelWidth, "Abstract Value", out newAbstractValue, 0);

            EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
            {
                int pounds;
                int shillings;
                float pences;

                ItemExtension.ToCoins(newAbstractValue, out pounds, out shillings, out pences);

                result |= DrawGenericIntField(pounds, labelWidth, "Pounds", out pounds, 0);
                result |= DrawGenericIntField(shillings, labelWidth, "Shillings", out shillings, 0);
                result |= DrawGenericFloatField(pences, labelWidth, "Pences", out pences, 0);

                newAbstractValue = ItemExtension.FromCoins(pounds, shillings, pences);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();

        return result;
    }

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

    bool DrawGenericIntField(int prevInt, int labelWidth, string labelText, out int newInt, int minimum = int.MinValue)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            //EditorGUILayout.PrefixLabel(labelText/*, GUILayout.Width(labelWidth)*/);
            newInt = EditorGUILayout.IntField(labelText, prevInt);
            if (newInt < minimum) newInt = minimum;

            result |= prevInt != newInt;
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawGenericFloatField(float prevFloat, int labelWidth, string labelText, out float newFloat, float minimum = float.MinValue)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            //EditorGUILayout.PrefixLabel(labelText/*, GUILayout.Width(labelWidth)*/);
            newFloat = EditorGUILayout.FloatField(labelText, prevFloat);
            if (newFloat < minimum) newFloat = minimum;

            result |= prevFloat != newFloat;
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

    bool DrawGenericWorldDateField(WorldDate prevDate, int labelWidth, string labelText, out WorldDate newDate, bool omitHours = false)
    {
        bool result = false;

        bool foldout =
            prevDate == null ||
            prevDate.ShowInFoldout;

        bool newFoldout =
            EditorGUILayout.Foldout(foldout, (foldout) ? labelText : labelText + ": " + ((prevDate == null) ? "..." : prevDate.ToString()));

        if (newFoldout)
        {
            EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
            {
                bool prevIsNull = prevDate == null;
                int days = (prevIsNull) ? 0 : prevDate.Days;
                int hours = (prevIsNull) ? 0 : prevDate.Hours;
                int minutes = (prevIsNull) ? 0 : prevDate.Minutes;
                int secs = (prevIsNull) ? 0 : prevDate.Seconds;

                days = (omitHours) ? 0 : EditorGUILayout.IntField("Day", days);
                hours = EditorGUILayout.IntField("Hour", hours);
                minutes = EditorGUILayout.IntField("Minute", minutes);
                secs = EditorGUILayout.IntField("Seconds", secs);

                days = (days >= 0) ? days : 0;
                hours = (hours >= 0) ? hours : 0;
                minutes = (minutes >= 0) ? minutes : 0;
                secs = (secs >= 0) ? secs : 0;

                newDate = new WorldDate(secs, minutes, hours, days);                

                result = !newDate.Equals(prevDate);                
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            newDate = prevDate;
        }

        newDate.ShowInFoldout = newFoldout;
        prevDate.ShowInFoldout = newFoldout;

        return result;
    }

    bool DrawGenericAssetField<T>(T prevObject, int labelWidth, string labelText, out T newObject) where T: UnityEngine.Object
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            newObject =
                (T)EditorGUILayout.ObjectField(labelText, prevObject, typeof(T), false);

            result |=
                (prevObject != null && !prevObject.Equals(newObject)) ||
                (newObject != null && !newObject.Equals(prevObject));
        }
        GUILayout.EndHorizontal();

        return result;
    }

    bool DrawCharacterAttributeField(CharacterAttribute prevStat, int labelWidth, out CharacterAttribute stat)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Attribute"/*, GUILayout.Width(labelWidth)*/);
            stat = (CharacterAttribute)EditorGUILayout.EnumPopup(prevStat);

            result |= prevStat != stat;
        }
        GUILayout.EndHorizontal();        

        return result;
    }

    bool DrawCharacterStatisticField(CharacterStatistic prevStat, int labelWidth, out CharacterStatistic stat)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Attribute"/*, GUILayout.Width(labelWidth)*/);
            stat = (CharacterStatistic)EditorGUILayout.EnumPopup(prevStat);

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

    private bool DrawBackgroundDefinitionField(BackgroundDefinition prevBackground, int labelWidth, string labelText, out BackgroundDefinition newBackground)
    {
        bool result = false;

        GUILayout.BeginHorizontal();
        {
            newBackground =
                (BackgroundDefinition)
                    EditorGUILayout.ObjectField(labelText, prevBackground, typeof(BackgroundDefinition), false);

            result |=
                (prevBackground != null && !prevBackground.Equals(newBackground)) ||
                (prevBackground != newBackground);
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

        CharacterAttribute stat;
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

    bool DrawStatisticCheckInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        CharacterStatistic stat;
        float value;
        InequalityTypes inequality;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawCharacterStatisticField(currentCondition.StatisticCheck.Statistic, labelWidth, out stat);
            result |= DrawInequalityField(currentCondition.StatisticCheck.InequalityType, labelWidth, out inequality);
            result |= DrawGenericFloatField(currentCondition.StatisticCheck.ValueChecked, labelWidth, "Statistic Value", out value);
                //DrawValueCheckedField(currentCondition.AttributeCheck.ValueChecked, labelWidth, out value);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetStatisticCheckCondition(stat, inequality, value);
            Debug.Log("Zmiana attribute check");
        }

        return typeChangedToThis || result;
    }

    bool DrawAttributeTestInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        CharacterAttribute stat;
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

        ItemScript item;
        int quantity;
        bool weWantItem;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericAssetField<ItemScript>(currentCondition.PlayerHasItem.Item, labelWidth, "Item", out item);
            result |= DrawGenericIntField(currentCondition.PlayerHasItem.Quantity, labelWidth, "Quantity", out quantity, 0);
            result |= DrawGenericBoolField(currentCondition.PlayerHasItem.IsRequired, labelWidth, "Wanted?", out weWantItem);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetPlayerHasItemCondition(item, quantity, weWantItem);
            Debug.Log("Zmiana playerHasItem");
        }

        return typeChangedToThis || result;
    }

    bool DrawPlayerHasMoneyInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;
        int abstractValue;

        result |= DrawGenericMoneyInterior(currentCondition.PlayerHasMoney.AbstractValue, labelWidth, out abstractValue);

        if(result || typeChangedToThis)
        {
            currentCondition.SetPlayerHasMoneyCondition(abstractValue);
        }

        return result;
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
            result |= DrawGenericStringField(currentCondition.StoryStateHappened.PlotName, labelWidth, "Plot Name", out stateMachineName);
            result |= DrawGenericStringField(currentCondition.StoryStateHappened.StateName, labelWidth, "State Name", out stateName);
            result |= DrawGenericBoolField(currentCondition.StoryStateHappened.HasHappened, labelWidth, "Happened?", out stateHappened);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetStoryStateHappenedCondition(stateMachineName, stateName, stateHappened);
            Debug.Log("Zmiana story state happened");
        }

        return typeChangedToThis || result;
    }

    bool DrawWorldDateInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        WorldDate date;
        InequalityTypes time;
        
        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericWorldDateField(currentCondition.WorldDate.Date, labelWidth, "Date", out date);
            result |= DrawInequalityField(currentCondition.WorldDate.TimeOrientation, labelWidth, out time);
        }
        EditorGUILayout.EndVertical();

        if(typeChangedToThis || result)
        {
            currentCondition.SetWorldDateCondition(date, time);
            Debug.Log("Zmiana na World Date");
        }

        return result;
    }

    bool DrawWithinTimeRangeInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        WorldDate startDate;
        WorldDate finishDate;
        bool hoursOnly;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawGenericWorldDateField(currentCondition.WithinTimeRange.Start, labelWidth, "Start", out startDate, currentCondition.WithinTimeRange.HoursOnly);
            result |= DrawGenericWorldDateField(currentCondition.WithinTimeRange.Finish, labelWidth, "Finish", out finishDate, currentCondition.WithinTimeRange.HoursOnly);
            result |= DrawGenericBoolField(currentCondition.WithinTimeRange.HoursOnly, labelWidth, "Hours Only?", out hoursOnly);
        }
        EditorGUILayout.EndVertical();

        if(typeChangedToThis || result)
        {
            currentCondition.SetWithinTimeRangeCondition(startDate, finishDate, hoursOnly);
            Debug.Log("Zmiana na Within Time Range");
        }

        return result;
    }

    bool DrawBackgroundRequiredInterior(ConditionNode currentCondition, bool typeChangedToThis)
    {
        bool result = false;
        int labelWidth = 90;

        BackgroundDefinition background;
        bool isRequired;

        EditorGUILayout.BeginVertical(Config.FoldoutInteriorStyle);
        {
            result |= DrawBackgroundDefinitionField(currentCondition.BackgroundRequired.Background, labelWidth, "Background", out background);
            result |= DrawGenericBoolField(currentCondition.BackgroundRequired.Required, labelWidth, "Required?", out isRequired);
        }
        EditorGUILayout.EndVertical();

        if (typeChangedToThis || result)
        {
            currentCondition.SetBackgroundRequiredCondition(background, isRequired);
            Debug.Log("Zmiana na Background Required");
        }

        return result;
    }    
    #endregion
}
