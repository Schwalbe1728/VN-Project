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
    private CharacterAttribute AttrDefinition;

    [SerializeField]
    private CharacterStatistic StatDefinition;

    [SerializeField]
    private bool IsAttribute;

    public void UpdatedStat()
    {
        if (gameObject.activeInHierarchy)
        {
            StringBuilder sb = new StringBuilder();
            
            if(IsAttribute)
            {
                //sb.Append(characterScript.StatValue(AttrDefinition));
                StartCoroutine(PostponeAppend(sb, AttrDefinition));
            }
            else
            {
                StartCoroutine(PostponeAppend(sb, StatDefinition));
            }
            
            /*
            try
            {
                CharacterAttribute statistic = CharacterStatExtension.FromString(StatDefinition);
                sb.Append(characterScript.StatValue(statistic));
            }
            catch (System.InvalidCastException)
            {
                //StartCoroutine(PostponeAppend(sb, StatDefinition));
                CharacterStatistic statistic = CharacterStatExtension.FromString(StatDefinition);
                sb.Append(characterScript.StatValue(statistic));
            }
            catch (System.NullReferenceException)
            {
                CharacterAttribute statistic = CharacterStatExtension.FromString(StatDefinition);
                StartCoroutine(PostponeAppend(sb, statistic));

                return;
            }
            */
            StatText.text = sb.ToString();
        }
    }
	
    void OnEnable()
    {
        UpdatedStat();
    }

    void Awake()
    {
        StatText = gameObject.GetComponent<Text>();
        characterScript = GameObject.Find("Game Info Component").GetComponent<CharacterCreationScript>();

        characterScript.RegisterToNotifyStatChange(UpdatedStat);

        UpdatedStat();        
    }

    private IEnumerator PostponeAppend(StringBuilder sb, CharacterAttribute statistic)
    {
        while( characterScript.PlayerInfo == null || characterScript.PlayerInfo.Stats == null)
        {
            yield return new WaitForEndOfFrame();
        }

        sb.Append(characterScript.StatValue(statistic));
        StatText.text = sb.ToString();
    }

    private IEnumerator PostponeAppend(StringBuilder sb, CharacterStatistic statistic)
    {        
        while (characterScript.PlayerInfo == null || characterScript.PlayerInfo.Stats == null)
        {
            yield return new WaitForEndOfFrame();
        }        

        CharacterInfo dummy = new CharacterInfo();
        CharStatsToValueDictionary temp = new CharStatsToValueDictionary();

        foreach(CharacterAttribute st in temp.Keys)
        {
            temp[st] = characterScript.StatValue(st);          
        }

        dummy.Stats = temp;

        //sb.Append(StatsRuleSet.StringToStatValueString(statistic, dummy));
        sb.Append(StatsRuleSet.GetStatValueString(dummy, statistic));
        StatText.text = sb.ToString();
    }
}
