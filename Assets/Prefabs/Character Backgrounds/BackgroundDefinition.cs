﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Background", menuName = "VN-Project/Character Background")]
public class BackgroundDefinition : ScriptableObject
{
    [Header("Description")]
    [SerializeField]
    private new string name;

    [Multiline]
    [SerializeField]
    private string description;

    [Space(10)]
    [Header("Attributes Modifiers")]
    [SerializeField]
    private int Agility;
    [SerializeField]
    private int Perception;
    [SerializeField]
    private int Character;
    [SerializeField]
    private int Wits;
    [SerializeField]
    private int Physique;

    private CharacterAttribute[] modifiedStatDeclaration;
    private int[] modifiedStatValues;

    [Space(10)]
    [Header("Skills")]
    [SerializeField]
    private CharacterSkills[] skillsGranted;

    [Space(10)]
    [Header("Stat Points")]
    [SerializeField]
    private int bonusStatPoints = 0;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public int BonusStatPoints { get { return bonusStatPoints; } }

    public bool ApplyModificators(CharacterCreationScript player, SkillsManagerScript skillManager)
    {
        player.BonusStatPoints = BonusStatPoints;

        if (modifiedStatDeclaration == null || modifiedStatDeclaration.Length == 0)
        {
            modifiedStatDeclaration =
                new CharacterAttribute[]
                {
                    CharacterAttribute.Agility,
                    CharacterAttribute.Perception,
                    CharacterAttribute.Character,
                    CharacterAttribute.Wits,
                    CharacterAttribute.Physique
                };
        }

        if (modifiedStatValues == null || modifiedStatValues.Length == 0)
        {
            modifiedStatValues =
                new int[]
                {
                    Agility,
                    Perception,
                    Character,
                    Wits,
                    Physique
                };
        }

        return
            player.SetBackground(this) &&
            player.ApplyBackgroundModificators(modifiedStatDeclaration, modifiedStatValues) &&
            skillManager.ApplyBackgroundSkills(skillsGranted);

    }
}
