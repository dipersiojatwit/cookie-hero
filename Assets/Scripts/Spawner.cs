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
    public static float spawnTimer;
    private float timeRemaining = 2.7f;
    

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
            GameObject enemy = enemies[Random.Range(0, enemies.Length)];
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

    /// <summary>
    /// Resets timeRemaining to its initial value
    /// </summary>
    public void Reset()
    {
        timeRemaining = 2.7f;

    }
    
}
