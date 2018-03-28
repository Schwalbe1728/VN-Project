using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SkillToggleScript : MonoBehaviour
{
    [SerializeField]
    private CharacterSkills skill;

    private Toggle toggle;
    private SkillsManagerScript skillManager;

    private bool previousTagWasInvalid;

    void Awake()
    {
        previousTagWasInvalid = false;

        skillManager = gameObject.GetComponentInParent<SkillsManagerScript>();        

        toggle = gameObject.GetComponent<Toggle>();
        toggle.GetComponentInChildren<Text>().text = skill.ToString(false);
        skillManager.RegisterHandle(skill, toggle);
        toggle.onValueChanged.AddListener(OnChanged);        
    }

    private void OnChanged(bool value)
    {
        if (!skillManager.SetSkill(skill, value, out previousTagWasInvalid))
        {
            toggle.onValueChanged.RemoveListener(OnChanged);
            toggle.isOn = false;            
            toggle.onValueChanged.AddListener(OnChanged);            
        }            
    }
}
