using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject[] clouds;
    private float timeBetweenSpawns;
    private float spawnTimer;
    private float timeRemaining = 2;

    // Start is called before the first frame update
    void Start()
    {   
        timeBetweenSpawns = 0.5f;
        spawnTimer = timeBetweenSpawns;

    }

    // Update is called once per frame
    void Update()
    {   
        timeBetweenSpawns = Random.Range(5, 10);
        timeRemaining -= Time.deltaTime;

        if (spawnTimer <= 0 && timeRemaining <= 0)
        {
            int index = Random.Range(0, clouds.Length);
            GameObject cloud = clouds[index];
            float y = Random.Range(3.6f, 7.7f);
            Vector3 pos = new Vector3(-12, y, 0);
            // Quaternion for gimble lock prevention, spawn with Instantiate
            Instantiate(cloud, pos, Quaternion.identity);
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
