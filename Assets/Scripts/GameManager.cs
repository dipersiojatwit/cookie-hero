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
    //singlton
    private static GameManager _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
    }

    public static GameManager instance()
    {
        return _instance;

    }

    //Other stuff
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
    public count theCounts;
    public AudioClip cookieHeroDeathOne;
    public AudioClip cookieHeroDeathTwo;
    public AudioClip executeOrder;
    private Player player;
    private Spawner spawner;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {   
        // don't use this if called every frame
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        theCounts = GameObject.FindGameObjectWithTag("TheCounts").GetComponent<count>();
        animator = GameObject.FindGameObjectWithTag("Wheel").GetComponent<Animator>();
        spriteRenderer = GameObject.FindGameObjectWithTag("Wheel").GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        player.Reset();
        healthText.alpha = 0;
        cookieCounterText.alpha = 0;

        //disable death UI 
        deathCanvas.gameObject.SetActive(false);

    }

    void Update()
    {   
        if (transitionTimer <= 0)
        {
            healthText.alpha = 255;
            cookieCounterText.alpha = 255;
        }
        else
        {
            transitionTimer -= Time.deltaTime;

        }

        // make UI elements transparent on overlay
        if(player.transform.position.x < -5.1)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.8f);

        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }

    }

    public void updateHealth(int value)
    {
        healthText.text = "x" + value;

    }

    public void updateCookieCounter(int value)
    {
        cookieCount += value;
        cookieCounterText.text = "x" + cookieCount;

    }

    public void EmptyWheel(Boolean status)
    {
        animator.SetBool("isDashing", status);

    }

    public void ResetWheel(Boolean status)
    {
        animator.SetBool("isReset", status);

    }

    public void resetCookieCounter()
    {
        cookieCount = 0;
        cookieCounterText.text = "x" + cookieCount;

    }

    public void deathCanvasSwitch()
    {   
        
        spawner.gameObject.SetActive(false);
        Stinkus.trashCanSpawn = false;
        Stinkus.canAct = false;

        // make sure death canvas isn't already on
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


    public void onResetClick()
    {   
        if(deathCanvas.gameObject.activeSelf)
        {
            deathCanvasSwitch();
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

    public void onMenuClick()
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
