using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EditorConfigurationData
{
    public bool ConfigurationOpened;

    public bool DiagonalStartPoints = false;
    public bool DiagonalEndPoints = false;

    public int MaxQuotasLength = 49;
    public int MaxTextAreaHeight = 150;
    public int MinTextAreaHeight = 50;

    public Color ImmidiateNodeConnection;
    public Color NodeToOptionConnection;
    public Color OptionToNodeConnection;

    public Color ToConditionConnection;
    public Color FromSuccesConnection;
    public Color FromFailureConnection;
    public Color EntryConditionConnection;

    public Color AreaBackgroundColor;

    public Color ConditionNodeColor;
    public Color DialogueNodeColor;
    public Color DialogueOptionColor;

    public GUIStyle BoundingBoxStyle = null;
    public GUIStyle EditorAreaBackgroundStyle = null;
    public GUIStyle TextAreaStyle = null;
    public GUIStyle FoldoutInteriorStyle = null;
    public GUIStyle WrappedLabelStyle = null;

    public GUIStyle InteriorLighterBackgroundStyle = null;
    public GUIStyle InteriorDarkerBackgroundStyle = null;

    public GUIStyle ConditionNodeStyle = null;
    public GUIStyle DialogueNodeStyle = null;
    public GUIStyle DialogueOptionStyle = null;

    public Rect RectHandle;

    private EditorConfigurationData Defaults;
    //private bool Connections = false;
    //private bool Backgrounds = false;

    private string[] preferencesKeys =
        new string[]
        {
                "NODE EDITOR: immidiateConnection",
                "NODE EDITOR: nodeToOption",
                "NODE EDITOR: optionToNode",
                "NODE EDITOR: editorBackground",
                "NODE EDITOR: diagonalStart",
                "NODE EDITOR: diagonalEnd",
                "NODE EDITOR: conditionConnection",
                "NODE EDITOR: fromSuccess",
                "NODE EDITOR: fromFailure",
                "NODE EDITOR: entryCondition",
                "NODE EDITOR: conditionNodeColor",
                "NODE EDITOR: dialogueNodeColor",
                "NODE EDITOR: dialogueOptionColor"
        };

    public string[] PreferencesKeys { get { return preferencesKeys; } }

    public EditorConfigurationData()
    {
        Defaults = new EditorConfigurationData(false);

        if (!PreferencesExist())
        {
            RestoreDefaults();
        }
        else
        {
            RestorePreferences();
        }

        RectHandle = new Rect(20 * Vector2.one, new Vector2(250, 30));
    }

    public void RestoreDefaults()
    {
        if (Defaults != null)
        {
            ImmidiateNodeConnection = Defaults.ImmidiateNodeConnection;
            NodeToOptionConnection = Defaults.NodeToOptionConnection;
            OptionToNodeConnection = Defaults.OptionToNodeConnection;

            ToConditionConnection = Defaults.ToConditionConnection;
            FromSuccesConnection = Defaults.FromSuccesConnection;
            FromFailureConnection = Defaults.FromFailureConnection;
            EntryConditionConnection = Defaults.EntryConditionConnection;

            AreaBackgroundColor = Defaults.AreaBackgroundColor;

            ConditionNodeColor = Defaults.ConditionNodeColor;
            DialogueNodeColor = Defaults.DialogueNodeColor;
            DialogueOptionColor = Defaults.DialogueOptionColor;

            DiagonalEndPoints = false;
            DiagonalStartPoints = false;

            InitStyles(true);
        }
    }

    public bool PreferencesExist()
    {
        bool result = true;

        foreach (string key in preferencesKeys)
        {
            result &= EditorPrefs.HasKey(key);
        }

        return result;
    }

    public void RestorePreferences()
    {
        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[0]), out ImmidiateNodeConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[1]), out NodeToOptionConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[2]), out OptionToNodeConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[3]), out AreaBackgroundColor))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[6]), out ToConditionConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[7]), out FromSuccesConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[8]), out FromFailureConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[9]), out EntryConditionConnection))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[10]), out ConditionNodeColor))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[11]), out DialogueNodeColor))
        {
            Debug.Log("Wtf");
        }

        if (!TryParseFromString(EditorPrefs.GetString(preferencesKeys[12]), out DialogueOptionColor))
        {
            Debug.Log("Wtf");
        }

        DiagonalStartPoints = EditorPrefs.GetBool(preferencesKeys[4], false);
        DiagonalEndPoints = EditorPrefs.GetBool(preferencesKeys[5], false);

        InitStyles(true);
    }

    public static string ColorToString(Color color)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(color.r);
        sb.Append("#");
        sb.Append(color.g);
        sb.Append("#");
        sb.Append(color.b);
        sb.Append("#");
        sb.Append(color.a);

        return sb.ToString();
    }

    public static bool TryParseFromString(string colorText, out Color col)
    {
        string[] seperated = colorText.Split(new char[] { '#' }, System.StringSplitOptions.RemoveEmptyEntries);

        float red = -1;
        float green = -1;
        float blue = -1;
        float alpha = -1;

        bool success = true;

        if ((success = float.TryParse(seperated[0], out red)))
        {
            if (success && (success = float.TryParse(seperated[1], out green)))
            {
                if (success && (success = float.TryParse(seperated[2], out blue)))
                {
                    if (success && (success = float.TryParse(seperated[3], out alpha)))
                    {

                    }
                }
            }
        }

        col = (success) ? new Color(red, green, blue, alpha) : new Color();

        return success;
    }

    public void InitStyles(bool forced = false)
    {        
        if (forced || BoundingBoxStyle == null)
        {
            BoundingBoxStyle = new GUIStyle();
            BoundingBoxStyle.normal.background = MakeTex(1, 1, new Color(0, 0, 0, 0));
        }

        if (forced || EditorAreaBackgroundStyle == null)
        {
            EditorAreaBackgroundStyle = new GUIStyle();
            EditorAreaBackgroundStyle.normal.background = MakeTex(1, 1, AreaBackgroundColor);
        }

        if (forced || TextAreaStyle == null)
        {
            TextAreaStyle = new GUIStyle(GUI.skin.textArea);
            TextAreaStyle.wordWrap = true;
            //textAreaStyle.fixedWidth = 200;           
        }

        if (forced || FoldoutInteriorStyle == null)
        {
            FoldoutInteriorStyle = new GUIStyle();
            FoldoutInteriorStyle.margin = new RectOffset(20, 5, 0, 0);
            FoldoutInteriorStyle.clipping = TextClipping.Clip;
            FoldoutInteriorStyle.stretchWidth = false;
        }

        if(forced || InteriorLighterBackgroundStyle == null)
        {
            InteriorLighterBackgroundStyle = new GUIStyle();
            InteriorLighterBackgroundStyle.normal.background = MakeTex(1, 1, new Color(1, 1, 1, 0.25f));
            InteriorLighterBackgroundStyle.clipping = TextClipping.Clip;
            InteriorLighterBackgroundStyle.stretchWidth = false;
        }

        if (forced || InteriorDarkerBackgroundStyle == null)
        {
            InteriorDarkerBackgroundStyle = new GUIStyle();
            InteriorDarkerBackgroundStyle.normal.background = MakeTex(1, 1, new Color(0, 0, 0, 0.25f));
            InteriorDarkerBackgroundStyle.clipping = TextClipping.Clip;
            InteriorDarkerBackgroundStyle.stretchWidth = false;
        }

        if (forced || WrappedLabelStyle == null)
        {            
            WrappedLabelStyle = new GUIStyle(EditorStyles.label);
            WrappedLabelStyle.wordWrap = true;
            WrappedLabelStyle.clipping = TextClipping.Clip;
            WrappedLabelStyle.fontStyle = FontStyle.Italic;
            WrappedLabelStyle.stretchWidth = false;            
        }

        if(forced || ConditionNodeStyle == null)
        {
            ConditionNodeStyle = new GUIStyle(GUI.skin.window);
            ConditionNodeStyle.normal.background = MakeTex(ConditionNodeStyle.normal.background, ConditionNodeColor);
        }

        if (forced || DialogueNodeStyle == null)
        {
            DialogueNodeStyle = new GUIStyle(GUI.skin.window);
            DialogueNodeStyle.normal.background = MakeTex(DialogueNodeStyle.normal.background, DialogueNodeColor);
        }

        if (forced || DialogueOptionStyle == null)
        {
            DialogueOptionStyle = new GUIStyle(GUI.skin.window);
            DialogueOptionStyle.normal.background = MakeTex(DialogueOptionStyle.normal.background, DialogueOptionColor);
        }
    }

    private static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private static Texture2D MakeTex(Texture2D texture, Color col)
    {
        RenderTexture temporary =
            RenderTexture.GetTemporary(
                    texture.width,
                    texture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        // Blit the pixels on texture to the RenderTexture
        Graphics.Blit(texture, temporary);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = temporary;
        Texture2D result = new Texture2D(texture.width, texture.height);
        result.ReadPixels(new Rect(0, 0, temporary.width, temporary.height), 0, 0);
        result.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(temporary);

        Color[] basePixels = result.GetPixels();
        Color[] pix = new Color[texture.width * texture.height];

        for(int i = 0; i < pix.Length; ++i)
        {
            pix[i] = basePixels[i] * col;
        }
        
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private EditorConfigurationData(bool dummy)
    {
        ConfigurationOpened = dummy;

        ImmidiateNodeConnection = Color.red;
        NodeToOptionConnection = Color.black;
        OptionToNodeConnection = Color.blue;

        ToConditionConnection = Color.yellow;
        FromSuccesConnection = Color.yellow;
        FromFailureConnection = Color.yellow;
        EntryConditionConnection = Color.yellow;

        AreaBackgroundColor = new Color(0, 0.5f, 0, 0.4f);

        ConditionNodeColor = new Color(0.6f, 0.4f, 1f, 0.4f);
        DialogueNodeColor = new Color(1f, 0.6f, 0.4f, 0.4f);
        DialogueOptionColor = new Color(0.4f, 1f, 0.6f, 0.4f);
    }
}
