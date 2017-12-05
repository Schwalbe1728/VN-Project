using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VNInputManager : MonoBehaviour
{
    public static VNInputManager Instance { get; private set; }

    [SerializeField]
    private KeyBinding[] bindings;

    private Dictionary<string, KeyCode> bindingsDictionary;
    private Dictionary<string, bool> buttonPressed;
    private Dictionary<string, bool> buttonDown;

    public static bool Instantiated { get { return Instance != null; } }

    public bool GetKeyDown(string keyName)
    {
        return buttonDown.ContainsKey(keyName) && buttonDown[keyName];
    }

    public bool GetKeyPressed(string keyName)
    {
        return buttonPressed.ContainsKey(keyName) && buttonPressed[keyName];
    }

    public void SetKeybinding(string bindingName, KeyCode key)
    {
        bindingsDictionary[bindingName] = key;
        PlayerPrefs.SetString(bindingName, key.ToString());
    }

    public KeyCode GetKeyBinding(string keyName)
    {
        return bindingsDictionary[keyName];
    }

    public bool BindingExists(KeyCode key)
    {
        return bindingsDictionary.ContainsValue(key);
    }

    public static IEnumerator WaitForInstantiating()
    {
        while (! VNInputManager.Instantiated)
        {
            yield return null;
        }
    }

    void Awake()
    {       
        bindingsDictionary = new Dictionary<string, KeyCode>();
        buttonPressed = new Dictionary<string, bool>();
        buttonDown = new Dictionary<string, bool>();

        foreach(KeyBinding bd in bindings)
        {
            string key = PlayerPrefs.GetString(bd.Name);

            if (key != null && !key.Equals(""))
            {
                //Debug.Log(key);
                bindingsDictionary.Add
                    (bd.Name, 
                    (KeyCode)System.Enum.Parse(typeof(KeyCode), key))
                    ;                
            }
            else
            {
                bindingsDictionary.Add(bd.Name, bd.Key);
            }
            
            buttonPressed.Add(bd.Name, false);
            buttonDown.Add(bd.Name, false);
        }

        Instance = this;

        Debug.Log("Singleton created");
    }

    void Update()
    {
        foreach(string bind in bindingsDictionary.Keys)
        {
            buttonPressed[bind] = Input.GetKey(bindingsDictionary[bind]);
            buttonDown[bind] = Input.GetKeyDown(bindingsDictionary[bind]);

            //Debug.Log("Button " + bind.Name + ((buttonPressed[bind.Name]) ? " pressed" : " not pressed"));
            //Debug.Log("Button " + bind.Name + ((buttonDown[bind.Name]) ? " down" : " not down"));
        }       
    }
}

[Serializable]
public class KeyBinding
{
    [SerializeField]
    private string keyName;

    [SerializeField]
    private KeyCode keyCode;    

    public KeyCode Key { get { return keyCode; } }
    public string Name { get { return keyName; } }
}


