using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioSystem : Singleton<AudioSystem>
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectPref = "SoundEffectPref";
    private int firstPlayInt;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _DefaultsoundsClip;

    public Slider BackgroundSlider, SoundEffectsSlider;
    public float BackgroundFloat, SoundEffectsFloat;

    public Sound[] sounds;

    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if (firstPlayInt == 0)
        {
            BackgroundFloat = .4f;
            SoundEffectsFloat = .5f;
            BackgroundSlider.value = BackgroundFloat;
            SoundEffectsSlider.value =SoundEffectsFloat;
            PlayerPrefs.SetFloat(BackgroundPref, BackgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectPref, SoundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            BackgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            BackgroundSlider.value = BackgroundFloat;
            SoundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectPref);
            SoundEffectsSlider.value = SoundEffectsFloat;
        } 
        PlayMusic(_DefaultsoundsClip);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source?.Play();
    }


    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }

    public void SaveSoundSettings()
    {   
        BackgroundFloat = BackgroundSlider.value;
        SoundEffectsFloat = SoundEffectsSlider.value;
        PlayerPrefs.SetFloat(BackgroundPref, BackgroundFloat);
        PlayerPrefs.SetFloat(SoundEffectPref, SoundEffectsFloat);
        PlayerPrefs.Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) 
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        _musicSource.volume = BackgroundSlider.value;

        foreach (Sound s in sounds)
        {
            s.source.volume = SoundEffectsSlider.value;
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
