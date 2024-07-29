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

        // Spawn another cloud if it's time
        if (spawnTimer <= 0 && timeRemaining <= 0)
        {   
            // Choose which cloud to spawn
            int index = Random.Range(0, clouds.Length);
            GameObject cloud = clouds[index];

            // Choose a random y-value for the cloud
            float y = Random.Range(3.6f, 7.7f);

            // Always spawn the cloud off screen with a low x value
            Vector3 pos = new Vector3(-12, y, 0);

            // Quaternion for gimble lock prevention, spawn with Instantiate
            Instantiate(cloud, pos, Quaternion.identity);
            spawnTimer = timeBetweenSpawns;
            
        }
        else if (timeRemaining <= 0)
        {
            spawnTimer -= Time.deltaTime;

        }

    }

    /// <summary>
    /// Resets timeRemaining
    /// </summary>
    public void Reset()
    {
        timeRemaining = 2;

    }
}
