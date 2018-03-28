using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerDied();
public delegate void PlayerHurt();
public delegate void HealthChanged(float healthPercentage);

public delegate void SanityChanged(float sanityPercentage);

public static class StatsRuleSet
{
    private static float healPercentPerHour = 0.02f;

    public static bool TestRNG(CharacterInfoScript charInfo, CharacterAttribute statToTest, int modificator, CharacterSkills skill)
    {
        int diceResult = UnityEngine.Random.Range(1, 100);
        bool result = false;
        bool crit = false;

        if (diceResult <= 5) { result = true; crit = true; }
        if (diceResult >= 95) { result = false; crit = true; }

        if (!crit)
        {
            int baseToPass = 5 * (charInfo.GetStat(statToTest) + modificator);
            int stressPenalty = charInfo.StressPenalty;
            int skillBonus = (charInfo.HasSkill(skill)) ? 20 : 0;

            result = diceResult <= (baseToPass + skillBonus - stressPenalty);
        }

        Debug.Log("Rand Value: " + diceResult + ", result: " + result);

        return result;
    }
    /*
    public static int StringToStatValue(string statString, CharacterInfoScript charInfo)
    {
        switch(statString.ToLower())
        {
            case "hp":
                return MaxHealthPoints(charInfo);

            case "sanity":
                return MaxSanity(charInfo);

            case "bdmg":
                return BaseDamage(charInfo);

            case "hr":
                return HealRate(charInfo);

            case "maxstresspenalty":
                return MaxStressPenalty(charInfo);

            case "speed":
                return WalkSpeedPerHour(charInfo);
        }

        throw new System.InvalidCastException();
    }

    public static int StringToStatValue(string statString, CharacterInfo charInfo)
    {
        switch (statString.ToLower())
        {
            case "hp":
                return MaxHealthPoints(charInfo);

            case "sanity":
                return MaxSanity(charInfo);

            case "basedmg":
                return BaseDamage(charInfo);

            case "healrate":
                return HealRate(charInfo);

            case "maxstresspenalty":
                return MaxStressPenalty(charInfo);

            case "speed":
                return WalkSpeedPerHour(charInfo);
        }

        throw new System.InvalidCastException();
    }
    */
    /*
    public static string StringToStatValueString(string statString, CharacterInfoScript charInfo)
    {
        switch (statString.ToLower())
        {
            case "hp":
                return MaxHealthPoints(charInfo).ToString();

            case "sanity":
                return MaxSanity(charInfo).ToString();

            case "bdmg":
                return BaseDamage(charInfo).ToString();

            case "hr":
                return
                    VitalityHealRate(charInfo).ToString("n1") + "/" +
                    SanityHealRate(charInfo).ToString("n1");
                    ;

            case "maxstresspenalty":
                return MaxStressPenalty(charInfo).ToString();

            case "speed":
                return WalkSpeedPerHour(charInfo).ToString();
        }

        throw new System.InvalidCastException();
    }

    public static string StringToStatValueString(string statString, CharacterInfo charInfo)
    {
        switch (statString.ToLower())
        {
            case "hp":
                return MaxHealthPoints(charInfo).ToString();

            case "sanity":
                return MaxSanity(charInfo).ToString();

            case "basedmg":
                return BaseDamage(charInfo).ToString();

            case "healrate":
                return
                    VitalityHealRate(charInfo).ToString("n1") + "/" +
                    SanityHealRate(charInfo).ToString("n1");                

            case "maxstresspenalty":
                return MaxStressPenalty(charInfo).ToString();

            case "speed":
                return WalkSpeedPerHour(charInfo).ToString();
        }

        throw new System.InvalidCastException();
    }
    */

