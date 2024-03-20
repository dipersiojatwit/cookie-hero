using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{   
    public int random;
    public float speed;
    public int maxHealth = 3;
    private float timeRemaining = 2;
    private int health;
    private int healthRestoreValue;
    private int cookieCount;
    private float input;
    private float dashDuration;
    private float dashCooldown;
    public AudioClip cookieHeroHurtOne;
    public AudioClip cookieHeroHurtTwo;
    public AudioClip cookieHeroHurtThree;
    public AudioClip chomp;
    public AudioClip crunch;
    public AudioClip cookieScream;
    private AudioSource audioSource;
    private Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        health = 3;
        healthRestoreValue = 20;
        cookieCount = 0;
        dashDuration = 0;
        dashCooldown = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        GameManager.instance().ResetWheel(false);
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {   
            CheckDash();
            input = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(input * speed, rb.velocity.y);

            if (Input.GetKey(KeyCode.Space))
            {   
                if (dashCooldown <= 0)
                {   
                    dashCooldown = 5;
                    speed = 15;
                    dashDuration = 1f;
                    animator.SetBool("isDashing", true);
                    GameManager.instance().EmptyWheel(true);
                }
            }
            
            if (input < 0)
            {
                animator.SetBool("isRunningLeft", true);
            }
            else if(input > 0)
            {
                animator.SetBool("isRunningRight", true);
            }
            else
            {
                animator.SetBool("isRunningLeft", false);
                animator.SetBool("isRunningRight", false);
            }
        }

    }

    public void takeDamage(int damage)
    {   
        health -= damage;
        GameManager.instance().updateHealth(health);

        // check if Cookie Hero dies
        if (health <= 0)
        {   
            this.gameObject.SetActive(false);
            GameManager.instance().deathCanvasSwitch();
        }
        else
        {
            //choose a random hurt voice clip
            random = UnityEngine.Random.Range(1, 100);
            if (random <= 33)
            {
                audioSource.PlayOneShot(cookieHeroHurtOne);
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
        GameManager.instance().updateCookieCounter(value);

        // only play munch animations while not dashing
        if (dashDuration <= 0)
        {
            if (input == 0)
            {
                animator.SetBool("isMunchWhileStopped", true);
            }
            if (input < 0)
            {
                animator.SetBool("isMunchWhileLeft", true);
            }
            if (input > 0)
            {
                animator.SetBool("isMunchWhileRight", true);
            }

        }

        // check if health restore threshold is surpassed
        if (cookieCount >= healthRestoreValue)
        {
            health++;
            GameManager.instance().updateHealth(health);
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
        }
    }

    public void Reset()
    {
        health = maxHealth;
        healthRestoreValue = 20;
        cookieCount = 0;
        Vector3 pos = new Vector3(0.0f, -1.47f, 0.0f);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
        GameManager.instance().updateHealth(maxHealth);
        GameManager.instance().resetCookieCounter();
        GameManager.instance().ResetWheel(true);
        GameManager.instance().EmptyWheel(false);
        timeRemaining = 2;
        dashDuration = 0;
        dashCooldown = 0;
        speed = 5;

    }

    // animation events
    public void MunchFinished()
    {
        animator.SetBool("isMunchWhileStopped", false);
        animator.SetBool("isMunchWhileLeft", false);
        animator.SetBool("isMunchWhileRight", false);
        
    }
}
