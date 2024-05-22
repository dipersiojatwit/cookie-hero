using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRound : MonoBehaviour
{   
    public GameObject[] cookies;
    public static bool isBonus;
    public static bool ateCookie;
    public static int bonusRoundCookies;
    private int bonusContinueThreshold;
    private float spawnTimerValue;
    private float spawnTimer;
    private int cookiesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        bonusRoundCookies = 0;
        spawnTimerValue = 2f;
        spawnTimer = spawnTimerValue;
        bonusContinueThreshold = 10;
    }

    // Update is called once per frame
    void Update()
    {   
        spawnTimer -= Time.deltaTime;

        // check if it's the bonus round
        if (isBonus)
        {
            if (spawnTimer <= 0)
            {
                GameObject cookie = cookies[Random.Range(0, cookies.Length)];
                float x = Random.Range(-9.0f, 9.0f);
                Vector3 pos = new Vector3(x, 10, 0);
                // Quaternion for gimble lock prevention, spawn with Instantiate
                Instantiate(cookie, pos, Quaternion.identity);
                cookiesSpawned++;
                spawnTimer = spawnTimerValue;
            }

        }

        // check if the bonus round should continue every ten cookie spawns
        if (cookiesSpawned % 10 == 0)
        {
            if (bonusRoundCookies >= 50)
            {
                PerfectRound();
            }
            else if (bonusRoundCookies >= bonusContinueThreshold)
            {
                spawnTimerValue -= 0.3f;
                bonusContinueThreshold += 10;
            }
            else
            {
                ResetBonusRound();
            }
        }
        
    }

    // call when every possible cookie is eaten 
    private void PerfectRound()
    {

    }

    private void ResetBonusRound()
    {
        isBonus = false;
        spawnTimer = 2f;
        bonusContinueThreshold = 10;
        cookiesSpawned = 0;

        // set the normal spawner 
        Spawner.canSpawn = true;
    }
}
