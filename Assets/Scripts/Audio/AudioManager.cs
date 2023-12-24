using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sounds[] musics, sfxs;
    public AudioSource musicSource, sfxSource;
    public LayerMask unitLayer;
    private float currentMusicVolume;
    private float currentSFXVolume;
    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
        instance = this;    
    }

    private void Start()
    {
        MenuScenceManager.Instance.musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        MenuScenceManager.Instance.sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        PlayMusic("Theme");
    }

  
    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(musics, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Khong co nhac");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }    public void PlaySFX(string name)
    {
        Sounds s = Array.Find(sfxs, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Khong co nhac");
        }
        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
