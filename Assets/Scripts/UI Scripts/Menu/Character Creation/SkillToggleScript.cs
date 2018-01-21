using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SkillToggleScript : MonoBehaviour
{
    [SerializeField]
    private CharacterSkills skill;

    private SkillsManagerScript skillManager;

    void Awake()
    {
        skillManager = gameObject.GetComponentInParent<SkillsManagerScript>();        

        Toggle temp = gameObject.GetComponent<Toggle>();
        temp.GetComponentInChildren<Text>().text = skill.ToString(false);
        skillManager.RegisterHandle(skill, temp);
        temp.onValueChanged.AddListener(OnChanged);
    }

    private void OnChanged(bool value)
    {
        if(!skillManager.SetSkill(skill, value))
        {
            gameObject.GetComponent<Toggle>().isOn = false;
        }
    }
}
