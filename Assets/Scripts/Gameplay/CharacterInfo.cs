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

    public static bool TestRNG(CharacterInfoScript charInfo, CharacterStat statToTest, int modificator)
    {
        int diceResult = UnityEngine.Random.Range(1, 100);

        if (diceResult <= 5) return true;
        if (diceResult >= 95) return false;

        int baseToPass = 5 * (charInfo.GetStat(statToTest) + modificator);        
        int stressPenalty = charInfo.StressPenalty;

        return diceResult <= (baseToPass - stressPenalty);
    }

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
        }

        throw new System.InvalidCastException();
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
            Mod1 * charInfo.Stats[CharacterStat.Physique] + 
            Mod2 * charInfo.Stats[CharacterStat.Character];

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
            Mod1 * charInfo.Stats[CharacterStat.Physique] +
            Mod2 * charInfo.Stats[CharacterStat.Agility];
            

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
            5 * charInfo.Stats[CharacterStat.Physique] +
            2 * charInfo.Stats[CharacterStat.Character];

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
            modCh * charInfo.Stats[CharacterStat.Character] +
            modWt * charInfo.Stats[CharacterStat.Wits];

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
                        (CHMod * charInfo.Stats[CharacterStat.Character] + WTMod * charInfo.Stats[CharacterStat.Wits]) / ((float)CHMod + WTMod))
                     * 15 / 19.0f);
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

    public void SetPlayerInfo(CharacterInfo info)
    {
        PlayerInfo = info;

        currentHealth = MaxHealthPoints;
        currentSanity = MaxSanity;
    }

    public int GetStat(CharacterStat stat)
    {
        return PlayerInfo.Stats[stat];
    }

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
    public int Agility { get { return PlayerInfo.Stats[CharacterStat.Agility]; } }
    public int Perception { get { return PlayerInfo.Stats[CharacterStat.Perception]; } }
    public int Character { get { return PlayerInfo.Stats[CharacterStat.Character]; } }
    public int Wits { get { return PlayerInfo.Stats[CharacterStat.Wits]; } }
    public int Physique { get { return PlayerInfo.Stats[CharacterStat.Physique]; } }
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
            return Mathf.RoundToInt(StatsRuleSet.MaxStressPenalty(this) * StressModificator );
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
    public int HealRate { get { return StatsRuleSet.HealRate(this); } }
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
        float healValue = HealRate / 60.0f;

        ChangeHealth(healValue);

        if(UnityEngine.Random.Range(0, 100) < 66 - 2 * StressPenalty)
        {
            ChangeSanity(healValue);
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

public enum CharacterStat
{
    Agility,
    Perception,
    Character,
    Wits,
    Physique
}

public static class CharacterStatExtension
{

    public static CharacterStat FromString(string from)
    {
        from = from.ToLower();

        switch (from)
        {
            case "agility":
                return CharacterStat.Agility;

            case "perception":
                return CharacterStat.Perception;

            case "character":
                return CharacterStat.Character;

            case "wits":
                return CharacterStat.Wits;

            case "physique":
                return CharacterStat.Physique;

            case "ag":
                return CharacterStat.Agility;

            case "pe":
                return CharacterStat.Perception;

            case "ch":
                return CharacterStat.Character;

            case "wt":
                return CharacterStat.Wits;

            case "ph":
                return CharacterStat.Physique;
        }

        throw new InvalidCastException();
    }

    public static CharacterStat FromString(this CharacterStat stat, string from)
    {
        return FromString(from);
    }

    public static string GetString(this CharacterStat stat)
    {
        switch (stat)
        {
            case CharacterStat.Agility:
                return "Agility";

            case CharacterStat.Character:
                return "Character";

            case CharacterStat.Perception:
                return "Perception";

            case CharacterStat.Physique:
                return "Physique";

            case CharacterStat.Wits:
                return "Wits";
        }

        throw new InvalidOperationException();
    }
}

public class CharacterInfo
{
    public string CharName { get { return charName; } }

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

    private string charName;

    private CharStatsToValueDictionary stats;   //  Final Stats    
    private bool CharacterCreationInProgress;            

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
}
