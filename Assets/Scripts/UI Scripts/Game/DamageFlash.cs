using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScreenFadeInFadeOut))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField]
    private Color FlashColor;

    [SerializeField]
    private float FlashDuration;

    private ScreenFadeInFadeOut FadeInScript;

    void Awake()
    {
        FadeInScript = gameObject.GetComponent<ScreenFadeInFadeOut>();
        GameObject.Find("Game Info Component").
            GetComponent<CharacterInfoScript>().
            RegisterToPlayerHurt(PlayDamageFlash);
    }

    private void PlayDamageFlash()
    {
        float rand = UnityEngine.Random.Range(0.9f, 1.1f);

        Color flash =
            new Color(
                FlashColor.r * rand,
                FlashColor.g * rand,
                FlashColor.b * rand,
                FlashColor.a * rand
            );

        FadeInScript.StartFlash(flash, FlashDuration * UnityEngine.Random.Range(0.9f, 1.1f));
    }
}
