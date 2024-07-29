using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics.Eventing.Reader;
public class GameManager : MonoBehaviour
{
    // Singlton
    private static GameManager _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }

    public static GameManager Instance()
    {
        return _instance;

    }

    // Other stuff
    public int cookieCount = 0;
    public int highScore = 0;
    public int random;
    private float transitionTimer = 0.4f;
    public Canvas deathCanvas;
    public TMP_Text healthText;
    public TMP_Text cookieCounterText;
    public TMP_Text roundScore;
    public GameObject spriteOne;
    public GameObject spriteTwo;
    public GameObject spriteThree; 
    public TheCounts theCounts;
    public AudioClip cookieHeroDeathOne;
    public AudioClip cookieHeroDeathTwo;
    public AudioClip executeOrder;
    private Player player;
    private Spawner spawner;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    private void Start()
    {   
        // Don't use this if called every frame
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        theCounts = GameObject.FindGameObjectWithTag("TheCounts").GetComponent<TheCounts>();
        animator = GameObject.FindGameObjectWithTag("Wheel").GetComponent<Animator>();
        spriteRenderer = GameObject.FindGameObjectWithTag("Wheel").GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        player.Reset();
        healthText.alpha = 0;
        cookieCounterText.alpha = 0;

        // Disable death UI 
        deathCanvas.gameObject.SetActive(false);

    }

    void Update()
    {   
        // Check if the circle fade out is done
        if (transitionTimer <= 0)
        {
            healthText.alpha = 255;
            cookieCounterText.alpha = 255;
        }
        else
        {
            transitionTimer -= Time.deltaTime;

        }

        // Make UI elements transparent on overlay
        if(player.transform.position.x < -5.1)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.8f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

        }

    }

    /// <summary>
    /// Updates the health text
    /// </summary>
    /// <param name="value">The health value to display</param>
    public void UpdateHealth(int value)
    {
        healthText.text = "x" + value;

    }

    public void UpdateCookieCounter(int value)
    {
        cookieCount += value;
        cookieCounterText.text = "x" + cookieCount;

    }

    /// <summary>
    /// Triggers the stamina animation to deplete
    /// </summary>
    /// <param name="status">A bool to set the status of the animation</param>
    public void EmptyWheel(Boolean status)
    {
        animator.SetBool("isDashing", status);

    }

    /// <summary>
    /// Called to trigger the stamina reset animation
    /// </summary>
    /// <param name="status">The bool to set the animation status to</param>
    public void ResetWheel(Boolean status)
    {
        animator.SetBool("isReset", status);

    }
    
    /// <summary>
    /// Sets the status of the rainbow wheel animation
    /// </summary>
    /// <param name="status">The bool to set the animation true or false</param>
    public void RainbowWheel(Boolean status)
    {
        animator.SetBool("isRainbow", status);

    }

    /// <summary>
    /// Resets the cookie counter UI to 0
    /// </summary>
    public void ResetCookieCounter()
    {
        cookieCount = 0;
        cookieCounterText.text = "x" + cookieCount;

    }

    /// <summary>
    /// Switches the status of the death canvas screen
    /// </summary>
    public void DeathCanvasSwitch()
    {   
        spawner.gameObject.SetActive(false);
        Stinkus.trashCanSpawn = false;
        Stinkus.canAct = false;

        // Make sure death canvas isn't already on
        if (deathCanvas.gameObject.activeSelf)
        {
            deathCanvas.gameObject.SetActive(false);
        }
        else
        {   
            roundScore.text = "x" + cookieCount;

            // MenuManager will check if high score should update
            MenuManager.instance().updateHighScore(cookieCount);
            deathCanvas.gameObject.SetActive(true);

            // choose a random death sound if order 66 isn't executed
            random = UnityEngine.Random.Range(1, 100);
            if (cookieCount == 66)
            {
                audioSource.PlayOneShot(executeOrder);
            }
            else if (random < 50)
            {
                audioSource.PlayOneShot(cookieHeroDeathOne);
            }
            else if(random >= 50)
            {
                audioSource.PlayOneShot(cookieHeroDeathTwo);
            }
        
        }

    }

    /// <summary>
    /// Called when the reset button is pressed to try again
    /// </summary>
    public void OnResetClick()
    {   
        if(deathCanvas.gameObject.activeSelf)
        {
            DeathCanvasSwitch();
            player.Reset();
            spawner.gameObject.SetActive(true);
            Spawner.canSpawn = true;
            Stinkus.trashCanSpawn = true;
            Stinkus.canAct = true;
            spawner.Reset();
            theCounts.Reset();
            Stinkus.Reset();
            Heart.SetHeartDieFalse();

        }

    }

    /// <summary>
    /// Called when clicking the menu button to load the menu
    /// </summary>
    public void OnMenuClick()
    {
        SceneManager.LoadScene("Menu");

    }

    // animation events
    public void numberOne()
    {
        spriteOne.SetActive(true);

    }

    public void numberTwo()
    {
        spriteOne.SetActive(false);
        spriteTwo.SetActive(true);

    }

    public void numberThree()
    {   
        spriteTwo.SetActive(false);
        spriteThree.SetActive(true);

    }

    public void endCount()
    {
        spriteThree.SetActive(false);

    }

}
