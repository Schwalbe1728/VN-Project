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
    private PlayerHasMoneyCondition playerHasMoneyCondition;

    [SerializeField]
    private StoryStateHappenedCondition storyStateHappenedCondition;

    [SerializeField]
    private WorldDateCondition worldDateCondition;

    [SerializeField]
    private WithinTimeRangeCondition worldDateRangeCondition;

    [SerializeField]
    private BackgroundRequiredCondition backgroundRequiredCondition;

    [SerializeField]
    private StatisticsCheckCondition statisticCheckCondition;

    public ConditionTypes ConditionType { get { return conditionTypeSet; } }

    public AttributeCheckCondition AttributeCheck { get { return (attributeCheckCondition != null)? attributeCheckCondition : new AttributeCheckCondition(); } }
    public AttributeTestCondition AttributeTest { get { return (attributeTestCondition != null)? attributeTestCondition : new AttributeTestCondition(); } }
    public SkillPossessedCondition SkillPossessed { get { return (skillPossessedCondition != null)? skillPossessedCondition : new SkillPossessedCondition(); } }
    public PlayerHasItemCondition PlayerHasItem { get { return (playerHasItemCondition != null) ? playerHasItemCondition : new PlayerHasItemCondition() ; } }
    public PlayerHasMoneyCondition PlayerHasMoney { get { return (playerHasMoneyCondition != null) ? playerHasMoneyCondition : new PlayerHasMoneyCondition(); } }
    public StoryStateHappenedCondition StoryStateHappened { get { return (storyStateHappenedCondition != null)? storyStateHappenedCondition : new StoryStateHappenedCondition(); } }
    public WorldDateCondition WorldDate { get { return (worldDateCondition != null)? worldDateCondition : new WorldDateCondition(); } }
    public WithinTimeRangeCondition WithinTimeRange { get { return (worldDateRangeCondition != null) ? worldDateRangeCondition : new WithinTimeRangeCondition(); } }
    public BackgroundRequiredCondition BackgroundRequired { get { return (backgroundRequiredCondition != null)? backgroundRequiredCondition : new BackgroundRequiredCondition(); } }
    public StatisticsCheckCondition StatisticCheck { get { return (statisticCheckCondition != null) ? statisticCheckCondition : new StatisticsCheckCondition(); } }

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

            case ConditionTypes.BackgroundRequired:
                return BackgroundRequired.ConditionTest();

            case ConditionTypes.PlayerHasMoney:
                return PlayerHasMoney.ConditionTest();

            case ConditionTypes.StatisticCheck:
                return StatisticCheck.ConditionTest();

            default:
                Debug.Log("Wtf, ConditionTest default");
                return false;
        }
    }    

    public void SetAttributeCheckCondition(CharacterAttribute stat, int value, InequalityTypes type)
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
    public void SetAttributeTestCondition(CharacterAttribute stat, int mod, CharacterSkills skill)
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
    public void SetPlayerHasItemCondition(ItemScript item, int quantity, bool isRequired)
    {
        conditionTypeSet = ConditionTypes.PlayerHasItem;

        if(playerHasItemCondition == null)
        {
            playerHasItemCondition = new PlayerHasItemCondition();
        }

        playerHasItemCondition.Item = item;
        playerHasItemCondition.Quantity = quantity;
        playerHasItemCondition.IsRequired = isRequired;
    }

    public void SetPlayerHasMoneyCondition(int abstractValueRequired)
    {
        conditionTypeSet = ConditionTypes.PlayerHasMoney;

        if(playerHasMoneyCondition == null)
        {
            playerHasMoneyCondition = new PlayerHasMoneyCondition();
        }

        playerHasMoneyCondition.AbstractValue = abstractValueRequired;
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

    public void SetBackgroundRequiredCondition(BackgroundDefinition background, bool isRequired)
    {
        conditionTypeSet = ConditionTypes.BackgroundRequired;

        if(backgroundRequiredCondition == null)
        {
            backgroundRequiredCondition = new BackgroundRequiredCondition();
        }

        backgroundRequiredCondition.Background = background;
        backgroundRequiredCondition.Required = isRequired;
    }

    public void SetStatisticCheckCondition(CharacterStatistic statistic, InequalityTypes ineqType, float valueToCheck)
    {
        conditionTypeSet = ConditionTypes.StatisticCheck;

        if(statisticCheckCondition == null)
        {
            statisticCheckCondition = new StatisticsCheckCondition();
        }

        statisticCheckCondition.InequalityType = ineqType;
        statisticCheckCondition.Statistic = statistic;
        statisticCheckCondition.ValueChecked = valueToCheck;
    }
}

[System.Serializable]
public class AttributeCheckCondition : ConditionNodeBase
{
    public CharacterAttribute Attribute;
    public int ValueChecked;
    public InequalityTypes InequalityType;

    public override bool ConditionTest()
    {
        int attributeValue =
            GameObject.Find("Game Info Component").
            GetComponent<CharacterInfoScript>().
            GetStat(Attribute);

        return InequalityType.Value(attributeValue, ValueChecked);
    }
}

[System.Serializable]
public class AttributeTestCondition : ConditionNodeBase
{
    public CharacterAttribute Attribute;
    public int AttributeMod;
    public CharacterSkills Skill;

    public override bool ConditionTest()
    {
        CharacterInfoScript characterInfo = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>();

        bool result =
            StatsRuleSet.TestRNG(characterInfo, Attribute, AttributeMod, Skill);        

        return
            result;
    }
}

[System.Serializable]
public class SkillPossessedCondition : ConditionNodeBase
{
    public CharacterSkills SkillToCheck;
    public bool IsNeeded = true;

    public override bool ConditionTest()
    {
        bool playerHasSkill =
            GameObject.Find("Game Info Component").
            GetComponent<CharacterInfoScript>().HasSkill(SkillToCheck);

        return
            IsNeeded ?
                playerHasSkill :
                !playerHasSkill;
    }
}

[System.Serializable]
public class PlayerHasItemCondition : ConditionNodeBase
{
    public ItemScript Item;
    public int Quantity;
    public bool IsRequired = true;

    public override bool ConditionTest()
    {
        PlayerEquipmentScript equipment =
            GameObject.Find("Game Info Component").GetComponent<PlayerEquipmentScript>();

        bool containsItems = equipment.HasItem(Item, Quantity);

        return
            (IsRequired) ?
                containsItems : !containsItems;
    }
}

[System.Serializable]
public class PlayerHasMoneyCondition : ConditionNodeBase
{
    public int AbstractValue;

    public override bool ConditionTest()
    {
        PlayerEquipmentScript equipment =
            GameObject.Find("Game Info Component").GetComponent<PlayerEquipmentScript>();

        return equipment.AbstractMoneyValue >= AbstractValue;
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

[System.Serializable]
public class BackgroundRequiredCondition : ConditionNodeBase
{
    public BackgroundDefinition Background;
    public bool Required = true;

    public override bool ConditionTest()
    {
        BackgroundDefinition
            playerBackground = GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>().Background;

        return
            Background != null &&
            (Required) ?
                playerBackground.Name.Equals(Background.Name) :
                !playerBackground.Name.Equals(Background.Name);
    }
}

[System.Serializable]
public class StatisticsCheckCondition : ConditionNodeBase
{
    public CharacterStatistic Statistic;
    public float ValueChecked;
    public InequalityTypes InequalityType;

    public override bool ConditionTest()
    {
        float attributeValue =
            GameObject.Find("Game Info Component").
            GetComponent<CharacterInfoScript>().
            GetStat(Statistic);

        return InequalityType.Value(attributeValue, ValueChecked);
    }
}