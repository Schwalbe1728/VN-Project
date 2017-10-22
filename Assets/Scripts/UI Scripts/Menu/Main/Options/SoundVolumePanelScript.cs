using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumePanelScript : MonoBehaviour {

    [SerializeField]
    private string Title;

    [SerializeField]
    private float Value;

    [SerializeField]
    private Text TitleText;

    [SerializeField]
    private Slider ValueSlider;

    [SerializeField]
    private Text ValueText;    

	// Use this for initialization
	void Awake ()
    {
        Value = PlayerPrefs.GetFloat(Title + " Volume", Value);
        ValueSlider.onValueChanged.AddListener(OnValueChanged);

        OnValidate();        
	}
	
    void OnValidate()
    {
        TitleText.text = Title;
        ValueSlider.value = Value;
        OnValueChanged(ValueSlider.value);
    }

    private void OnValueChanged(float val)
    {
        ValueText.text = (100 * val).ToString("N1");
    }
}
