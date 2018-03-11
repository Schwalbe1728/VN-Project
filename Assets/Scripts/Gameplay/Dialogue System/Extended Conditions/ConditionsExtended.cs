using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ConditionNode
{
    [SerializeField]
    private ConditionTypes conditionTypeSet;

    [SerializeField]
    private AttributeCheckCondition attributeCheckCondition;

    [SerializeField]
    private AttributeTestCondition attributeTestCondition;

    [SerializeField]
    private SkillPossessedCondition skillPossessedCondition;

    public ConditionTypes ConditionType { get { return conditionTypeSet; } }

    public AttributeCheckCondition AttributeCheck { get { return attributeCheckCondition; } }
    public AttributeTestCondition AttributeTest { get { return attributeTestCondition; } }
    public SkillPossessedCondition SkillPossessed { get { return skillPossessedCondition; } }

    public override bool ConditionTest()
    {
        switch(conditionTypeSet)
        {
            case ConditionTypes.AttributeCheck:
                return attributeCheckCondition.ConditionTest();

            case ConditionTypes.AttributeTest:
                return attributeTestCondition.ConditionTest();

            case ConditionTypes.SkillPossessed:
                return skillPossessedCondition.ConditionTest();

            default:
                Debug.Log("Wtf, ConditionTest default");
                return false;
        }
    }    

    public void SetAttributeCheckCondition(CharacterStat stat, int value, InequalityTypes type)
    {
        conditionTypeSet = ConditionTypes.AttributeCheck;

        if(attributeCheckCondition == null)
        {
            attributeCheckCondition = new AttributeCheckCondition();
        }

        attributeCheckCondition.Attribute = stat;
        attributeCheckCondition.ValueChecked = value;
        attributeCheckCondition.InequalityType = type;
    }
    public void SetAttributeTestCondition(CharacterStat stat, int mod, CharacterSkills skill)
    {
        conditionTypeSet = ConditionTypes.AttributeTest;

        if(attributeTestCondition == null)
        {
            attributeTestCondition = new AttributeTestCondition();
        }

        attributeTestCondition.AttributeMod = mod;
        attributeTestCondition.Attribute = stat;
        attributeTestCondition.Skill = skill;
    }
    public void SetSkillPossessedCondition(CharacterSkills skill, bool doWeWantTheSkill)
    {
        conditionTypeSet = ConditionTypes.SkillPossessed;

        if(skillPossessedCondition == null)
        {
            skillPossessedCondition = new SkillPossessedCondition();
        }

        skillPossessedCondition.IsNeeded = doWeWantTheSkill;
        skillPossessedCondition.SkillToCheck = skill;
    }

}

[System.Serializable]
public class AttributeCheckCondition : ConditionNodeBase
{
    public CharacterStat Attribute;
    public int ValueChecked;
    public InequalityTypes InequalityType;

    public override bool ConditionTest()
    {
        int attributeValue = -1;    //TODO: znajdź obiekt gracza i pobierz potrzebną wartość daną AttributeName

        return InequalityType.Value(attributeValue, ValueChecked);
    }
}

[System.Serializable]
public class AttributeTestCondition : ConditionNodeBase
{
    public CharacterStat Attribute;
    public int AttributeMod;
    public CharacterSkills Skill;

    public override bool ConditionTest()
    {
        int attributeValue = -1;    //TODO: znajdź obiekt gracza i pobierz potrzebną wartość daną AttributeName
        int stressPenalty = 0;      //TODO: znajdź obiekt gracza i pobierz wartość kary od stresu
        bool playerHasSkill = false;    //TODO: sprawdź czy gracz posiada umiejętność                       

        int randValue = Random.Range(0, 101);
        int valueToCheck =
            5 * (attributeValue + AttributeMod) + 
            (playerHasSkill ? 20 : 0) +
            stressPenalty
            ;

        return
            InequalityTypes.Less.Value(randValue, 5) ?
                true :
                    (InequalityTypes.Greater.Value(randValue, 95) ?
                        false : 
                        InequalityTypes.LessOrEqual.Value(randValue, valueToCheck)
                    );
    }
}

[System.Serializable]
public class SkillPossessedCondition : ConditionNodeBase
{
    public CharacterSkills SkillToCheck;
    public bool IsNeeded;

    public override bool ConditionTest()
    {
        bool playerHasSkill = false;    //TODO: sprawdzamy posiadanie skilla

        return
            IsNeeded ?
                playerHasSkill :
                !playerHasSkill;
    }
}