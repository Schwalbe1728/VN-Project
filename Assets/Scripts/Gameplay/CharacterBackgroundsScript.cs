using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBackgroundsScript : MonoBehaviour
{    
    private Text BackgroundTitle;
    private AttributeDescriptionScript ExplainText;

    [SerializeField]
    private BackgroundDefinition[] definitions;
    private BackgroundDefinition chosenBackground;
    private int choice;

    [SerializeField]
    private CharacterCreationScript player;

    public void NextBackground()
    {
        choice = (choice + 1) % definitions.Length;

        SetBackgroundAndText();

        chosenBackground.ApplyModificators(player);
    }

    public void PreviousBackground()
    {
        choice =
            (definitions.Length + choice - 1) % definitions.Length;

        SetBackgroundAndText();

        chosenBackground.ApplyModificators(player);
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
        SetBackgroundAndText();        
    }

    private void SetBackgroundAndText()
    {
        chosenBackground = definitions[choice];

        if(BackgroundTitle != null)
        {
            BackgroundTitle.text = chosenBackground.Name;
        }        
    }    
}

[System.Serializable]
public class BackgroundDefinition
{
    [SerializeField]
    private string name;

    [Multiline]
    [SerializeField]
    private string description;

    [SerializeField]
    private CharacterStat[] modifiedStatDeclaration;

    [SerializeField]
    private int[] modifiedStatValues;

    [SerializeField]
    private CharacterSkills[] skillsGranted;

    public string Name { get { return name; } }
    public string Description { get { return description; } }

    public void ApplyModificators(CharacterCreationScript player)
    {
        player.ApplyBackgroundModificators(modifiedStatDeclaration, modifiedStatValues);
    }
}

public enum CharacterSkills
{

}
