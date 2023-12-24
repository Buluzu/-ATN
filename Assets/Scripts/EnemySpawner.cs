using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    private float currentCD;
    public Transform spawnPoint;
    public wave[] waves;
    private int currentWaveIndex = 0;
    //private bool readyToCD;

    private void Start()
    {
        currentCD = countdown;
        //readyToCD = true;
       /* for(int i = 0; i < waves.Length; i++) 
        {
            waves[i].
        }*/
    }
    void Update()
    {
        if(currentWaveIndex>=waves.Length)
        {
            return;
        }
        //if(readyToCD==true) countdown -= Time.time;

        currentCD-=Time.deltaTime;
        if (currentCD <= 0)
        {
            //readyToCD = false;
            //countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(spawnWave());
            currentCD = countdown;
        }

    }

    private IEnumerator spawnWave()
    {
        if(currentWaveIndex < waves.Length) 
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.position,transform.rotation  );
                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
        
    }
    
}
[System.Serializable]
public class wave
{
    public Unit[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;
}
