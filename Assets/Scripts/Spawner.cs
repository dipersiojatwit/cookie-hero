using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Analytics;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float timeBetweenSpawns;
    public static bool canSpawn;
    private float spawnTimer;
    private float timeRemaining = 2;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = timeBetweenSpawns;
        canSpawn = true;

    }

    // Update is called once per frame
    void Update()
    {   
        timeRemaining -= Time.deltaTime;

        if (spawnTimer <= 0 && timeRemaining <= 0 && canSpawn)
        {
            int index = Random.Range(0, enemies.Length);
            GameObject enemy = enemies[index];
            float x = Random.Range(-9.0f, 9.0f);
            Vector3 pos = new Vector3(x, 10, 0);
            // Quaternion for gimble lock prevention, spawn with Instantiate
            Instantiate(enemy, pos, Quaternion.identity);
            // reset timer
            spawnTimer = timeBetweenSpawns;
            
        }
        else if (timeRemaining <= 0)
        {
            spawnTimer -= Time.deltaTime;

        }
    }

    public void Reset()
    {
        timeRemaining = 2;

    }
    
}
