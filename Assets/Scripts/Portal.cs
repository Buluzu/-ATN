using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Portal : Unit
{
    // Start is called before the first frame update
    public static Portal instance;
    public Slider slider;
    public TextMeshProUGUI HPamount;
    public bool isDestroyed=false;  
    //Level check
    public int index;
    public string nameLevel;
    public int achieved;
    //[SerializeField] private TextMeshProUGUI level;
    //public Image levelSelection;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        HP += (index - 1) * 500;
        currentHP = HP;
        achieved = PlayerPrefs.GetInt(nameLevel);
        //level.text = "Level" + index;

    }

    private void Update()
    {
        stat();
        PortalHPBar();
        if (currentHP <= 0) isDestroyed = true;
        CheckPortal();
    }
    public void PortalHPBar()
    {
        slider.maxValue = HP;
        slider.value = currentHP;
        HPamount.text = currentHP.ToString();

    }

    void CheckPortal()
    {
        if (isDestroyed == true)
        {
            Time.timeScale = 0.5f;
            if (achieved == 0)
            {
                index += 1;
                achieved++;
                PlayerPrefs.SetInt("highestLevel", index);
                PlayerPrefs.SetInt(nameLevel, achieved);
                PlayerPrefs.Save();

                //SceneManager.LoadScene("LevelManager");
                //MenuScenceManager.instance.levelSelector.gameObject.SetActive(true);
            }

            if (achieved == 1)
            {
                //SceneManager.LoadScene("LevelManager");
                //MenuScenceManager.instance.levelSelector.gameObject.SetActive(true);

            }
        }
    }

}
