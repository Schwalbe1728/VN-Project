using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatValueScript : MonoBehaviour
{
    private Text StatText;
    private CharacterCreationScript characterScript;   

    [SerializeField]
    private string StatDefinition;

    public void UpdatedStat()
    {        
        StringBuilder sb = new StringBuilder();        

        try
        {
            CharacterStat statistic = CharacterStatExtension.FromString(StatDefinition);
            sb.Append(characterScript.StatValue(statistic));
        }
        catch(System.InvalidCastException)
        {
            StartCoroutine(PostponeAppend(sb, StatDefinition));
        }
        catch(System.NullReferenceException)
        {
            CharacterStat statistic = CharacterStatExtension.FromString(StatDefinition);
            StartCoroutine(PostponeAppend(sb, statistic));

            return;
        }

        StatText.text = sb.ToString();
    }
	
    void Awake()
    {
        StatText = gameObject.GetComponent<Text>();
        characterScript = GameObject.Find("Game Info Component").GetComponent<CharacterCreationScript>();

        characterScript.RegisterToNotifyStatChange(UpdatedStat);

        UpdatedStat();        
    }

    private IEnumerator PostponeAppend(StringBuilder sb, CharacterStat statistic)
    {
        while( characterScript.PlayerInfo == null || characterScript.PlayerInfo.Stats == null)
        {
            yield return new WaitForEndOfFrame();
        }

        sb.Append(characterScript.StatValue(statistic));
        StatText.text = sb.ToString();
    }

    private IEnumerator PostponeAppend(StringBuilder sb, string statistic)
    {        
        while (characterScript.PlayerInfo == null || characterScript.PlayerInfo.Stats == null)
        {
            yield return new WaitForEndOfFrame();
        }        

        CharacterInfo dummy = new CharacterInfo();
        CharStatsToValueDictionary temp = new CharStatsToValueDictionary();

        foreach(CharacterStat st in temp.Keys)
        {
            temp[st] = characterScript.StatValue(st);          
        }

        dummy.Stats = temp;

        sb.Append(StatsRuleSet.StringToStatValueString(statistic, dummy));
        StatText.text = sb.ToString();
    }
}