    public static float GetStatValue(CharacterInfo charInfo, CharacterStatistic stat)
    {
        switch (stat)
        {
            case CharacterStatistic.BaseDamage:
                return BaseDamage(charInfo);

            case CharacterStatistic.HealRateSanity:
                return SanityHealRate(charInfo);

            case CharacterStatistic.HealRateVitality:
                return VitalityHealRate(charInfo);

            case CharacterStatistic.Sanity:
                return MaxSanity(charInfo);

            case CharacterStatistic.Vitality:
                return MaxHealthPoints(charInfo);

            case CharacterStatistic.WalkingSpeed:
                return WalkSpeedPerHour(charInfo);
        }

        throw new NotImplementedException();
    }

    public static string GetStatValueString(CharacterInfo charInfo, CharacterStatistic stat)
    {
        string result = "";

        try
        {
            float value = GetStatValue(charInfo, stat);

            switch (stat)
            {
                case CharacterStatistic.BaseDamage:
                    result = (value - 1).ToString("n0") + " - " + (value + 1).ToString("n0");
                    break;

                case CharacterStatistic.WalkingSpeed:
                    result = (value / 1000).ToString("n1");
                    break;

                default:
                    result = value.ToString("n0");
                    break;
            }
        }
        catch (NotImplementedException e)
        {
            switch (stat)
            {
                case CharacterStatistic.HealRatesCombined:
                    return VitalityHealRate(charInfo).ToString("n1") + "/" +
                    SanityHealRate(charInfo).ToString("n1");
            }
        }

        return result;
    }

    public static string GetStatValueString(CharacterInfoScript charInfo, CharacterStatistic stat)
    {
        string result = "";

        try
        {
            float value = GetStatValue(charInfo, stat);            

            switch(stat)
            {
                case CharacterStatistic.BaseDamage:
                    result = (value - 1).ToString("n0") + " - " + (value + 1).ToString("n0");
                    break;

                case CharacterStatistic.WalkingSpeed:
                    result = (value / 1000).ToString("n1");
                    break;

                default:
                    result = value.ToString("n0");
                    break;
            }
        }
        catch( NotImplementedException e )
        {
            switch(stat)
            {                
                case CharacterStatistic.HealRatesCombined:
                    return VitalityHealRate(charInfo).ToString("n1") + "/" +
                    SanityHealRate(charInfo).ToString("n1");
            }
        }

        return result;
    }

    public static float GetStatValue(CharacterInfoScript charInfo, CharacterStatistic stat)
    {
        switch(stat)
        {
            case CharacterStatistic.BaseDamage:
                return BaseDamage(charInfo);

            case CharacterStatistic.HealRateSanity:
                return SanityHealRate(charInfo);

            case CharacterStatistic.HealRateVitality:
                return VitalityHealRate(charInfo);

            case CharacterStatistic.Sanity:
                return MaxSanity(charInfo);

            case CharacterStatistic.Vitality:
                return MaxHealthPoints(charInfo);

            case CharacterStatistic.WalkingSpeed:
                return WalkSpeedPerHour(charInfo);
        }

        throw new NotImplementedException();
    }

    public static int MaxHealthPoints(CharacterInfoScript charInfo)
    {
        int Mod1 = 5;
        int Mod2 = 2;
        float Mod3 = 10f;

        int result =
            Mod1 * charInfo.Physique +             
            Mod2 * charInfo.Character;

        return Mathf.RoundToInt(Mod3 * result / (Mod1 + Mod2) );
    }

    public static int MaxHealthPoints(CharacterInfo charInfo)
    {
        int Mod1 = 5;
        int Mod2 = 2;
        float Mod3 = 10f;

        int result =
            Mod1 * charInfo.Stats[CharacterAttribute.Physique] + 
            Mod2 * charInfo.Stats[CharacterAttribute.Character];

        return Mathf.RoundToInt(Mod3 * result / (Mod1 + Mod2));
    }

    public static int BaseDamage(CharacterInfoScript charInfo)
    {
        int Mod1 = 5;
        int Mod2 = 3;
        float Mod3 = 1f;

        int result =
            Mod1 * charInfo.Physique +
            Mod2 * charInfo.Agility;            

        return Mathf.RoundToInt( Mod3 * result / (Mod1 + Mod2) );
    }

