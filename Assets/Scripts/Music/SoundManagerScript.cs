using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    [SerializeField]
    private float FadeTime;
    
    [SerializeField]
    private float AmbienceVolume;

    [SerializeField]
    private float MusicVolume;

    [SerializeField]
    private float SoundVolume;

    [SerializeField]
    private float VoiceVolume;
    
    [SerializeField]
    private AudioClip DefaultMusic;

    [SerializeField]
    private AudioClip DefaultAmbience;

    [SerializeField]
    private AudioSource Music;
    [SerializeField]
    private AudioSource Ambience;
    [SerializeField]
    private AudioSource Sound;
    [SerializeField]
    private AudioSource Voice;    

    private Dictionary<AudioSource, bool> Fading;

    private bool AllowSaving;

    public void SetListenerVolume(float val)
    {
        AudioListener.volume = val;
    }

    public void SetMusicVolume(float val)
    {
        MusicVolume = val;
        Music.volume = MusicVolume;
        if (AllowSaving)
        {
            Debug.Log("Set music prefs");
            PlayerPrefs.SetFloat("Music Volume", val);
        }
    }

    public void SetAmbientVolume(float val)
    {
        AmbienceVolume = val;
        Ambience.volume = val;
        if (AllowSaving) PlayerPrefs.SetFloat("Ambience Volume", val);
    }

    public void SetVoiceVolume(float val)
    {
        VoiceVolume = val;
        Voice.volume = val;
        if (AllowSaving) PlayerPrefs.SetFloat("Voice Volume", val);
    }

    public void SetSoundVolume(float val)
    {
        SoundVolume = val;
        Sound.volume = val;
        if (AllowSaving) PlayerPrefs.SetFloat("SFX Volume", val);
    }

    public void PlayMusic(AudioClip music = null)
    {
        StartCoroutine(SwitchClipWithFading((music != null)? music : DefaultMusic, Music, MusicVolume));        
    }

    public void PlayAmbience(AudioClip ambience = null)
    {
        StartCoroutine(SwitchClipWithFading((ambience != null) ? ambience : DefaultAmbience, Ambience, AmbienceVolume));
    }

    public void PlaySound(AudioClip sound)
    {
        if(Sound.isPlaying)
        {
            Sound.Stop();
        }

        Sound.clip = sound;
        Sound.Play();
    }

	// Use this for initialization
	void Start ()
    {
        AllowSaving = false;

        Fading = new Dictionary<AudioSource, bool>();
        Fading.Add(Ambience, false);
        Fading.Add(Music, false);
        Fading.Add(Sound, false);
        Fading.Add(Voice, false);

        StartCoroutine(WaitForFirstSets());

        SetMusicVolume(PlayerPrefs.GetFloat("Music Volume"));
        SetAmbientVolume(PlayerPrefs.GetFloat("Ambience Volume"));
        SetVoiceVolume(PlayerPrefs.GetFloat("Voice Volume"));
        SetSoundVolume(PlayerPrefs.GetFloat("SFX Volume"));

        PlayMusic();
        PlayAmbience();
	}
	
	// Update is called once per frame
	void Update () {
		
	}    

    private IEnumerator FadeIn(AudioSource source, float maxVolume)
    {
        if (Fading.ContainsKey(source))
        {
            while (Fading[source]) yield return new WaitForSeconds(FadeTime / 5);

            Fading[source] = true;
            float fadePerSec = maxVolume / FadeTime;

            while (source.volume < maxVolume)
            {
                source.volume = Mathf.Clamp(source.volume + fadePerSec * Time.deltaTime, 0, maxVolume);
                yield return new WaitForEndOfFrame();
            }
            Fading[source] = false;
        }
    }

    private IEnumerator FadeOut(AudioSource source, float maxVolume)
    {
        if (Fading.ContainsKey(source))
        {
            while (Fading[source]) yield return new WaitForSeconds(FadeTime / 5);

            Fading[source] = true;
            float fadePerSec = maxVolume / FadeTime;

            while (source.volume > 0)
            {
                source.volume = Mathf.Clamp(source.volume - fadePerSec * Time.deltaTime, 0, maxVolume);
                yield return new WaitForEndOfFrame();
            }

            Fading[source] = false;
        }
    }

    private IEnumerator SwitchClipWithFading(AudioClip clip, AudioSource source, float maxVolume)
    {
        if(source != null && source.clip != null && source.isPlaying)
        {
            StartCoroutine(FadeOut(source, maxVolume));

            while (source.volume > 0) yield return new WaitForEndOfFrame();
        }

        if(source != null) source.Stop();
        source.volume = 0;
        source.clip = clip;
        source.Play();

        StartCoroutine(FadeIn(source, maxVolume));
        


    }

    private IEnumerator WaitForFirstSets()
    {
        yield return new WaitForEndOfFrame();

        AllowSaving = true;
    }

}
