using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAffectedByFontSizeOption : MonoBehaviour
{
    private Text TextToScale;    
	
    void Awake()
    {
        TextToScale = gameObject.GetComponent<Text>();
        GameplayOptions gOptions = GameObject.Find("Settings").GetComponent<GameplayOptions>();

        gOptions.OnFontSizeChanged += OnFontChanged;

        OnFontChanged(gOptions.FontSize);
    }

    private void OnFontChanged(int size)
    {
        TextToScale.fontSize = size;
    }
}
