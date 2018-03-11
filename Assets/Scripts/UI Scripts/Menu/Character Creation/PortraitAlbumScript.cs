using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitAlbumScript : MonoBehaviour
{
    [SerializeField]
    private PlayerPortraitScript[] PortraitArray;

    private int PresentedIndex;
    private Image portraitDisplay;

    public void NextPortrait()
    {
        PresentedIndex = (PresentedIndex + 1) % PortraitArray.Length;
        PresentPortrait();
    }

    public void PreviousPortrait()
    {
        PresentedIndex = ((PresentedIndex - 1) % PortraitArray.Length + PortraitArray.Length) % PortraitArray.Length;
        PresentPortrait();
    }

    public Sprite SelectedPortrait
    {
        get
        {
            return portraitDisplay.sprite;
        }
    }

    // Use this for initialization
    void Awake()
    {
        PresentedIndex = 0;
        portraitDisplay = gameObject.GetComponent<Image>();

        PresentPortrait();
    }

    private void PresentPortrait()
    {
        if(PortraitArray != null && PortraitArray.Length > 0)
        {
            portraitDisplay.sprite = PortraitArray[PresentedIndex].Portrait;
        }
    }
}
