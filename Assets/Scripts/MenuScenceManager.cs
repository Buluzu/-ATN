using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScenceManager : Singleton<MenuScenceManager>
{
    // Start is called before the first frame update
    public Image settingBG;
    public Slider musicSlider;
    public Slider sfxSlider;

   
    public void StartGame()
    {

        AudioManager.instance.PlaySFX("Click");

        SceneManager.LoadScene("LevelManager");
    }
    
    public void ExitGame()
    {
        AudioManager.instance.PlaySFX("Click");
        Application.Quit();

    }

    public void Setting()
    {
        AudioManager.instance.PlaySFX("Click");
        settingBG.gameObject.SetActive(true);
    }

    public void CloseSetting()
    {
        AudioManager.instance.PlaySFX("Click");
        settingBG.gameObject.SetActive(false);
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void MuteMusic()
    {
        AudioManager.instance.MuteMusic();
    }
}
