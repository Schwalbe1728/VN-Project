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

    public Color GetColor(CharacterStat stat)
    {
        switch(stat)
        {
            case CharacterStat.Agility:
                return Agility;

            case CharacterStat.Character:
                return Character;

            case CharacterStat.Perception:
                return Perception;

            case CharacterStat.Physique:
                return Physique;

            case CharacterStat.Wits:
                return Wits;
        }

        throw new System.InvalidCastException();
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
