using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCodedAttributes : MonoBehaviour {

    [SerializeField]
    private Color Agility;

    [SerializeField]
    private Color Perception;

    [SerializeField]
    private Color Character;

    [SerializeField]
    private Color Wits;

    [SerializeField]
    private Color Physique;    

    public Color GetColor(CharacterAttribute stat)
    {
        switch(stat)
        {
            case CharacterAttribute.Agility:
                return Agility;

            case CharacterAttribute.Character:
                return Character;

            case CharacterAttribute.Perception:
                return Perception;

            case CharacterAttribute.Physique:
                return Physique;

            case CharacterAttribute.Wits:
                return Wits;
        }

        throw new System.InvalidCastException();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