    public static int BaseDamage(CharacterInfo charInfo)
    {
        int Mod1 = 5;
        int Mod2 = 3;
        float Mod3 = 1f;

        int result =
            Mod1 * charInfo.Stats[CharacterAttribute.Physique] +
            Mod2 * charInfo.Stats[CharacterAttribute.Agility];
            

        return Mathf.RoundToInt( Mod3 * result / (Mod1 + Mod2) );
    }

    [Obsolete]
    public static int HealRate(CharacterInfoScript charInfo)
    {
        int result =
            5 * charInfo.Physique +
            2 * charInfo.Character;

        return 1 + Mathf.RoundToInt(result / 30.0f);
    }

    [Obsolete]
    public static int HealRate(CharacterInfo charInfo)
    {
        int result =
            5 * charInfo.Stats[CharacterAttribute.Physique] +
            2 * charInfo.Stats[CharacterAttribute.Character];

        return 1 + Mathf.RoundToInt(result / 30.0f);
    }

    public static float VitalityHealRate(CharacterInfo charInfo)
    {
        return healPercentPerHour * MaxHealthPoints(charInfo);
    }

    public static float VitalityHealRate(CharacterInfoScript charInfo)
    {
        return healPercentPerHour * MaxHealthPoints(charInfo);
    }

    public static float SanityHealRate(CharacterInfo charInfo)
    {
        return healPercentPerHour * MaxSanity(charInfo);
    }

    public static float SanityHealRate(CharacterInfoScript charInfo)
    {
        return healPercentPerHour * MaxSanity(charInfo);
    }

    public static int MaxSanity(CharacterInfo charInfo)
    {
        int modCh = 2;
        int modWt = 5;
        float Mod3 = 10f;

        int result =
            modCh * charInfo.Stats[CharacterAttribute.Character] +
            modWt * charInfo.Stats[CharacterAttribute.Wits];

        return Mathf.RoundToInt(Mod3 * result / (modCh + modWt));
    }

    public static int MaxSanity(CharacterInfoScript charInfo)
    {
        int modCh = 2;
        int modWt = 5;
        float Mod3 = 10f;

        int result =
            modCh * charInfo.Character +
            modWt * charInfo.Wits;

        return Mathf.RoundToInt(Mod3 * result / (modCh + modWt));
    }

    [Obsolete]
    public static int MaxStressPenalty(CharacterInfoScript charInfo)
    {
        int CHMod = 5;
        int WTMod = 3;        

        return
            10 + Mathf.RoundToInt(
                    (20 - 
                        (CHMod * charInfo.Character + WTMod * charInfo.Wits) / ((float) CHMod + WTMod )) 
                     * 15 / 19.0f);
    }

    [Obsolete]
    public static int MaxStressPenalty(CharacterInfo charInfo)
    {
        int CHMod = 5;
        int WTMod = 3;

        return
            10 + Mathf.RoundToInt(
                    (20 -
                        (CHMod * charInfo.Stats[CharacterAttribute.Character] + WTMod * charInfo.Stats[CharacterAttribute.Wits]) / ((float)CHMod + WTMod))
                     * 15 / 19.0f);
    }

    private static int[] StressThresholds = 
        new int[] { 3, 6, 10, 16, 23, 31, 40, 50, 61, 73, 86, 95, 100  };
    private static int[] StressPenalties = 
        new int[] { 5, 3, 0, -5, -10, -16, -23, -30, -38, -47, -57, -64, -68 };

    public static int StressPenalty(CharacterInfoScript charInfo)
    {
        float currentStress = 100 * Stress(charInfo);
        int result = 0;        

        for(int i = 0; i < StressThresholds.Length; i++)
        {
            if(StressThresholds[i] > currentStress)
            {
                result = StressPenalties[i];
                break;
            }            
        }

        return result;
    }

    public static float Stress(CharacterInfoScript charInfo)
    {
        return StressM2(charInfo);
    }

