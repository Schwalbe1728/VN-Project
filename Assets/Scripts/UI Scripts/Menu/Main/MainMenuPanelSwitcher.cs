using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelSwitcher : MonoBehaviour {

    [SerializeField]
    private GameObject loadGamePanel;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private GameObject creditsPanel;

    private GameObject currentPanel;

    public void OpenLoadGamePanel()
    {
        OpenPanel(loadGamePanel);
    }

    public void OpenOptionsPanel()
    {
        OpenPanel(optionsPanel);
    }

    public void OpenCreditsPanel()
    {
        OpenPanel(creditsPanel);
        creditsPanel.GetComponentInChildren<AutoScrollScript>().enabled = true;
    }

    public void CloseCurrentPanel()
    {
        if(PanelOpened)
        {
            if(currentPanel == creditsPanel)
            {
                creditsPanel.GetComponentInChildren<AutoScrollScript>().enabled = false;
            }

            currentPanel.SetActive(false);
        }
    }    

    void Awake()
    {

    }

    private void OpenPanel(GameObject panel)
    {
        CloseCurrentPanel();

        currentPanel = panel;
        currentPanel.SetActive(true);
    }

    private bool PanelOpened
    {
        get
        {
            return currentPanel != null && currentPanel.activeSelf;
        }
    }
}
