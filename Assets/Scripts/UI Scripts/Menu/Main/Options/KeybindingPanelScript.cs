using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindingPanelScript : MonoBehaviour
{
    [SerializeField]
    private string KeyName;

    [SerializeField]
    private Text TitleText;

    [SerializeField]
    private Button KeyBindingButton;
    private Text KeyBindingText;

    private KeyCode keyCode;

    private bool Set = false;
    private Coroutine Cor = null;
    private bool Edit = false;

    public void StartSetKey()
    {
        Edit = true;
    }

    public void StopSetKey()
    {
        Edit = false;
        KeyBindingButton.OnDeselect(null);
    }

    /*
    public IEnumerator SetKeyCoroutine()
    {
        Event e;

        while((e = Event.current) == null)
        {
            Debug.Log("Wait");
            yield return new WaitForEndOfFrame();
        }

        KeyCode code;        

        Debug.Log(e.isKey);
        /*
        if(System.Enum.TryParse<KeyCode>(Input.inputString, out code))
        {
            VNInputManager.Instance.SetKeybinding(KeyName, code);
            Set = false;
            Cor = null;
        }
                

        KeyBindingButton.OnDeselect(null);
    }
    */

    void Awake()
    {        
        KeyBindingText = KeyBindingButton.GetComponent<Text>();        
    }

    void Update()
    {
        if (!Set && Cor == null)
        {
            Cor = StartCoroutine(WaitForInputCreation());
        }
    }   

	void OnValidate()
    {
        TitleText.text = KeyName;
    }
    
    void OnGUI()
    {
        if (Edit)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                if (!VNInputManager.Instance.BindingExists(e.keyCode))
                {
                    VNInputManager.Instance.SetKeybinding(KeyName, e.keyCode);
                    Set = false;
                    Cor = null;                    
                }

                StopSetKey();
            }
        }
    }    
    
    private IEnumerator WaitForInputCreation()
    {
        yield return VNInputManager.WaitForInstantiating();        

        keyCode = VNInputManager.Instance.GetKeyBinding(KeyName);
        KeyBindingText.text = keyCode.ToString();        

        Set = true;
        Cor = null;
    }    
}