    private static float StressM1(CharacterInfoScript charInfo)
    {
        return
            (1 - ((float)(charInfo.CurrentHealth + charInfo.CurrentSanity)) / (charInfo.MaxHealthPoints + charInfo.MaxSanity));
    }

    private static float StressM2(CharacterInfoScript charInfo)
    {
        return
            1 - (charInfo.HealthPercentage + charInfo.SanityPercentage)/2;
    }

    private static int WalkingSpeedMin = 2500;
    private static int WalkingSpeedMax = 5000;

    public static int WalkSpeedPerHour(CharacterInfoScript charInfo)
    {
        int AGMod = 4;
        int PHMod = 1;

        float walkingAttributeValue = 
            ((float) AGMod * (charInfo.Agility - 1 ) + PHMod * (charInfo.Physique - 1) ) / (AGMod + PHMod);
        float value = WalkingSpeedMin + (WalkingSpeedMax - WalkingSpeedMin) * walkingAttributeValue / 19;

        return Mathf.RoundToInt( value );
    }

    public static int WalkSpeedPerHour(CharacterInfo charInfo)
    {
        int AGMod = 4;
        int PHMod = 1;

        float walkingAttributeValue =
            ((float)AGMod * (charInfo.Stats[CharacterAttribute.Agility] - 1 ) + 
                PHMod * (charInfo.Stats[CharacterAttribute.Physique] - 1 )) / (AGMod + PHMod);
        float value = WalkingSpeedMin + (WalkingSpeedMax - WalkingSpeedMin) * walkingAttributeValue / 19;

        return Mathf.RoundToInt(value);
    }
}

public class CharacterInfoScript : MonoBehaviour
{
    public Sprite CharacterPortrait { get { return portrait; } }

    private CharacterInfo PlayerInfo;

    private event PlayerDied OnPlayerDied;
    private event PlayerHurt OnPlayerHurt;
    private event HealthChanged OnHealthChanged;

    private event SanityChanged OnSanityChanged;

    private Sprite portrait;

    private bool Asleep;
    private int MinutesOfSleepLeft;

    public void SetPlayerInfo(CharacterInfo info)
    {
        PlayerInfo = info;

        currentHealth = MaxHealthPoints;
        currentSanity = MaxSanity;
    }

    public int GetStat(CharacterAttribute stat)
    {
        return PlayerInfo.Stats[stat];
    }

    public bool HasSkill(CharacterSkills skill)
    {
        return PlayerInfo.HasSkill(skill);
    }

    public BackgroundDefinition Background { get { return PlayerInfo.Background; } }

    public void RegisterToPlayerDied(PlayerDied function)
    {
        OnPlayerDied += function;
    }

    public void RegisterToHealthChanged(HealthChanged function)
    {
        OnHealthChanged += function;
    }

    public void RegisterToPlayerHurt(PlayerHurt function)
    {
        OnPlayerHurt += function;
    }

    public void RegisterToSanityChanged(SanityChanged function)
    {
        OnSanityChanged += function;
    }

    public string Name { get { return PlayerInfo.CharName; } }

    #region Basic Stats
    public int Agility { get { return PlayerInfo.Stats[CharacterAttribute.Agility]; } }
    public int Perception { get { return PlayerInfo.Stats[CharacterAttribute.Perception]; } }
    public int Character { get { return PlayerInfo.Stats[CharacterAttribute.Character]; } }
    public int Wits { get { return PlayerInfo.Stats[CharacterAttribute.Wits]; } }
    public int Physique { get { return PlayerInfo.Stats[CharacterAttribute.Physique]; } }
    #endregion

    #region Health Points
    public int MaxHealthPoints { get { return StatsRuleSet.MaxHealthPoints(this); } }
    public int CurrentHealth { get { return Mathf.RoundToInt(currentHealth); } }
    public float HealthPercentage { get { return Mathf.Clamp((float)CurrentHealth / MaxHealthPoints, 0, 1); } }   

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;

        //Debug.Log("HP Left: " + (100 * HealthPercentage).ToString("N0"));

        if (OnHealthChanged != null)
        {
            OnHealthChanged(HealthPercentage);
        }

