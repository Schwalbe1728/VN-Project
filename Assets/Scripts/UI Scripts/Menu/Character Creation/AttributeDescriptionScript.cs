using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeDescriptionScript : MonoBehaviour
{
    private Text DescriptionText;
    private Coroutine TransitionCoroutine;

    [SerializeField]
    private float TransitionTime;

    #region Basic Attributes
    [Multiline]
    [SerializeField]
    private string Agility;

    [Multiline]
    [SerializeField]
    private string Perception;

    [Multiline]
    [SerializeField]
    private string Character;

    [Multiline]
    [SerializeField]
    private string Wits;

    [Multiline]
    [SerializeField]
    private string Physique;
    #endregion

    #region Additional Attributes
    [Multiline]
    [SerializeField]
    private string HealthPoints;

    [Multiline]
    [SerializeField]
    private string BaseDMG;

    [Multiline]
    [SerializeField]
    private string HealRate;

    [Multiline]
    [SerializeField]
    private string MaxStressPenalty;

    [Multiline]
    [SerializeField]
    private string MaxSanity;
    #endregion

    public void SetDescriptionText(string text, bool dump)
    {
        Description = text;
    }

    public void SetDescriptionText(string stat)
    {
        string result = null;

        try
        {
            CharacterStat statEnum = CharacterStat.Physique.FromString(stat);

            switch (statEnum)
            {
                case CharacterStat.Agility:
                    result = Agility;
                    break;

                case CharacterStat.Perception:
                    result = Perception;
                    break;

                case CharacterStat.Character:
                    result = Character;
                    break;

                case CharacterStat.Wits:
                    result = Wits;
                    break;

                case CharacterStat.Physique:
                    result = Physique;
                    break;
            }
        }
        catch (System.InvalidCastException)
        {
            stat = stat.ToLower();

            switch(stat)
            {
                case "hp":
                    result = HealthPoints;
                    break;

                case "bdmg":
                    result = BaseDMG;
                    break;

                case "hr":
                    result = HealRate;
                    break;

                case "maxsp":
                    result = MaxStressPenalty;
                    break;

                case "sanity":
                    result = MaxSanity;
                    break;
            }
        }

        Description = result;
    }

    void Awake()
    {
        DescriptionText = gameObject.GetComponent<Text>();
    }

    private string Description
    {
        get
        {
            return DescriptionText.text;
        }

        set
        {
            if (!value.Equals(DescriptionText.text))
            {
                if (TransitionCoroutine != null)
                {
                    StopCoroutine(TransitionCoroutine);
                }

                TransitionCoroutine = StartCoroutine(ChangeText(value));
            }
        }
    }

    private IEnumerator ChangeText(string value)
    {
        Color textColor = DescriptionText.color;

        while(DescriptionText.color.a > 0)
        {
            textColor.a -= Time.deltaTime / (TransitionTime / 2);
            DescriptionText.color = textColor;
            yield return new WaitForEndOfFrame();
        }

        DescriptionText.text = value;

        while(DescriptionText.color.a < 1)
        {
            textColor.a += Time.deltaTime / (TransitionTime / 2);
            DescriptionText.color = textColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
