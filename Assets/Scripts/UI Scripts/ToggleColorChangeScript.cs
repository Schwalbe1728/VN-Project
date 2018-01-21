using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColorChangeScript : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private Color BaseColor = Color.white;

    [SerializeField]
    private float UncheckedAlpha;

    [SerializeField]
    private float CheckedAlpha;

    public void SelectionChanged(bool state)
    {
        text.color =
            BaseColor * ((state)? CheckedAlpha : UncheckedAlpha);
    }
}