        if (OnPlayerHurt != null && amount < 0)
        {
            OnPlayerHurt();            
        }

        if(CurrentHealth <= 0f)
        {
            if(OnPlayerDied != null)
            {
                OnPlayerDied();
            }
        }
    }

    public void ChangeSanity(float amount)
    {
        currentSanity += amount;

        if(OnSanityChanged != null)
        {
            OnSanityChanged(SanityPercentage);
        }
    }
    
    public int StressPenalty
    {
        get
        {
            return //Mathf.RoundToInt(StatsRuleSet.MaxStressPenalty(this) * StressModificator );
                StatsRuleSet.StressPenalty(this);
        }
    }

    public int MaxSanity { get { return StatsRuleSet.MaxSanity(this); } }

    public int CurrentSanity
    {
        get { return Mathf.RoundToInt(currentSanity); }
    }

    public float SanityPercentage
    {
        get { return currentSanity / MaxSanity; }
    }

    public bool GoToSleep(int hours)
    {
        if(!Asleep && MinutesOfSleepLeft <= 0)
        {
            Asleep = true;
            MinutesOfSleepLeft = Mathf.RoundToInt(60 * hours * UnityEngine.Random.Range(0.95f, 1.05f));
        }

        return Asleep;
    }

    public void WakePlayerUp(int hours)
    {
        Asleep = false;
        MinutesOfSleepLeft = Mathf.RoundToInt(60 * hours * UnityEngine.Random.Range(0.95f, 1.05f));
    }

    private float currentHealth { get { return Mathf.Clamp(trueHealthValue, 0, MaxHealthPoints); } set { trueHealthValue = Mathf.Min(value, MaxHealthPoints); } }
    private float trueHealthValue;

    private float currentSanity
    {
        get { return Mathf.Clamp(trueSanityValue, 0, MaxSanity); }
        set { trueSanityValue = Mathf.Min(value, MaxSanity); }
    }
    private float trueSanityValue;

    private float StressModificator
    {
        get
        {
            return 1 - ((float)(CurrentSanity + CurrentHealth) / (MaxSanity + MaxHealthPoints));
        }
    }

    #endregion

    #region Damage Stats
    public int BaseDamage { get { return StatsRuleSet.BaseDamage(this); } }
    #endregion

    #region Heal Rate
    //public int HealRate { get { return StatsRuleSet.HealRate(this); } }
    public float HealRateVitality { get { return MaxHealthPoints * 0.02f * ((Asleep)? 3 : 1); } }
    public float HealRateSanity { get { return MaxSanity * 0.02f * ((Asleep)? 3 : 1); } }
    #endregion

    void Awake()
    {
        CharacterCreationScript toDestroy = gameObject.GetComponent<CharacterCreationScript>();
        Destroy(toDestroy, 1.0f);

        DontDestroyOnLoad(gameObject);
        StartCoroutine(TryRegisteringToTimeEvent());

        portrait = GameObject.Find("Displayed Portrait").GetComponent<Image>().sprite;

        //RegisterToPlayerHurt(Test);
        RegisterToSanityChanged(SaveSanityPercentageBetweenGames);
    }

    private void HealPerMinute()
    {        
        ChangeHealth(HealRateVitality / 60.0f);

        if(UnityEngine.Random.Range(0, 100) < 66 - 2 * StressPenalty)
        {
            ChangeSanity(HealRateSanity / 60.0f);
        }

        if(MinutesOfSleepLeft > 0)
        {
            MinutesOfSleepLeft--;            
        }
        else
        {
            if(Asleep)
            {
                WakePlayerUp(10);
            }
        }
    }

    private IEnumerator TryRegisteringToTimeEvent()
    {
        GameObject timeManager = null;

        while(timeManager == null)
        {
            timeManager = GameObject.Find("Time Manager");
            yield return new WaitForSeconds(0.5f);
        }

        timeManager.GetComponent<TimeManagerScript>().OnMinutePassed += HealPerMinute;
    }
    
    private void SaveSanityPercentageBetweenGames(float percentage)
    {
        PlayerPrefs.SetFloat("Last Session Sanity", percentage);
    }
}

