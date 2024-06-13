using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusRound : MonoBehaviour
{   
    public GameObject candyCookie;
    public static bool isBonus;
    public static bool ateCookie;
    public static int bonusRoundCookies;
    private float xPos;
    private Vector3 posVec;
    private int bonusContinueThreshold;
    private float spawnTimerValue;
    private float spawnTimer;
    private int cookiesSpawned;
    private static Animator animator;
    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        bonusRoundCookies = 0;
        spawnTimerValue = 1.7f;
        spawnTimer = spawnTimerValue;
        bonusContinueThreshold = 10;

        // these are used for choosing where to spawn cookies
        xPos = Random.Range(-9.0f, 9.0f);
        posVec = new Vector3(xPos, 10, 0);

    }

    // Update is called once per frame
    void Update()
    {   
        // check if it's the bonus round
        if (isBonus)
        {
            spawnTimer -= Time.deltaTime;
            Debug.Log(Spawner.canSpawn);
            Debug.Log(isBonus);
            if (spawnTimer <= 0)
            {   
                // Generate a random position for the current spawn
                xPos = Random.Range(-9.0f, 9.0f);
                posVec = new Vector3(xPos, 10, 0);
                Instantiate(candyCookie, posVec, Quaternion.identity);
                
                spawnTimer = spawnTimerValue;
            }

            // check if the bonus round should continue every ten cookie spawns
            if (cookiesSpawned % 10 == 0 && cookiesSpawned != 0)
            {   
                Debug.Log("Check continue, cookies spawned: " + cookiesSpawned);
                if (bonusRoundCookies >= 50)
                {
                    PerfectRound();
                }
                // bonus round will continue if the cookies eaten meets the required continue ammount
                else if (bonusRoundCookies >= bonusContinueThreshold)
                {   
                    Debug.Log("Continue");
                    spawnTimerValue -= 0.3f;
                    bonusContinueThreshold += 10;
                }
                else
                {
                    ResetBonusRound();
                }
            }

        }
        
    }

    // called during an animation event from ActivateBonusRoundAnimation()
    public void ActivateBonusRound()
    {
        bonusRoundCookies = 0;
        cookiesSpawned = 0;
        Spawner.canSpawn = false;
        isBonus = true;

    }

    public static void ActivateBonusRoundAnimation()
    {
        // an animation event activates the bonus round spawn
        animator.SetBool("isBonusIcon", true);

    }

    // call when every possible cookie is eaten 
    private void PerfectRound()
    {

    }

    private void ResetBonusRound()
    {
        Debug.Log("Reset");
        isBonus = false;
        spawnTimer = 2f;
        bonusContinueThreshold = 10;
        cookiesSpawned = 0;

        // set the normal spawner 
        Spawner.canSpawn = true;
    }

    public void SetAnimationFalse(String animationName)
    {
        animator.SetBool(animationName, false);
        
    }
}
