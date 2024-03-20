using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using TMPro;
using UnityEngine.SceneManagement;
using System;
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
    public Canvas deathCanvas;
    public TMP_Text healthText;
    public TMP_Text cookieCounter;
    public TMP_Text roundScore;
    public GameObject spriteOne;
    public GameObject spriteTwo;
    public GameObject spriteThree; 
    public count theCounts;
    public AudioClip cookieHeroDeathOne;
    public AudioClip cookieHeroDeathTwo;
    private Player player;
    private Spawner spawner;
    private AudioSource audioSource;
    private Animator animator;
    
    private void Start()
    {   
        // don't use this if called every frame
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        theCounts = GameObject.FindGameObjectWithTag("TheCounts").GetComponent<count>();
        animator = GameObject.FindGameObjectWithTag("Wheel").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player.Reset();

        //disable ui
        deathCanvas.gameObject.SetActive(false);

    }

    public void updateHealth(int value)
    {
        healthText.text = "x" + value;

    }

    public void updateCookieCounter(int value)
    {
        cookieCount += value;
        cookieCounter.text = "x" + cookieCount;

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
        cookieCounter.text = "x" + cookieCount;

    }

    public void deathCanvasSwitch()
    {   
        
        spawner.gameObject.SetActive(false);
        if (deathCanvas.gameObject.activeSelf)
        {
            deathCanvas.gameObject.SetActive(false);
        }
        else
        {   
            roundScore.text = "x" + cookieCount;
            if(MenuManager.instance().updateHighScore(cookieCount))
            {
                
            }
            
            deathCanvas.gameObject.SetActive(true);
            random = UnityEngine.Random.Range(1, 100);
            if (random < 50)
            {
                audioSource.PlayOneShot(cookieHeroDeathOne);
            }
            else
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
        spawner.Reset();
        theCounts.Reset();
        }

    }

    public void onMenuClick()
    {
        SceneManager.LoadScene("Menu");

    }

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
