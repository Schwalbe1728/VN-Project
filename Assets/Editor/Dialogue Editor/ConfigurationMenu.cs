﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConfigurationWindow : EditorWindow
{
    EditorConfigurationData ConfigData;
    System.Action RepaintFunction;

    bool Connections = false;
    bool Backgrounds = false;
    bool Display = false;

    int DescriptionLabelWidth = 175;

    public void Init(EditorConfigurationData data, System.Action repaintFunction)
    {
        ConfigData = data;
        RepaintFunction = repaintFunction;
    }

    public static void ShowConfigMenu(EditorConfigurationData data, System.Action repaintFunction)
    {
        ConfigurationWindow config = EditorWindow.GetWindow<ConfigurationWindow>();
        config.Init(data, repaintFunction);
    }

    void OnGUI()
    {
        if (ConfigData != null)
        {
            DrawMenu();
        }
    }

    private void DrawMenu()
    {
        bool repaint = false;

        Display = EditorGUILayout.Foldout(Display, "Display: ");

        if (Display)
        {
            GUILayout.BeginVertical(ConfigData.FoldoutInteriorStyle);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Max Quothas Length: ", GUILayout.Width(DescriptionLabelWidth));
                    int check = EditorGUILayout.IntField(ConfigData.MaxQuotasLength, GUILayout.Width(50));
                    if (check > 16)
                    {
                        ConfigData.MaxQuotasLength = check;
                        repaint = true;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Min Text Area Height: ", GUILayout.Width(DescriptionLabelWidth));
                    int check = EditorGUILayout.IntField(ConfigData.MinTextAreaHeight, GUILayout.Width(50));

                    if (check > ConfigData.MaxTextAreaHeight)
                    {
                        check = ConfigData.MaxTextAreaHeight;
                    }

                    if (check > 10)
                    {
                        ConfigData.MinTextAreaHeight = check;
                        repaint = true;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Max Text Area Height: ", GUILayout.Width(DescriptionLabelWidth));
                    int check = EditorGUILayout.IntField(ConfigData.MaxTextAreaHeight, GUILayout.Width(50));
                    if (check > 50 && check > ConfigData.MinTextAreaHeight)
                    {
                        ConfigData.MaxTextAreaHeight = check;
                        repaint = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        Connections = EditorGUILayout.Foldout(Connections, "Connections: ");

        if (Connections)
        {
            GUILayout.BeginVertical(ConfigData.FoldoutInteriorStyle);
            {
                DrawColorChanger("Immidiate Node Connection: ", ref ConfigData.ImmidiateNodeConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[0], EditorConfigurationData.ColorToString(ConfigData.ImmidiateNodeConnection));

                DrawColorChanger("Node To Option Connection: ", ref ConfigData.NodeToOptionConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[1], EditorConfigurationData.ColorToString(ConfigData.NodeToOptionConnection));

                DrawColorChanger("Option To Node Connection: ", ref ConfigData.OptionToNodeConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[2], EditorConfigurationData.ColorToString(ConfigData.OptionToNodeConnection));

                DrawColorChanger("To Condition Connection: ", ref ConfigData.ToConditionConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[6], EditorConfigurationData.ColorToString(ConfigData.ToConditionConnection));

                DrawColorChanger("Condition Success Connection: ", ref ConfigData.FromSuccesConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[7], EditorConfigurationData.ColorToString(ConfigData.FromSuccesConnection));

                DrawColorChanger("Condition Failure Connection: ", ref ConfigData.FromFailureConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[8], EditorConfigurationData.ColorToString(ConfigData.OptionToNodeConnection));

                DrawColorChanger("Entry Condition Connection: ", ref ConfigData.EntryConditionConnection);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[9], EditorConfigurationData.ColorToString(ConfigData.EntryConditionConnection));

                GUILayout.BeginHorizontal();
                GUILayout.Label("Diagonal Start Points: ", GUILayout.Width(DescriptionLabelWidth));
                ConfigData.DiagonalStartPoints = EditorGUILayout.Toggle(ConfigData.DiagonalStartPoints);
                EditorPrefs.SetBool(ConfigData.PreferencesKeys[4], ConfigData.DiagonalStartPoints);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Diagonal End Points: ", GUILayout.Width(DescriptionLabelWidth));
                ConfigData.DiagonalEndPoints = EditorGUILayout.Toggle(ConfigData.DiagonalEndPoints);
                EditorPrefs.SetBool(ConfigData.PreferencesKeys[5], ConfigData.DiagonalEndPoints);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            repaint = true;
        }

        Backgrounds = EditorGUILayout.Foldout(Backgrounds, "Backgrounds: ");

        if (Backgrounds)
        {
            bool changedStyles = false;

            GUILayout.BeginVertical(ConfigData.FoldoutInteriorStyle);
            {
                changedStyles |= DrawColorChanger("Editor Background: ", ref ConfigData.AreaBackgroundColor);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[3], EditorConfigurationData.ColorToString(ConfigData.AreaBackgroundColor));

                changedStyles |= DrawColorChanger("Condition Node Background", ref ConfigData.ConditionNodeColor);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[10], EditorConfigurationData.ColorToString(ConfigData.ConditionNodeColor));

                changedStyles |= DrawColorChanger("Dialogue Node Background", ref ConfigData.DialogueNodeColor);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[11], EditorConfigurationData.ColorToString(ConfigData.DialogueNodeColor));

                changedStyles |= DrawColorChanger("Dialogue Option Background", ref ConfigData.DialogueOptionColor);
                EditorPrefs.SetString(ConfigData.PreferencesKeys[12], EditorConfigurationData.ColorToString(ConfigData.DialogueOptionColor));

                
            }
            GUILayout.EndVertical();            

            if(changedStyles)
            {
                ConfigData.InitStyles(true);
                repaint = true;
            }
        }

        GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
        {
            if (GUILayout.Button("Close"))
            {
                ConfigData.ConfigurationOpened = false;
                this.Close();
            }

            if (GUILayout.Button("Restore"))
            {
                ConfigData.RestoreDefaults();
                repaint = true;
            }
        }
        GUILayout.EndHorizontal();

        if (repaint)
        {
            RepaintFunction();
        }
    }

    private bool DrawColorChanger(string labelText, ref Color color)
    {
        Color temp = color;
        bool repaint = false;

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(labelText, GUILayout.Width(DescriptionLabelWidth));
            temp = EditorGUILayout.ColorField(temp, GUILayout.Width(50));
        }
        GUILayout.EndHorizontal();

        repaint = !color.Equals(temp);
        color = temp;

        return repaint;
    }
}
