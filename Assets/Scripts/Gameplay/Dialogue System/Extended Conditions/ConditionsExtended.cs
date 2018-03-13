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

    [SerializeField]
    private PlayerHasItemCondition playerHasItemCondition;

    [SerializeField]
    private StoryStateHappenedCondition storyStateHappenedCondition;

    [SerializeField]
    private WorldDateCondition worldDateCondition;

    [SerializeField]
    private WithinTimeRangeCondition worldDateRangeCondition;

    public ConditionTypes ConditionType { get { return conditionTypeSet; } }

    public AttributeCheckCondition AttributeCheck { get { return attributeCheckCondition; } }
    public AttributeTestCondition AttributeTest { get { return attributeTestCondition; } }
    public SkillPossessedCondition SkillPossessed { get { return skillPossessedCondition; } }
    public PlayerHasItemCondition PlayerHasItem { get { return playerHasItemCondition; } }
    public StoryStateHappenedCondition StoryStateHappened { get { return storyStateHappenedCondition; } }
    public WorldDateCondition WorldDate { get { return worldDateCondition; } }
    public WithinTimeRangeCondition WithinTimeRange { get { return worldDateRangeCondition; } }

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

            case ConditionTypes.PlayerHasItem:
                return playerHasItemCondition.ConditionTest();

            case ConditionTypes.StoryStateHappened:
                return storyStateHappenedCondition.ConditionTest();

            case ConditionTypes.WithinTimeRange:
                return WithinTimeRange.ConditionTest();

            case ConditionTypes.WorldDate:
                return WorldDate.ConditionTest();

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
    public void SetPlayerHasItemCondition(/*EquipmentItem item, */ bool isRequired)
    {
        conditionTypeSet = ConditionTypes.PlayerHasItem;

        if(playerHasItemCondition == null)
        {
            playerHasItemCondition = new PlayerHasItemCondition();
        }

        //playerHasItemCondition.Item = item;
        playerHasItemCondition.IsRequired = isRequired;
    }
    public void SetStoryStateHappenedCondition(string plotName, string stateName, bool hasHappened)
    {
        conditionTypeSet = ConditionTypes.StoryStateHappened;

        if(storyStateHappenedCondition == null)
        {
            storyStateHappenedCondition = new StoryStateHappenedCondition();
        }

        storyStateHappenedCondition.HasHappened = hasHappened;
        storyStateHappenedCondition.PlotName = plotName;
        storyStateHappenedCondition.StateName = stateName;
    }

    public void SetWorldDateCondition(WorldDate date, InequalityTypes beforeOrAfter)
    {
        conditionTypeSet = ConditionTypes.WorldDate;

        if(WorldDate == null)
        {
            worldDateCondition = new WorldDateCondition();
        }

        worldDateCondition.Date = date;
        worldDateCondition.TimeOrientation = beforeOrAfter;
    }

    public void SetWithinTimeRangeCondition(WorldDate startDate, WorldDate finishDate, bool hoursOnly)
    {
        conditionTypeSet = ConditionTypes.WithinTimeRange;

        if(worldDateRangeCondition == null)
        {
            worldDateRangeCondition = new WithinTimeRangeCondition();
        }

        worldDateRangeCondition.Finish = finishDate;
        worldDateRangeCondition.Start = startDate;
        worldDateRangeCondition.HoursOnly = hoursOnly;

        if(!worldDateRangeCondition.ValidateParametres(hoursOnly))
        {
            Debug.LogWarning("Incorrect parametres values! Start of the range should be earlier than it's finish");
        }
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

//TO FINISH
[System.Serializable]
public class PlayerHasItemCondition : ConditionNodeBase
{
    //public EquipmentItem Item;
    public bool IsRequired;

    public override bool ConditionTest()
    {
        return base.ConditionTest();
    }
}

//TO FINISH
[System.Serializable]
public class StoryStateHappenedCondition : ConditionNodeBase
{
    public string PlotName;
    public string StateName;
    public bool HasHappened;

    public override bool ConditionTest()
    {


        return base.ConditionTest();
    }
}

[System.Serializable]
public class WorldDateCondition : ConditionNodeBase
{
    public WorldDate Date;
    public InequalityTypes TimeOrientation;

    public override bool ConditionTest()
    {
        WorldDate currentDate = TimeManagerScript.GetDate();

        return Date.CompareTo(currentDate, TimeOrientation, false, false);
    }
}

[System.Serializable]
public class WithinTimeRangeCondition : ConditionNodeBase
{
    public WorldDate Start;
    public WorldDate Finish;

    public bool HoursOnly;

    public override bool ConditionTest()
    {
        WorldDate currentDate = TimeManagerScript.GetDate();
        bool finishIsNextDay = 
            (HoursOnly)? Start.ToSecondsHoursOnly() > Finish.ToSecondsHoursOnly() : false;
        bool currentIsNextDay = finishIsNextDay && Finish.ToSecondsHoursOnly() >= currentDate.ToSecondsHoursOnly();


        return
            (HoursOnly) ?
                CompareHoursOnly(currentDate, currentIsNextDay, finishIsNextDay) :
                CompareFullDates(currentDate);
    }

    private bool CompareHoursOnly(WorldDate currentDate, bool currentIsNextDay, bool finishIsNextDay)
    {
        return 
            Start.ToSecondsHoursOnly() <= currentDate.ToSecondsHoursOnly(currentIsNextDay) &&
            Finish.ToSecondsHoursOnly(finishIsNextDay) >= currentDate.ToSecondsHoursOnly();
    }

    private bool CompareFullDates(WorldDate currentDate)
    {
        return
            ValidateParametres() &&
            Start.ToSeconds() <= currentDate.ToSeconds() &&
            Finish.ToSeconds() >= currentDate.ToSeconds();
    }

    public bool ValidateParametres(bool hoursOnly = false)
    {
        return hoursOnly || Start.ToSeconds() <= Finish.ToSeconds();
    }
}