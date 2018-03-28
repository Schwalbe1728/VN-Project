using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManagerScript : MonoBehaviour
{
    //[SerializeField]
    //private Color LockedSkillColor;

    private Dictionary<CharacterSkills, bool> Skills;
    private Dictionary<CharacterSkills, Toggle> ToggleHandles = new Dictionary<CharacterSkills, Toggle>();

    [SerializeField]
    private CharacterCreationScript characterCreation;

    [SerializeField]
    private int SkillCost = 2;   

    public bool SetSkill(CharacterSkills skill, bool value, out bool pointRelatedIssue)
    {
        bool result =
            (value) ?
                characterCreation.TryDecreasingStatPoints(SkillCost, out pointRelatedIssue) :
                characterCreation.IncreaseStatPoints(SkillCost, out pointRelatedIssue);

        if (result)
        {
            if (!Skills.ContainsKey(skill))
            {
                Skills.Add(skill, value);
            }
            else
            {
                Skills[skill] = value;
            }
        }

        characterCreation.Skills = GetSkills();

        //pointRelatedIssue = !pointRelatedIssue;

        return result;
    }

    public bool ApplyBackgroundSkills(CharacterSkills[] skillsGranted)
    {
        foreach(CharacterSkills skill in ToggleHandles.Keys)
        {
            ToggleHandles[skill].interactable = true;
            ToggleHandles[skill].isOn = false;                                    
        }

        foreach (CharacterSkills skill in skillsGranted)
        {
            ToggleHandles[skill].interactable = false;
            ToggleHandles[skill].isOn = true;            
        }

        return true;
    }

    public List<CharacterSkills> GetSkills()
    {
        List<CharacterSkills> result = new List<CharacterSkills>();

        foreach(CharacterSkills skill in Skills.Keys)
        {
            if(Skills[skill])
            {
                result.Add(skill);
            }

        }

        return result;
    }

    public void RegisterHandle(CharacterSkills skill, Toggle handle)
    {
        ToggleHandles.Add(skill, handle);        
    }

    void Awake()
    {
        Skills = new Dictionary<CharacterSkills, bool>();
        //ToggleHandles = new Dictionary<CharacterSkills, Toggle>();
    }
}
