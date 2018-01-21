﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void NotifyStatChange();

public class CharacterCreationScript : MonoBehaviour
{
    public CharacterInfo PlayerInfo { get { return playerInfo; } }
    public List<CharacterSkills> Skills { get; set; }    

    private CharacterInfo playerInfo;
    private ScreenFadeInFadeOut scr;

    private InputField inputField;
    
    private CharStatsToValueDictionary minimumStats;    //  Minimum possible stats
    private CharStatsToValueDictionary statModifiers;
    private CharStatsToValueDictionary pointsSpent;

    public int StatPointsLeft { get { return statPointsLeft; } }
    public int BonusStatPoints { get; set; }

    [SerializeField]
    private int StartingStatPoints = 5;
    private int statPointsLeft;

    private static int MAX_STAT = 20;
    private static int MIN_STAT = 1;

    private NotifyStatChange OnNotifyStatChange;

    public void RegisterToNotifyStatChange(NotifyStatChange function)
    {
        OnNotifyStatChange += function;
    }

    public void SetPlayerName()
    {        
        playerInfo.ChangeName(inputField.text);
    }

    public void EndCreation()
    {
        CharStatsToValueDictionary finalStats = new CharStatsToValueDictionary();

        foreach(CharacterStat st in minimumStats.Keys)
        {
            finalStats[st] = StatValue(st);
        }

        playerInfo.SetSkills(Skills);
        playerInfo.Stats = finalStats;
        playerInfo.FinishCreation();
        
        scr = GameObject.Find("Fade Out Canvas").GetComponent<ScreenFadeInFadeOut>();
        scr.StartFadeOut();

        StartCoroutine(WaitForFadeOutEnd());
    }

    public int StatValue(CharacterStat stat)
    {
        return
            minimumStats[stat] +
            statModifiers[stat] +
            pointsSpent[stat];
    }

    public bool IncreaseStat(CharacterStat stat)
    {
        bool result = false;

        if (statPointsLeft > 0 && StatValue(stat) < MAX_STAT)
        {
            //stats[stat]++;
            pointsSpent[stat]++;
            TryDecreasingStatPoints(1);
            result = true;

            FireNotifyStatChange();
        }

        return result;
    }

    public bool DecreaseStat(CharacterStat stat)
    {
        bool result = false;

        if (StatValue(stat) > MIN_STAT && pointsSpent[stat] > 0)
        {
            pointsSpent[stat]--;
            IncreaseStatPoints(1);
            result = true;

            FireNotifyStatChange();
        }

        return result;
    }

    public bool ApplyBackgroundModificators(CharacterStat[] statsArray, int[] modifs)
    {
        bool result = ComponentReadyToWork();

        if (result)
        {
            ResetCreation();

            for (int i = 0; i < statsArray.Length; i++)
            {
                ApplyBackgroundModificator(statsArray[i], modifs[i]);
            }

            FireNotifyStatChange();
        }

        return result;
    }

    public bool TryDecreasingStatPoints(int value)
    {
        bool result = value > 0 && StatPointsLeft - value >= 0;

        if(result)
        {
            statPointsLeft -= value;
            FireNotifyStatChange();
        }

        return result;
    }

    public bool IncreaseStatPoints(int value)
    {
        bool result = value > 0 && statPointsLeft + value <= StartingStatPoints + BonusStatPoints;

        if (result)
        {
            statPointsLeft += value;
            FireNotifyStatChange();
        }

        return result;
    }

    // Use this for initialization
    void Awake ()
    {        
        playerInfo = new CharacterInfo();
        inputField = GameObject.Find("Name Input Field").GetComponent<InputField>();

        minimumStats = new CharStatsToValueDictionary(10);
        pointsSpent = new CharStatsToValueDictionary(0);
        statModifiers = new CharStatsToValueDictionary(0);

        statPointsLeft = StartingStatPoints;

    }    

    private bool ComponentReadyToWork()
    {
        return
            playerInfo != null &&
            inputField != null &&
            minimumStats != null &&
            pointsSpent != null &&
            statModifiers != null;
    }

    private IEnumerator WaitForFadeOutEnd()
    {
        while(scr.FadeInProgress)
        {
            yield return new WaitForEndOfFrame();
        }

        CharacterInfoScript temp = gameObject.AddComponent<CharacterInfoScript>();
        temp.SetPlayerInfo(PlayerInfo);

        SceneManager.LoadSceneAsync("Loading Screen");
    }    

    private void ResetCreation()
    {
        int spentCharPointsOnAttributes = 0;

        foreach(CharacterStat stat in minimumStats.Keys)
        {
            statModifiers[stat] = 0;
            spentCharPointsOnAttributes += pointsSpent[stat];
            pointsSpent[stat] = 0;
        }

        statPointsLeft = StartingStatPoints + BonusStatPoints ;

        if(Skills != null) Skills.Clear();
    }

    private void ApplyBackgroundModificator(CharacterStat stat, int modif)
    {        
        statModifiers[stat] = modif;        
    }
       
    private void FireNotifyStatChange()
    {
        if(OnNotifyStatChange != null)
        {
            OnNotifyStatChange();
        }
    }
}
