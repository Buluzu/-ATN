using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner1 : MonoBehaviour
{
    /*// Start is called before the first frame update
    public GameObject enemyPrefab; // 
    public float spawnInterval = 2f; // 
    public int maxEnemies = 10; // 

    [SerializeField]private int currentEnemies = 0; // 
    [SerializeField]private bool spawning = false; // 

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (currentEnemies < maxEnemies)
        {
            if (spawning)
            {
                yield return null;
            }
            else
            {
                spawning = true;

                
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                
                currentEnemies++;

                newEnemy.GetComponent<Unit>().OnDestroy += DecreaseEnemyCount;

                spawning = false;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DecreaseEnemyCount()
    {
        currentEnemies--;
    }*/

    public GameObject enemyPrefab; // Prefab của quái vật
    public float waveDelay = 5f; // Thời gian giữa các đợt sinh quái
    public int[] enemiesPerWave; // Mảng chứa số lượng quái vật trong mỗi đợt

    [SerializeField] private int currentWave = 0; // Đợt hiện tại
    public float unitDelay = 1;

    public Image notificationImage;
    public Text notificationText;


    private void Start()
    {
        StartCoroutine(SpawnWaves());

        // Sau 3 giây, thông báo sẽ tự động ẩn đi
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < enemiesPerWave.Length)
        {
            yield return new WaitForSeconds(waveDelay);
            SpawnWave(enemiesPerWave[currentWave]);
            currentWave++;
        }
    }

    private void SpawnWave(int enemyCount)
    {
        StartCoroutine(SpawnEnemies(enemyCount));
        notificationImage.gameObject.SetActive(true);
        notificationText.text = "Một nhóm kẻ địch đang đến, hay cẩn thận!";
        StartCoroutine(HideNotification());
    }

    private IEnumerator SpawnEnemies(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(unitDelay);
        }
    }
    IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f);
        notificationImage.gameObject.SetActive(false);
        notificationText.text = "";
    }
}

