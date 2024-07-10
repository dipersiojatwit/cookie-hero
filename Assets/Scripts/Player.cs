using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{   
    public int maxHealth = 3;
    private float timeRemaining = 3;
    private static float speed;
    private int random;
    private int health;
    private int healthRestoreValue;
    public static int cookieCount;
    private float input;
    private static float dashDuration;
    private static float dashCooldown;
    private bool isInvincible;
    private bool isShiny;
    private float invincibilityTimer;
    public GameObject shinySparkle;
    public GameObject shinyStar;
    public Heart heart;
    public Stinkus stinkus;
    public AudioClip cookieHeroHurtOne;
    public AudioClip cookieHeroHurtTwo;
    public AudioClip cookieHeroHurtThree;
    public AudioClip chomp;
    public AudioClip crunch;
    public AudioClip cookieScream;
    public AudioClip shinySound;
    private AudioSource audioSource;
    private SpriteRenderer sprite;
    private static Animator animator;
    private Color shinyColor;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        health = 3;
        healthRestoreValue = 20;
        cookieCount = 0;
        dashDuration = 0;
        dashCooldown = 0;

        if (checkShiny())
        {      
            isShiny = true;
            shinyColor = Color.cyan;
            sprite.color = shinyColor;
            Instantiate(shinySparkle);
            Instantiate(shinyStar);
            audioSource.PlayOneShot(shinySound);

        }
        
    }

    void FixedUpdate()
    {   
        GameManager.instance().ResetWheel(false);
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {   
            CheckInvincibility();
            CheckDash();

            // check for horizontal input
            input = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(input * speed, rb.velocity.y);

            // check for dash input. Speed is checked before movement in handled
            if (Input.GetKey(KeyCode.Space))
            {   
                // check if Cookie Hero can dash
                if (dashCooldown <= 0)
                {   
                    // sets a cooldown, raises speed, sets duration of speed-up
                    dashCooldown = 5;
                    speed = 15;
                    dashDuration = 1f;
                    animator.SetBool("isDashing", true);

                    // game manager handles the stamina wheel UI
                    GameManager.instance().EmptyWheel(true);

                }

            }
            
            // move in accordance to horizontal input
            if (input < 0)
            {
                animator.SetBool("isRunningLeft", true);
                animator.SetBool("isRunningRight", false);

            }
            else if(input > 0)
            {
                animator.SetBool("isRunningRight", true);
                animator.SetBool("isRunningLeft", false);
            }
            else
            {
                animator.SetBool("isRunningLeft", false);
                animator.SetBool("isRunningRight", false);

            }

        }

    }

    public void TakeDamage(int damage)
    {   
        // check for invincibility frames
        if (isInvincible)
        {
            return;
        }
        health -= damage;

        // handle UI stuff
        heart.Hurt();
        GameManager.instance().updateHealth(health);

        // check if Cookie Hero dies
        if (health <= 0)
        {   
            heart.HeartDie();

            // Stinkus retreats as soon as Cookie Hero dies
            Stinkus.Reset();
            Stinkus.canAct = false;
            Stinkus.trashCanSpawn = false;
            this.gameObject.SetActive(false);

            // handle UI stuff
            GameManager.instance().deathCanvasSwitch();
            GameManager.instance().EmptyWheel(false);

        }
        else
        {   
            // check if Cookie Hero got hit during trash time
            stinkus.GotHit();

            // set invincibility frames
            isInvincible = true;
            invincibilityTimer = 1f;

            //choose a random hurt voice clip
            random = UnityEngine.Random.Range(1, 100);
            if (random <= 33)
            {
                audioSource.PlayOneShot(cookieHeroHurtOne);
                
                TheCounts.HeadScratch();

            }
            if (random > 33 && random < 69)
            {
                audioSource.PlayOneShot(cookieHeroHurtTwo);

            }
            if (random >= 69)
            {
                audioSource.PlayOneShot(cookieHeroHurtThree);

            }

        }

    }

    public void GetCookie(int value)
    {   
        cookieCount++;
        CookieUI.CookieCrunch();
        GameManager.instance().updateCookieCounter(value);

        // check if TheCounts should cough
        TheCounts.RollCough();

        // only play munch animations while not dashing
        if (dashDuration <= 0)
        {
            if (input < 0)
            {
                animator.SetBool("isMunchWhileLeft", true);
            }
            if (input > 0)
            {
                animator.SetBool("isMunchWhileRight", true);
            }
            else
            {
                animator.SetBool("isMunchWhileStopped", true);

            }

        }

        // check if health restore threshold is surpassed
        if (cookieCount >= healthRestoreValue)
        {
            health++;
            GameManager.instance().updateHealth(health);

            // set health restore value to a new threshold
            healthRestoreValue *= 2;
            audioSource.PlayOneShot(cookieScream);

        }
        else
        {
            // choose a random munching noise
            random = UnityEngine.Random.Range(1, 100);

            if (random < 50)
            {
                audioSource.PlayOneShot(chomp);

            }
            else
            {
                audioSource.PlayOneShot(crunch);

            }
        }

    }

    void CheckDash()
    {   
        // check if dashing
        if (dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            
        }
        else
        {   
            GameManager.instance().EmptyWheel(false);
            speed = 5f;
            dashDuration = 0f;
            dashCooldown -= Time.deltaTime;
            animator.SetBool("isDashing", false);
            GameManager.instance().RainbowWheel(false);

        }

    }

    void CheckInvincibility()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            // create blink effect
            if ((invincibilityTimer > 0.8 && invincibilityTimer < 0.85) 
                || (invincibilityTimer > 0.6 && invincibilityTimer < 0.65)
                || (invincibilityTimer > 0.4 && invincibilityTimer < 0.45)
                || (invincibilityTimer > 0.2 && invincibilityTimer < 0.25))
            {
                sprite.color = new Color(1, 1, 1, 0);
            }
            else
            {   
                // check isShiny to maintain shiny colors after blink effect
                if (isShiny)
                {
                    sprite.color = Color.cyan;
                }
                else
                {
                    sprite.color = new Color(1, 1, 1, 1);
                }
            }    
            
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    // Called when getting a candy cookie
    public static void ResetDashDuration()
    {   
        // if not dashing, make Cookie Hero automatically dash
        Debug.Log(dashDuration);
        if (dashDuration <= 0)
        {   
            Debug.Log("Auto dash");
            dashCooldown = 5;
            speed = 15;
            animator.SetBool("isDashing", true);

        }

        // dash duration is reset for each candy cookie
        dashDuration = 2.5f;
        GameManager.instance().RainbowWheel(true);
    }

    public void Reset()
    {   
        health = maxHealth;
        healthRestoreValue = 20;
        cookieCount = 0;
        timeRemaining = 3;
        dashDuration = 0;
        dashCooldown = 0;
        speed = 5;
        isShiny = false;
        Vector3 pos = new Vector3(0.0f, -1.85f, 0.0f);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
        GameManager.instance().updateHealth(maxHealth);
        GameManager.instance().resetCookieCounter();
        GameManager.instance().ResetWheel(true);
        GameManager.instance().EmptyWheel(false);
        sprite = GetComponent<SpriteRenderer>();
        if (checkShiny())
        {   
            isShiny = true;
            shinyColor = Color.cyan;
            sprite.color = shinyColor;
            Instantiate(shinySparkle);
            Instantiate(shinyStar);
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(shinySound);

        }
        else
        {
            sprite.color = Color.white;

        }

    }

    public Boolean checkShiny()
    {
        return (UnityEngine.Random.Range(1, 8192)) == 8192;

    }

    // Animation events
    public void MunchFinished()
    {
        animator.SetBool("isMunchWhileStopped", false);
        animator.SetBool("isMunchWhileLeft", false);
        animator.SetBool("isMunchWhileRight", false);
        
    }

}
