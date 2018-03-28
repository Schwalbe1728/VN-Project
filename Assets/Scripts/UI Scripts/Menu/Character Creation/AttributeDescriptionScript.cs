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
    private string MaxSanity;

    [Multiline]
    [SerializeField]
    private string WalkingSpeed;
    #endregion

    public void SetDescriptionText(string text, bool dump)
    {
        Description = text;
    }

    public void SetDescriptionText(string stat)
    {
        try
        {
            CharacterAttribute attr = CharacterStatExtension.FromString(stat);
            SetDescriptionText(attr);
        }
        catch(System.InvalidCastException e)
        {
            try
            {
                CharacterStatistic statistic = CharacterStatExtension.FromStringStatistic(stat);
                SetDescriptionText(statistic);
            }
            catch(System.InvalidCastException ex)
            {
                Debug.LogWarning("Nieprawidłowa nazwa cechy postaci: " + stat);
            }
        }
        
    }
    
    public void SetDescriptionText(CharacterStatistic stat)
    {
        string result = "";

        switch(stat)
        {
            case CharacterStatistic.BaseDamage:
                result = BaseDMG;
                break;

            case CharacterStatistic.HealRatesCombined:
                result = HealRate;
                break;

            case CharacterStatistic.Sanity:
                result = MaxSanity;
                break;

            case CharacterStatistic.Vitality:
                result = HealthPoints;
                break;

            case CharacterStatistic.WalkingSpeed:
                result = WalkingSpeed;
                break;
        }

        Description = result;
    }

    public void SetDescriptionText(CharacterAttribute stat)
    {
        string result = "";

        switch (stat)
        {
            case CharacterAttribute.Agility:
                result = Agility;
                break;

            case CharacterAttribute.Perception:
                result = Perception;
                break;

            case CharacterAttribute.Character:
                result = Character;
                break;

            case CharacterAttribute.Wits:
                result = Wits;
                break;

            case CharacterAttribute.Physique:
                result = Physique;
                break;
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
