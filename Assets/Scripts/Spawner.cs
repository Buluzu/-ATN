using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnUnit;
    [SerializeField] private float randomTime = 5f;
    [SerializeField] private float spawnCD = 6f;
    private float timeBTWSpawn;


    void Start()
    {
        

    }
    private void Update()
    {
        if (timeBTWSpawn <= 0)
        {
            StartCoroutine(GetRandomTime());
            
            timeBTWSpawn = spawnCD;
        }
        else timeBTWSpawn -= Time.deltaTime;

        if(timeBTWSpawn <= 0)
        {
            timeBTWSpawn=spawnCD;
            if (randomTime > 14) StartCoroutine(Spawn());
        }
        else timeBTWSpawn -= Time.deltaTime;
    }
    IEnumerator GetRandomTime()
    {
        yield return new WaitForSeconds(5f);
        randomTime = Random.Range(5, 20);
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(spawnUnit, transform.position, transform.rotation);
    }
    /* public IEnumerator spawn()
     {

         yield return new WaitForSeconds(0);
         Instantiate(spawnUnit, spawnPoint.position, transform.rotation);

     }*/
}
