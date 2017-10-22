using DialogueTree.Conditionals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Condition : ICondition
{
    public int TargetID;

    [SerializeField]
    private ConditionType CondType;

    [SerializeField]
    private ComparisonType CompType;

    [SerializeField]
    private string ConditionArgument;

    [SerializeField]
    private string ValueCompaired;

    private CharacterInfoScript Player;

    public bool CheckCondition()
    {
        int valComp, valSkill;

        switch(CondType)
        {
            case ConditionType.SkillCheck:
                valComp = int.Parse(ValueCompaired);
                valSkill = Player.GetStat(CharacterStatExtension.FromString(ConditionArgument));

                return CompType.Compare(valSkill, valComp);

            case ConditionType.RNGSkillCheck:
                valComp = int.Parse(ValueCompaired);
                valSkill = Player.GetStat(CharacterStatExtension.FromString(ConditionArgument));

                CompType = ComparisonType.LesserOrEqualThan;

                return StatsRuleSet.TestRNG(Player, CharacterStatExtension.FromString(ConditionArgument), Player.StressPenalty);
                    //RNGSkillCheckTest(valComp, valSkill);

            case ConditionType.PlotPoint:
                PlotSpecificValuesScript temp = GameObject.Find("Game Info Component").GetComponent<PlotSpecificValuesScript>();

                return temp.CheckState(ConditionArgument).ToLower().Equals(ValueCompaired.ToLower());
        }

        return false;
    }

    public string ChanceToPass
    {
        get
        {
            string result = "null";

            if(CondType == ConditionType.RNGSkillCheck)
            {
                int modificator = int.Parse(ValueCompaired);
                int attributeValue = Player.GetStat(CharacterStatExtension.FromString(ConditionArgument));

                int resultPercent = Mathf.Min(95, 5 * (modificator + attributeValue) - Player.StressPenalty);
                resultPercent = Mathf.Max(5, resultPercent);

                result = resultPercent.ToString();
            }

            return result;
        }
    }

    public string AttributeDefinition
    {
        get
        {
            return ConditionArgument;
        }
    }

    public int Modifier
    {
        get
        {
            return int.Parse(ValueCompaired);
        }
    }

    public void SetPlayerObject(CharacterInfoScript scr)
    {
        Player = scr;
    }

    private bool RNGSkillCheckTest(int modificator, int attribute)
    {
        attribute = attribute + modificator;

        int throwResult = UnityEngine.Random.Range(1, 20);

        if (throwResult == 1) return true;
        if (throwResult == 20) return false;

        Debug.Log("To test:" + attribute + ", value on dice: " + throwResult + ", result: " + CompType.Compare(throwResult, attribute));

        return CompType.Compare(throwResult, attribute);
    }
}

public enum ConditionType
{
    SkillCheck,
    RNGSkillCheck,
    PlotPoint,
    HasItem,    
}

public enum ComparisonType
{
    LesserThan,
    GreaterThan,
    LesserOrEqualThan,
    GreaterOrEqualThan,
    Equal
}

public static class ComparisonTypeExtension
{
    /// <summary>
    /// Read as: "A is (comparison type) than B"
    /// </summary>
    /// <param name="type"></param>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    public static bool Compare(this ComparisonType type, int A, int B)
    {
        switch(type)
        {
            case ComparisonType.LesserThan:
                return A < B;

            case ComparisonType.GreaterThan:
                return A > B;

            case ComparisonType.LesserOrEqualThan:
                return A <= B;

            case ComparisonType.GreaterOrEqualThan:
                return A >= B;

            case ComparisonType.Equal:
                return A == B;
        }

        throw new InvalidOperationException();
    }
}