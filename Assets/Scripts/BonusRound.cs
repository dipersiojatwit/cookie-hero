using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class BonusRound : MonoBehaviour
{   
    public GameObject candyCookie;
    public GameObject bigCandyCookie;
    public GameObject hugeCandyCookie;
    public GameObject giantCandyCookie;
    public GameObject maxCandyCookie;
    private GameObject currentCandyCookie;
    public Image redWheel;
    public Image blueWheel;
    public Image greenWheel;
    public Image yellowWheel;
    public Image orangeWheel;
    private bool isSpawnPause;
    public static bool isBonus;
    public static bool isTick;
    public static int bonusRoundCookies;
    public static int cookiesInBatch;
    public static float initialWheelFill;
    public static bool isInitialWheelUpdate;
    private float xPos;
    private Vector3 posVec;
    private int bonusContinueThreshold;
    private float spawnTimerValue;
    private float spawnTimer;
    private int cookiesSpawned;
    private bool checkedStreak;
    private bool isEnd;
    private static Animator animator;

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        bonusRoundCookies = 0;
        cookiesInBatch = 0;
        spawnTimerValue = 1.7f;
        spawnTimer = spawnTimerValue;
        bonusContinueThreshold = 10;
        
    }

    // Update is called once per frame
    void Update()
    {   
        // check if it's the bonus round
        if (isBonus)
        {   
            if (cookiesInBatch == 10 && GetCurrentWheel().fillAmount == 1)
            {
                cookiesInBatch = 0;

            }

            if (isInitialWheelUpdate)
            {
                initialWheelFill = GetCurrentWheel().fillAmount;
                isInitialWheelUpdate = false;

            }

            if (isTick)
            {   
                UptickCookieWheel(GetWheelSpeed(), (10f - cookiesInBatch) * 0.1f, GetCurrentWheel());
                
            }
        
            if (isSpawnPause)
            {
                spawnTimer = 1.7f;
                isSpawnPause = false;

            }

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {   
                // Generate a random position for the current spawn
                xPos = Random.Range(-7.0f, 7.0f);
                posVec = new Vector3(xPos, 10, 0);
                currentCandyCookie = Instantiate(candyCookie, posVec, Quaternion.identity);
                cookiesSpawned++;

                
                checkedStreak = false;
                spawnTimer = spawnTimerValue;

                if (cookiesSpawned % 10 == 0 && cookiesSpawned != 0)
                {
                    isSpawnPause = true;

                }

            }

            // check if the bonus round should continue every ten cookie spawns
            if (cookiesSpawned % 10 == 0 && cookiesSpawned != 0 && currentCandyCookie == null && !checkedStreak && bonusRoundCookies < 50)
            {   
                checkedStreak = true;

                Debug.Log(currentCandyCookie);
                Debug.Log(bonusRoundCookies + "/" + bonusContinueThreshold);
                
                // bonus round will continue if the cookies eaten meets the required continue amount
                if (bonusRoundCookies >= bonusContinueThreshold)
                {   
                    Debug.Log("cotinue");
                    animator.SetBool("isSpeedUp", true);
                    bonusContinueThreshold += 10;

                    if (bonusRoundCookies < 30)
                    {
                        spawnTimerValue -= 0.4f;
                    }
                    else
                    {
                        spawnTimerValue -= 0.2f;

                    }
                    
                }
                else
                {   
                    Debug.Log("end");
                    EndBonusRound();

                }

            }

        }
        else if (isEnd && currentCandyCookie == null)
        {   
            Debug.Log("Reset");
            ResetBonusRound();

        }
        
    }

    /// <summary>
    /// Called through an animation event from the bonus round icon animation.
    /// Starts the bonus round and disables the normal spawner
    /// </summary>
    public void ActivateBonusRound()
    {
        bonusRoundCookies = 0;
        cookiesSpawned = 0;
        Spawner.canSpawn = false;
        isBonus = true;

    }

    /// <summary>
    /// Triggers the bonus round icon animation
    /// </summary>
    public static void ActivateBonusRoundAnimation()
    {
        // an animation event activates the bonus round spawn
        animator.SetBool("isBonusIcon", true);

    }

    /// <summary>
    /// Called when the bonus round ends.
    /// Resets bonus round variables and activates the normal spawner again.
    /// </summary>
    private void ResetBonusRound()
    {
        Debug.Log("is Reset");
        isBonus = false;
        isEnd = false;
        spawnTimer = 2f;
        bonusContinueThreshold = 10;
        cookiesSpawned = 0;
        // Have a pause between the end of the bonus round and restart of the normal spawner.
        Spawner.spawnTimer = 2;
        // Set the normal spawner
        Spawner.canSpawn = true;

    }

    private void EndBonusRound()
    {   
        Debug.Log("End bonus round");
        posVec.x = 0;
        posVec.y = 20;
        currentCandyCookie = Instantiate(GetBonusCookie(), posVec, Quaternion.identity);
        // Make sure Stinkus can't trigger another trash time for a while
        Stinkus.timeBetweenActions = 20;
        isEnd = true;
        isBonus = false;
        
    }

    private GameObject GetBonusCookie()
    {
        if (bonusRoundCookies >= 10 && bonusRoundCookies < 20)
        {
            return bigCandyCookie;
        }
        else if (bonusRoundCookies >= 20 && bonusRoundCookies < 30)
        {
            return hugeCandyCookie;
        }
        else if (bonusRoundCookies >= 30 && bonusRoundCookies < 50)
        {
            return giantCandyCookie;
        }
        else if (bonusRoundCookies >= 50)
        {
            return maxCandyCookie;
        }
        else
        {
            return candyCookie;
            
        }

    }
    
    /// <summary>
    /// Decreases the radius of the gray circle covering a colored circle.
    /// Used for bonus round UI when Cookie Hero gets a cookie
    /// </summary>
    /// <param name="speed">Float that determines how much the gray wheel's fill decreases each frame</param>
    /// <param name="healthBarDestination">Float that determines where the gray wheel's fill should be after the bar is done scrolling</param>
    /// <param name="wheel"></param>
    public void UptickCookieWheel(float speed, float healthBarDestination, Image wheel)
    {   
        if (initialWheelFill > healthBarDestination)
        {
            wheel.fillAmount -= speed; 
        }
        if (wheel.fillAmount <= healthBarDestination)
        {
            isTick = false;
        }
    }

    private float GetWheelSpeed()
    {
        if (cookiesSpawned <= 10)
        {   
            return 0.000699f;
        }
        else if (cookiesSpawned > 10 && cookiesSpawned <= 20)
        {   
            return 0.000999f;
        }
        else if (cookiesSpawned > 20 && cookiesSpawned <= 30)
        {
            return 0.002999f;
        }
        else
        {
            return 0.003999f;
        }
        
    }    

    /// <summary>
    /// Returns the correct UI wheel to fill
    /// </summary>
    /// <returns>An Image</returns>
    private Image GetCurrentWheel()
    {
        if (cookiesSpawned <= 10)
        {   
            return redWheel;
        }
        else if (cookiesSpawned > 10 && cookiesSpawned <= 20)
        {   
            return blueWheel;
        }
        else if (cookiesSpawned > 20 && cookiesSpawned <= 30)
        {
            return greenWheel;
        }
        else if (cookiesSpawned > 30 && cookiesSpawned <= 40)
        {
            return yellowWheel;
        }
        else
        {
            return orangeWheel;
        }
        
    }

    /// <summary>
    /// Sets the given animation to false
    /// </summary>
    /// <param name="animationName">String of the animation to set false</param>
    public void SetAnimationFalse(String animationName)
    {
        animator.SetBool(animationName, false);
        
    }
}
