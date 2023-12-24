using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Image lockLevel;
    public Image unlockLevel;

    [SerializeField] private int highestLevel;

    void Start()
    {
        highestLevel = PlayerPrefs.GetInt("highestLevel", 1);

        for(int i = 0; i < levelButtons.Length; i++) 
        {
            int levelNum = i + 1;
            //levelButtons[1].interactable = false;
            if(levelNum>highestLevel) 
            {
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().sprite = lockLevel.sprite;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                levelButtons[i].interactable = true;              
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = ""+levelNum;
                levelButtons[i].GetComponent<Image>().sprite = unlockLevel.sprite;
            }
        }
    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene("LV"+levelNum);
        //AudioManager.instance.PlayMusic("Theme" + levelNum);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("LevelManager");
    }
}
