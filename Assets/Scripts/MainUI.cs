using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainUI : MonoBehaviour
{
    public static MainUI instance;
    public TextMeshProUGUI goldAmount;
    public Image a;
    public float gold;
    public GameObject archerPrefab;
    public GameObject paladinPrefab;
    public Transform spawnPoint;
    public float summonArcherCD;
    public float summonPaladinCd;
    [SerializeField] float ArcherCD;
    [SerializeField] float PaladinCD;
    [SerializeField] private bool runningArcherCD = false;
    [SerializeField] private bool runningPaladinCD = false;
    public Image ArcherButtonBar;
    public Image PaladinButtonBar;
    public float archerCost;
    public float paladinCost;

    public float killed;
    public TextMeshProUGUI killedAmount;

    public LayerMask allyLayer;
    public Image openShop;

    //Thong báo
    public Image notificationImage;
    public Text notificationText;

    public Image RequestImage;
    public Text RequestText;

    public Image victoryBG;

    //Audio
    public Slider musicSlider;
    public Slider sfxSlider;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gold += (15 * Portal.instance.index);
        Time.timeScale = 0;
        Request(Portal.instance.index);
        if (Portal.instance.index == 1) Notification("Tiêu diệt những kẻ lang thang để kiếm thêm tiền, xây dựng quân đội, cố thủ trước các đợt tấn công của kẻ địch");
    }
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (runningArcherCD == true)
        {
            ArcherCD -= Time.deltaTime;
        }
        if (ArcherCD <= 0) runningArcherCD = false;
        if (runningPaladinCD == true)
        {
            PaladinCD -= Time.deltaTime;
        }
        if (PaladinCD <= 0) runningPaladinCD = false;

        if (KnightController.instance.currentHP <= 0) Time.timeScale = 0.5f;

        LevelRequestManager();
        if (Portal.instance.isDestroyed == true)
        {
            AudioManager.instance.musicSource.Stop();
            AudioManager.instance.PlayMusic("Victory");
            victoryBG.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

    }
    IEnumerator LoadVictoryBG()
    {

        yield return new WaitForSeconds(2);


    }
    public void Pause()
    {
        AudioManager.instance.PlaySFX("Click");
        Time.timeScale = 0.0f;
        a.gameObject.SetActive(true);

    }

    public void Continue()
    {
        AudioManager.instance.PlaySFX("Click");
        Time.timeScale = 1;
        a.gameObject.SetActive(false);
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }
    public void UpdateUI()
    {
        goldAmount.text = gold.ToString();
        ArcherButtonBar.fillAmount = (int)ArcherCD / summonArcherCD;
        PaladinButtonBar.fillAmount = (int)PaladinCD / summonArcherCD;
        killedAmount.text = killed.ToString();

    }

    public void SpawnArcher()
    {
        AudioManager.instance.PlaySFX("Click");

        if (gold >= archerCost)
        {
            if (ArcherCD <= 0)
            {
                gold -= archerCost;
                Instantiate(archerPrefab, spawnPoint.position, transform.rotation);
                ArcherCD = summonArcherCD;
                runningArcherCD = true;
            }
        }
        else Notification("Bạn không đủ tiền");
    }


    public void SpawnPaladin()
    {
        AudioManager.instance.PlaySFX("Click");

        if (gold >= paladinCost)
        {
            if (PaladinCD <= 0)
            {
                gold -= paladinCost;
                Instantiate(paladinPrefab, spawnPoint.position, transform.rotation);
                PaladinCD = summonPaladinCd;
                runningPaladinCD = true;
            }
        }
        else Notification("Bạn không đủ tiền");

    }

    public void Attack()
    {
        AudioManager.instance.PlaySFX("Click");

        Collider2D[] ally = Physics2D.OverlapCircleAll(spawnPoint.position, 1000f, allyLayer);
        for (int i = 0; i < ally.Length; i++)
        {
            ally[i].GetComponent<Unit>().isDefense = false;
        }
    }

    public void Defense()
    {
        AudioManager.instance.PlaySFX("Click");

        Collider2D[] ally = Physics2D.OverlapCircleAll(spawnPoint.position, 1000f, allyLayer);
        for (int i = 0; i < ally.Length; i++)
        {
            ally[i].GetComponent<Unit>().isDefense = true;
        }

    }
    public void OpenShop()
    {
        AudioManager.instance.PlaySFX("Click");

        Time.timeScale = 0.0f;
        openShop.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        AudioManager.instance.PlaySFX("Click");

        Time.timeScale = 1.0f;
        openShop.gameObject.SetActive(false);
    }

    void Notification(string message)
    {
        notificationImage.gameObject.SetActive(true);
        notificationText.text = message;
        StartCoroutine(HideNotification());
    }

    void Request(float index)
    {
        RequestImage.gameObject.SetActive(true);
        if (index == 1) RequestText.text = "Cố thủ và tiêu diệt 20 kẻ địch";
        if (index == 2) RequestText.text = "Cố thủ và tiêu diệt 60 kẻ địch";
        if (index == 3) RequestText.text = "Bảo vệ căn cứ và phá hủy cổng dịch chuyển của kẻ địch";
        if (index == 4) RequestText.text = "Tiêu diệt 150 kể địch hoặc phá hủy cổng dịch chuyển của kẻ địch";
        if (index == 5) RequestText.text = "Tiêu diệt 200 kẻ địch hoặc phá hủy cổng dịch chuyển của kẻ địch";
        if (index == 6) RequestText.text = "Tiêu diệt 300 kẻ địch hoặc phá hủy cổng dịch chuyển của kẻ địch";
        if (index == 7) RequestText.text = "Bảo vệ căn cứ và phá hủy cổng dịch chuyển của kẻ địch";
        if (index == 8) RequestText.text = "Bảo vệ căn cứ và phá hủy cổng dịch chuyển của kẻ địch";
    }

    public void CloseRequest()
    {
        AudioManager.instance.PlaySFX("Click");

        Time.timeScale = 1.0f;
        RequestImage.gameObject.SetActive(false);
    }

    public void OpenRequest()
    {
        AudioManager.instance.PlaySFX("Click");

        Time.timeScale = 0;
        RequestImage.gameObject.SetActive(true);
    }

    public void LevelRequestManager()
    {
        if (Portal.instance.index == 1)
        {
            if (killed >= 20) Portal.instance.isDestroyed = true;
        }

        if (Portal.instance.index == 2)
        {
            if (killed >= 60) Portal.instance.isDestroyed = true;
        }
        if (Portal.instance.index == 4)
        {
            if (killed >= 150) Portal.instance.isDestroyed = true;
        }
        if (Portal.instance.index == 5)
        {
            if (killed >= 200) Portal.instance.isDestroyed = true;
        }
        if (Portal.instance.index == 6)
        {
            if (killed >= 300) Portal.instance.isDestroyed = true;
        }
    }

    public void BuffHP()
    {
        AudioManager.instance.PlaySFX("Click");

        if (gold >= 500)
        {
            gold -= 500;
            Collider2D[] ally = Physics2D.OverlapCircleAll(transform.position, 20f, allyLayer);
            for (int i = 0; i < ally.Length; i++)
            {
                ally[i].GetComponent<Unit>().HP *= 1.5f;
                ally[i].GetComponent<Unit>().currentHP *= 1.5f;
            }
            //buffHP;
        }
        else Notification("Bạn không đủ tiền");
    }
    public void BuffSpeed()
    {
        AudioManager.instance.PlaySFX("Click");

        if (gold >= 500)
        {
            gold -= 500;
            Collider2D[] ally = Physics2D.OverlapCircleAll(transform.position, 20f, allyLayer);
            for (int i = 0; i < ally.Length; i++)
            {
                ally[i].GetComponent<Unit>().speed *= 1.5f;
            }
            //buffHP;
        }
        else Notification("Bạn không đủ tiền");
    }
    public void BuffDamage()
    {
        AudioManager.instance.PlaySFX("Click");

        if (gold >= 500)
        {
            gold -= 500;
            Collider2D[] ally = Physics2D.OverlapCircleAll(transform.position, 20f, allyLayer);
            for (int i = 0; i < ally.Length; i++)
            {
                ally[i].GetComponent<Unit>().damage *= 1.5f;
            }
            //buffHP;
        }
        else Notification("Bạn không đủ tiền");
    }

    public void Restart()
    {
        AudioManager.instance.PlaySFX("Click");

        SceneManager.LoadScene("LV" + Portal.instance.index);
    }

    public void NextLevel()
    {
        AudioManager.instance.PlaySFX("Click");

        float a = Portal.instance.index + 1;
        SceneManager.LoadScene("LV" + a);
    }

    public void LoadLevelScene()
    {
        AudioManager.instance.PlaySFX("Click");

        SceneManager.LoadScene("LevelManager");
    }
    public void BackToMenu()
    {
        AudioManager.instance.PlaySFX("Click");

        SceneManager.LoadScene("Menu");
    }
    IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f);
        notificationImage.gameObject.SetActive(false);
        notificationText.text = "";
    }

}
