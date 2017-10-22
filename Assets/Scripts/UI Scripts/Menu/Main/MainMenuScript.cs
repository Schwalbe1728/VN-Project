using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private ScreenFadeInFadeOut fadeScreen;

    public void NewGameButton()
    {
        fadeScreen.StartFadeOut();
        StartCoroutine(WaitForFadeToFinish());
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    private IEnumerator WaitForFadeToFinish()
    {
        while(fadeScreen.FadeInProgress)
        {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation op = SceneManager.LoadSceneAsync("CharacterCreationScreen");        
    }
}
