using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCanvasSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject Map;

    [SerializeField]
    private GameObject CharacterSheet;

    [SerializeField]
    private GameObject[] Defaults;

    private bool MapOpened;
    private bool CharacterSheetOpened;

    void Awake()
    {
        OpenDefaults();
    }

    void Update()
    {
        if(VNInputManager.Instance.GetKeyDown("Toggle Town Map"))
        {
            if(MapOpened)
            {
                OpenDefaults();
            }
            else
            {
                OpenMap();
            }
        }

        if (VNInputManager.Instance.GetKeyDown("Toggle Character Sheet"))
        {
            if (CharacterSheetOpened)
            {
                OpenDefaults();
            }
            else
            {
                OpenCharacterSheet();
            }
        }
    }

    private void OpenMap()
    {
        MapOpened = true;
        CharacterSheetOpened = false;

        Map.SetActive(true);
        CharacterSheet.SetActive(false);

        for (int i = 0; i < Defaults.Length; i++)
        {
            Defaults[i].SetActive(false);
        }
    }

    private void OpenCharacterSheet()
    {
        MapOpened = false;
        CharacterSheetOpened = true;

        Map.SetActive(false);
        CharacterSheet.SetActive(true);

        for (int i = 0; i < Defaults.Length; i++)
        {
            Defaults[i].SetActive(false);
        }
    }

    private void OpenDefaults()
    {
        MapOpened = false;
        CharacterSheetOpened = false;

        Map.SetActive(false);
        CharacterSheet.SetActive(false);

        for(int i = 0; i < Defaults.Length; i++)
        {
            Defaults[i].SetActive(true);
        }
    }
}
