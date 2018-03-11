using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManagerScript : MonoBehaviour {

	void Awake()
    {
        StartCoroutine(WaitForInitializationOfDialogues());
    }

    private IEnumerator WaitForInitializationOfDialogues()
    {
        yield return new WaitForSeconds(1f);
        /*
        while(!DialoguesManagerScript.Ready)
        {
            yield return new WaitForEndOfFrame();
        }
        */
        SceneManager.LoadSceneAsync("Game Screen");
    }
}
