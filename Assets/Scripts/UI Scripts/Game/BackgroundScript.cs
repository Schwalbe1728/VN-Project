using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour {

    [SerializeField]
    private float FadeTime;

    [SerializeField]
    private Sprite DefaultBackground;

    private Image Background;
    private Image NPCPortrait;

    private Dictionary<Image, bool> Fading;

    public void ShowSurroundings()
    {
        StartCoroutine(FadeIn(Background, 2));
        StartCoroutine(FadeIn(NPCPortrait, 2));
    }

    public void HideSurroundings()
    {
        StartCoroutine(FadeOut(Background));
        StartCoroutine(FadeOut(NPCPortrait));
    }

    public void NextBackground(Sprite next)
    {
        StartCoroutine(ChangeSprite(next, Background));
    }

    public void NextNPCPortrait(Sprite next)
    {
        StartCoroutine(ChangeSprite(next, NPCPortrait));
    }

    public void RestoreDefaults()
    {
        NextBackground(DefaultBackground);
        NextNPCPortrait(null);
    }

	// Use this for initialization
	void Start ()
    {
        Background = GameObject.Find("Background").GetComponent<Image>();
        NPCPortrait = GameObject.Find("NPC").GetComponent<Image>();

        Fading = new Dictionary<Image, bool>();
        Fading.Add(Background, false);
        Fading.Add(NPCPortrait, false);

        Background.sprite = DefaultBackground;

        ShowSurroundings();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ChangeSprite(null , Background));
        }
	}

    private IEnumerator FadeIn(Image image, float multiplier = 1)
    {
        if (Fading.ContainsKey(image) && image.sprite != null)
        {
            while (Fading[image]) yield return new WaitForSeconds(FadeTime / 5);

            Fading[image] = true;

            while (image.color.a < 1f)
            {
                image.color = new Color(1, 1, 1, Mathf.Min(image.color.a + Time.deltaTime / (FadeTime * multiplier), 1));
                yield return new WaitForEndOfFrame();
            }

            Fading[image] = false;
        }
    }

    private IEnumerator FadeOut(Image image, float multiplier = 1)
    {
        if (Fading.ContainsKey(image))
        {
            while (Fading[image]) yield return new WaitForSeconds(FadeTime/5);

            Fading[image] = true;

            while (image.color.a > 0f)
            {
                image.color = new Color(1, 1, 1, Mathf.Max(image.color.a - Time.deltaTime / (FadeTime * multiplier), 0));
                yield return new WaitForEndOfFrame();
            }

            Fading[image] = false;
        }       
    }

    private IEnumerator ChangeSprite(Sprite sprite, Image image)
    {
        StartCoroutine(FadeOut(image));

        while (Fading[image]) yield return new WaitForSeconds(FadeTime / 5);

        image.sprite = sprite;

        StartCoroutine(FadeIn(image));
    }
}
