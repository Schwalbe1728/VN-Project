using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayOptions : MonoBehaviour
{
    public delegate void FontSizeChanged(int fontSizeInPoints);
    public event FontSizeChanged OnFontSizeChanged;

    private EInfoVerbosity chosenInfoVerbosity;
    private EFontSize chosenFontSize;
    private Dictionary<EFontSize, int> FontSizeLabelToValue;
    private Dictionary<int, EFontSize> ValueToFontSizeLabel;

    [SerializeField]
    private EInfoVerbosity[] VerbosityValuesArray;

    [SerializeField]
    private int[] FontSizeArray;  

    public int FontSize
    {
        get { return FontSizeLabelToValue[chosenFontSize]; }
    }

    public void SetFontSize(EFontSize size)
    {
        chosenFontSize = size;
        FireOnFontChangedEvent();
    }

    public void SetFontSize(int arrayIndex)
    {
        SetFontSize(ValueToFontSizeLabel[FontSizeArray[arrayIndex]]);
    }

    public EInfoVerbosity InfoVerbosity { get { return chosenInfoVerbosity; } }

    public void SetInfoVerbosity(int arrayIndex)
    {
        chosenInfoVerbosity = VerbosityValuesArray[arrayIndex];
    }

    public bool InfoVerbosityNotEnough(EInfoVerbosity candidate)
    {
        Dictionary<EInfoVerbosity, int> temp = new Dictionary<EInfoVerbosity, int>();

        temp.Add(EInfoVerbosity.Minimum, 0);
        temp.Add(EInfoVerbosity.Normal, 2);
        temp.Add(EInfoVerbosity.Verbose, 5);

        return
            temp[candidate] > temp[chosenInfoVerbosity];
    }

    void Awake()
    {
        FontSizeLabelToValue = new Dictionary<EFontSize, int>();
        FontSizeLabelToValue.Add(EFontSize.VerySmall, FontSizeArray[0]);
        FontSizeLabelToValue.Add(EFontSize.Small, FontSizeArray[1]);
        FontSizeLabelToValue.Add(EFontSize.Medium, FontSizeArray[2]);
        FontSizeLabelToValue.Add(EFontSize.Big, FontSizeArray[3]);
        FontSizeLabelToValue.Add(EFontSize.VeryBig, FontSizeArray[4]);

        ValueToFontSizeLabel = new Dictionary<int, EFontSize>();
        ValueToFontSizeLabel.Add(FontSizeArray[0], EFontSize.VerySmall);
        ValueToFontSizeLabel.Add(FontSizeArray[1], EFontSize.Small);
        ValueToFontSizeLabel.Add(FontSizeArray[2], EFontSize.Medium);
        ValueToFontSizeLabel.Add(FontSizeArray[3], EFontSize.Big);
        ValueToFontSizeLabel.Add(FontSizeArray[4], EFontSize.VeryBig);

        SetFontSize(EFontSize.Medium);
    }

    private void FireOnFontChangedEvent()
    {
        if(OnFontSizeChanged != null)
        {
            OnFontSizeChanged(FontSize);
        }
    }
}

public enum EFontSize
{
    VerySmall,
    Small,
    Medium,
    Big,
    VeryBig
}

public enum EInfoVerbosity
{
    Minimum,
    Normal,
    Verbose
}
