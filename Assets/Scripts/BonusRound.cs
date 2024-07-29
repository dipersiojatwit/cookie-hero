using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEditor.Rendering;
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
    public Image greyWheel5;
    public Image redWheel;
    public Image greyWheel4;
    public Image blueWheel;
    public Image greyWheel3;
    public Image greenWheel;
    public Image greyWheel2;
    public Image yellowWheel;
    public Image greyWheel1;
    public Image orangeWheel;
    private bool isSpawnPause;
    public static bool isBonus;
    public static bool isTick;
    public static bool isEnd;
    public static int bonusRoundCookies;
    public static int cookiesInBatch;
    public static float initialWheelFill;
    public static bool isInitialWheelUpdate;
    private float xPos;
    private int bonusContinueThreshold;
    private float spawnTimerValue;
    private float spawnTimer;
    private int cookiesSpawned;
    private bool checkedStreak;
    public static bool isStart;
    private bool isReset;
    private Vector3 posVec;
    private static Animator animator;

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        bonusRoundCookies = 0;
        cookiesInBatch = 0;
        xPos = 0;
        spawnTimerValue = 1.7f;
        spawnTimer = spawnTimerValue;
        bonusContinueThreshold = 10;
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Check if it's the bonus round
        if (isBonus)
        {   
            if (isStart)
            {
                isStart = false;

            }
            
            // CookiesInBatch keeps track of the amount of cookies out of ten that needs to be obtained to continue the bonus round
            if (cookiesInBatch == 10 && GetCurrentWheel().fillAmount == 1)
            {
                cookiesInBatch = 0;

            }

            if (isInitialWheelUpdate)
            {
                initialWheelFill = GetCurrentWheel().fillAmount;
                isInitialWheelUpdate = false;

            }

            // Check if the wheel UI should be moving
            if (isTick)
            {   
                UptickCookieWheel(GetWheelSpeed(), (10f - cookiesInBatch) * 0.1f, GetCurrentWheel());
                
            }

            // Check if the next cookie spawn should be delayed 
            // This is done between batches of 10 for the UI display
            if (isSpawnPause)
            {
                spawnTimer = 1.7f;
                isSpawnPause = false;

            }

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {   
                // Generate a random x position for the current spawn if it's not the first
                if (bonusRoundCookies != 0)
                {
                    xPos = Random.Range(-7.0f, 7.0f);

                }

                posVec = new Vector3(xPos, 10, 0);
                currentCandyCookie = Instantiate(candyCookie, posVec, Quaternion.identity);
                cookiesSpawned++;

                // checkedStreak is set to false for each new cookie spawn
                // This prevents the continue check from running after the next new cookie already spawned
                checkedStreak = false;
                spawnTimer = spawnTimerValue;

                // Spawn pauses when it's time to check if the streak should continue
                if (cookiesSpawned % 10 == 0 && cookiesSpawned != 0)
                {
                    isSpawnPause = true;

                }

            }

            // Check if the bonus round should continue every ten cookie spawns
            // Only check if it hasn't been checked already and the current cookie is destroyed
            if (cookiesSpawned % 10 == 0 && cookiesSpawned != 0 && currentCandyCookie == null && !checkedStreak)
            {   
                checkedStreak = true;
                
                // Bonus round will continue if the cookies eaten meets the required continue amount
                if ((bonusRoundCookies >= bonusContinueThreshold) && bonusRoundCookies < 50)
                {   
                    animator.SetBool("isSpeedUp", true);
                    Debug.Log(bonusRoundCookies + " / " + bonusContinueThreshold);
                    bonusContinueThreshold += 10;

                    if (bonusRoundCookies < 30)
                    {
                        spawnTimerValue -= 0.4f;
                    }
                    else
                    {
                        // Spawn cookies faster the higher the streak 
                        spawnTimerValue -= 0.2f;

                    }
                    
                }
                else
                {   
                    Debug.Log("End. Cookies: " + bonusRoundCookies + " Bonus round threshold: " + bonusContinueThreshold);
                    EndBonusRound();

                }

            }

        }

        // isReset is set true in EndBonusRound()
        // This ensures the bonus round is not reset until the final cookie is destroyed
        else if (isReset && currentCandyCookie == null)
        {   
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
        MoveWheelUI();
        FadedCookie.isFadeIn = true;
        isBonus = true;

    }

    /// <summary>
    /// Called when the bonus round ends.
    /// Resets bonus round variables and activates the normal spawner again.
    /// </summary>
    private void ResetBonusRound()
    {
        ResetWheelUI();
        animator.SetBool("isEndIcon", true);
        isBonus = false;
        isEnd = false;
        isReset = false;
        spawnTimer = 2f;
        spawnTimerValue = 1.7f;
        bonusContinueThreshold = 10;
        bonusRoundCookies = 0;
        cookiesInBatch = 0;
        cookiesSpawned = 0;
        xPos = 0;
        FadedCookie.isFadeOut = true;

        // Have a pause between the end of the bonus round and restart of the normal spawner
        Spawner.spawnTimer = 2;
        
        // Set the normal spawner
        Spawner.canSpawn = true;

    }

    /// <summary>
    /// Called when the bonus round comes to the ending stage
    /// A giant candy cookie will be spawned
    /// </summary>
    private void EndBonusRound()
    {   
        isEnd = true;

        // spawn the bonus cookie in the middle of the screen far up
        posVec.x = 0;
        posVec.y = 20;
        currentCandyCookie = Instantiate(GetBonusCookie(), posVec, Quaternion.identity);

        // Make sure Stinkus can't trigger another trash time for a while
        Stinkus.timeBetweenActions = 20;
        isReset = true;
        isBonus = false;
        
    }

    /// <summary>
    /// Returns a bonus candy cookie based on how many candy cookies were eaten during the bonus round
    /// </summary>
    /// <returns>A reference to a GameObject that is some size of candy cookie</returns>
    private GameObject GetBonusCookie()
    {   
        // Determine which candy cookie to spawn based on the bonus round cookie score
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
    /// Moves the cookie wheel UI for the bonus round
    /// </summary>
    private void MoveWheelUI()
    {   
        redWheel.rectTransform.localPosition = new Vector2(-391.4f, 191.4f);
        blueWheel.rectTransform.localPosition = new Vector2(-391.8f, 191.4f);
        greenWheel.rectTransform.localPosition = new Vector2(-391.9f, 191.1f);
        yellowWheel.rectTransform.localPosition = new Vector2(-392f, 190.9f);
        orangeWheel.rectTransform.localPosition = new Vector2(-392f, 191f);

    }

    /// <summary>
    /// Resets the positions of each UI wheel
    /// Resets the fill of the grey wheels
    /// </summary>
    private void ResetWheelUI()
    {   
        // move each wheel to the initial position
        redWheel.rectTransform.localPosition = new Vector2(-593.8f, 191.4f);
        blueWheel.rectTransform.localPosition = new Vector2(-594, 191.4f);
        greenWheel.rectTransform.localPosition = new Vector2(-594.2f, 191.1f);
        yellowWheel.rectTransform.localPosition = new Vector2(-594.3f, 190.9f);
        orangeWheel.rectTransform.localPosition = new Vector2(-594.3f, 191f);

        // reset the fill of the grey wheels
        greyWheel5.fillAmount = 1;
        greyWheel4.fillAmount = 1;
        greyWheel3.fillAmount = 1;
        greyWheel2.fillAmount = 1;
        greyWheel1.fillAmount = 1;

    }
    
    /// <summary>
    /// Decreases the radius of a gray wheel covering a colored circle
    /// Used for bonus round UI when Cookie Hero gets a cookie
    /// </summary>
    /// <param name="speed">Float that determines how much the gray wheel's fill decreases each frame</param>
    /// <param name="healthBarDestination">Float that determines where the gray wheel's fill should be after the bar is done scrolling</param>
    /// <param name="wheel">A reference an Image</param>
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
    
    /// <summary>
    /// Return wheel increase speed depending on the current number of cookies
    /// </summary>
    /// <returns></returns>
    private float GetWheelSpeed()
    {
        if (cookiesSpawned <= 10)
        {   
            return 0.002699f;
        }
        else if (cookiesSpawned > 10 && cookiesSpawned <= 20)
        {   
            return 0.002999f;
        }
        else if (cookiesSpawned > 20 && cookiesSpawned <= 30)
        {
            return 0.004999f;
        }
        else
        {
            return 0.006999f;

        }
        
    }    

    /// <summary>
    /// Returns the correct UI wheel to fill
    /// </summary>
    /// <returns>An Image of the correct wheel UI</returns>
    private Image GetCurrentWheel()
    {
        if (cookiesSpawned <= 10)
        {   
            return greyWheel5;
        }
        else if (cookiesSpawned > 10 && cookiesSpawned <= 20)
        {   
            return greyWheel4;
        }
        else if (cookiesSpawned > 20 && cookiesSpawned <= 30)
        {
            return greyWheel3;
        }
        else if (cookiesSpawned > 30 && cookiesSpawned <= 40)
        {
            return greyWheel2;
        }
        else
        {
            return greyWheel1;
        }
        
    }

    /// <summary>
    /// Triggers the bonus round icon animation
    /// </summary>
    public static void ActivateBonusRoundAnimation()
    {
        // An animation event activates the bonus round spawn
        animator.SetBool("isBonusIcon", true);

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
