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

        // Check if shiny odds are rolled true
        if (CheckShiny())
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
        GameManager.Instance().ResetWheel(false);
        timeRemaining -= Time.deltaTime;

        // Check if The Counts is done counting
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
                    GameManager.Instance().EmptyWheel(true);

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

    /// <summary>
    /// Called from Enemy script when colliding with Cookie Hero
    /// </summary>
    /// <param name="damage">The amount of damage the trash deals</param>
    public void TakeDamage(int damage)
    {   
        // check for invincibility frames
        if (isInvincible)
        {
            return;
        }
        health -= damage;

        // handle UI elements
        heart.Hurt();
        GameManager.Instance().UpdateHealth(health);

        // check if Cookie Hero dies
        if (health <= 0)
        {   
            heart.HeartDie();

            // Stinkus retreats as soon as Cookie Hero dies
            Stinkus.Reset();
            Stinkus.canAct = false;
            Stinkus.trashCanSpawn = false;
            this.gameObject.SetActive(false);

            // handle UI elements
            GameManager.Instance().DeathCanvasSwitch();
            GameManager.Instance().EmptyWheel(false);

        }
        else
        {   
            // check if Cookie Hero got hit during trash time
            stinkus.GotHit();

            // set invincibility frames true for some time
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

    /// <summary>
    /// Called from cookie scripts when colliding with Cookie Hero
    /// </summary>
    /// <param name="value">The cookie score amount to increase</param>
    public void GetCookie(int value)
    {   
        cookieCount++;
        CookieUI.CookieCrunch();
        GameManager.Instance().UpdateCookieCounter(value);

        // Check if TheCounts should cough
        TheCounts.RollCough();

        // Only play munch animations while not dashing
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

        // Check if health restore threshold is surpassed
        if (cookieCount >= healthRestoreValue)
        {
            health++;
            GameManager.Instance().UpdateHealth(health);

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

    /// <summary>
    /// Checks and updates the status of dashing
    /// </summary>
    void CheckDash()
    {   
        // Check if dashing and update
        if (dashDuration > 0)
        {
            dashDuration -= Time.deltaTime;
            
        }
        else
        {   
            GameManager.Instance().EmptyWheel(false);
            speed = 5f;
            dashDuration = 0f;
            dashCooldown -= Time.deltaTime;
            animator.SetBool("isDashing", false);
            GameManager.Instance().RainbowWheel(false);

        }

    }

    /// <summary>
    /// Check and update the status of invincibility
    /// </summary>
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
            
            // Check if invincibility has run out
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;

            }

        }

    }

    /// <summary>
    /// Called when getting a candy cookie to make Cookie Hero dash
    /// </summary>
    public static void ResetDashDuration()
    {   
        // If not dashing, make Cookie Hero automatically dash
        if (dashDuration <= 0)
        {   
            dashCooldown = 5;
            speed = 15;
            animator.SetBool("isDashing", true);

        }

        // Dash duration is reset for each candy cookie
        dashDuration = 2.5f;
        GameManager.Instance().RainbowWheel(true);
    }

    /// <summary>
    /// Resets the status of Cookie Hero
    /// Called when he respawns
    /// </summary>
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
        GameManager.Instance().UpdateHealth(maxHealth);
        GameManager.Instance().ResetCookieCounter();
        GameManager.Instance().ResetWheel(true);
        GameManager.Instance().EmptyWheel(false);
        sprite = GetComponent<SpriteRenderer>();

        // Roll shiny odds on respawn
        if (CheckShiny())
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

    /// <summary>
    /// Rolls shiny odds
    /// </summary>
    /// <returns>True if the random number is 8192</returns>
    public Boolean CheckShiny()
    {
        return (UnityEngine.Random.Range(1, 8192)) == 8192;

    }

    /// <summary>
    /// An animation event for munch animations
    /// </summary>
    public void MunchFinished()
    {
        animator.SetBool("isMunchWhileStopped", false);
        animator.SetBool("isMunchWhileLeft", false);
        animator.SetBool("isMunchWhileRight", false);
        
    }

}