public enum CharacterAttribute
{
    Agility,
    Perception,
    Character,
    Wits,
    Physique
}

public enum CharacterStatistic
{
    Vitality,
    Sanity,
    BaseDamage,
    HealRatesCombined,
    HealRateVitality,
    HealRateSanity,
    WalkingSpeed
}

public static class CharacterStatExtension
{
    public static CharacterStatistic FromStringStatistic(string from)
    {
        from = from.ToLower();

        switch(from)
        {
            case "basedmg":
                return CharacterStatistic.BaseDamage;

            case "hrsanity":
                return CharacterStatistic.HealRateSanity;

            case "hrvitality":
                return CharacterStatistic.HealRateVitality;

            case "hrcombined":
                return CharacterStatistic.HealRatesCombined;

            case "sanity":
                return CharacterStatistic.Sanity;

            case "vitality":
                return CharacterStatistic.Vitality;

            case "walkspeed":
                return CharacterStatistic.WalkingSpeed;
        }

        throw new InvalidCastException();
    }


    public static CharacterAttribute FromString(string from)
    {
        from = from.ToLower();

        switch (from)
        {
            case "agility":
                return CharacterAttribute.Agility;

            case "perception":
                return CharacterAttribute.Perception;

            case "character":
                return CharacterAttribute.Character;

            case "wits":
                return CharacterAttribute.Wits;

            case "physique":
                return CharacterAttribute.Physique;

            case "ag":
                return CharacterAttribute.Agility;

            case "pe":
                return CharacterAttribute.Perception;

            case "ch":
                return CharacterAttribute.Character;

            case "wt":
                return CharacterAttribute.Wits;

            case "ph":
                return CharacterAttribute.Physique;
        }

        throw new InvalidCastException();
    }

    public static string GetString(this CharacterAttribute stat)
    {
        switch (stat)
        {
            case CharacterAttribute.Agility:
                return "Agility";

            case CharacterAttribute.Character:
                return "Character";

            case CharacterAttribute.Perception:
                return "Perception";

            case CharacterAttribute.Physique:
                return "Physique";

            case CharacterAttribute.Wits:
                return "Wits";
        }

        throw new InvalidOperationException();
    }
}

[Serializable]
public class CharacterInfo
{
    public string CharName { get { return charName; } }
    public BackgroundDefinition Background { get { return background; } set { if (CharacterCreationInProgress) background = value; } }

    public CharStatsToValueDictionary Stats
    {
        get
        {
            return stats;
        }
        set
        {
            if(CharacterCreationInProgress)
            {
                stats = value;
            }
        }
    }
    
    public void SetSkills(List<CharacterSkills> skillList)
    {
        if(skills == null)
        {
            skills = new HashSet<CharacterSkills>(skillList);

            foreach (CharacterSkills skill in skillList)
            {             
                Debug.Log("Dodaje skill: " + skill.ToString(false));
            }
        }
        else
        {
            skills.Clear();
            
            foreach(CharacterSkills skill in skillList)
            {
                skills.Add(skill);
                Debug.Log("Dodaje skill: " + skill.ToString(false));
            }
        }
    }       

    [SerializeField]
    private string charName;

    private CharStatsToValueDictionary stats;   //  Final Stats    
    private HashSet<CharacterSkills> skills;

    private bool CharacterCreationInProgress;
    private BackgroundDefinition background;

    public CharacterInfo()
    {
        stats = new CharStatsToValueDictionary(10);        
        CharacterCreationInProgress = true;        
    }

    public void FinishCreation()
    {
        CharacterCreationInProgress = false;
    }

    public bool ChangeName(string name)
    {
        if (CharacterCreationInProgress)
        {
            charName = name;
        }

        return CharacterCreationInProgress;
    }
    
    public bool HasSkill(CharacterSkills skill)
    {
        return skills.Contains(skill);
    }
}
