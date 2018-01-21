using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBackgroundsScript : MonoBehaviour
{    
    private Text BackgroundTitle;
    private AttributeDescriptionScript ExplainText;

    [Header("Background Definitions")]
    [SerializeField]
    private BackgroundDefinition[] definitions;
    private BackgroundDefinition chosenBackground;
    private int choice;

    [Space(10)]
    [Header("On Scene Objects")]
    [SerializeField]
    private CharacterCreationScript player;

    [SerializeField]
    private SkillsManagerScript skillsManager;

    public void NextBackground()
    {
        choice = (choice + 1) % definitions.Length;

        SetBackgroundAndText();        
    }

    public void PreviousBackground()
    {
        choice =
            (definitions.Length + choice - 1) % definitions.Length;

        SetBackgroundAndText();        
    }

    public void SetDescriptionText()
    {
        ExplainText.SetDescriptionText(chosenBackground.Description, true);
    }

    void Awake()
    {
        choice = 0;
        BackgroundTitle = gameObject.GetComponent<Text>();
        ExplainText = GameObject.Find("Explain Text").GetComponent<AttributeDescriptionScript>();                
    }

    void Start()
    {
        SetBackgroundAndText();
    }

    private void SetBackgroundAndText()
    {
        chosenBackground = definitions[choice];

        if(BackgroundTitle != null)
        {
            BackgroundTitle.text = chosenBackground.Name;
        }

        StartCoroutine(PostponeApplying());
    }
    
    private IEnumerator PostponeApplying()
    {
        while(!chosenBackground.ApplyModificators(player, skillsManager))
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("Wait");
        }
    }       
}

public enum CharacterSkills
{
    Lockpicks,
    Pickpocketing,
    Melee
}

public static class CharacterSkillsExtension
{
    public static string ToString(this CharacterSkills skill, bool value)
    {
        switch(skill)
        {
            case CharacterSkills.Lockpicks:
                return "Lockpicks";

            case CharacterSkills.Pickpocketing:
                return "Pickpocketing";

            case CharacterSkills.Melee:
                return "Melee";
        }

        throw new System.ArgumentOutOfRangeException();
    }
}
