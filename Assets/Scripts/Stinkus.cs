using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class Stinkus : MonoBehaviour
{   
    public GameObject[] trash;
    public GameObject player;
    public GameObject trashTimeSign;
    public static float timeBetweenActions;
    public static bool canAct;
    public AudioClip fastEmerge;
    public AudioClip slowEmerge;
    public AudioClip retreat;
    public AudioClip asthma;
    public AudioClip mock;
    public AudioClip raspyLaugh;
    public AudioClip largeLaugh;
    private AudioSource audioSource;
    private static Animator animator;
    private static bool isTrashTime;
    private static bool hasLaughed;
    public static bool trashCanSpawn;
    public static int timesHit = 0;
    private bool isStaring;
    private int random;
    private static float spawnTimer;
    private float stareTimer;
    private static float trashTimer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        trashCanSpawn = true;
        canAct = true;
        timeBetweenActions = 20;

    }

    // Update is called once per frame
    void Update()
    {   
        // check if trash time should be updated
        if (isTrashTime)
        {
            UpdateTrashTime();
        }
        else
        {
            timeBetweenActions -= Time.deltaTime;
        }

        // check if Stinkus should act
        if (timeBetweenActions <= 0 && !isTrashTime && !isStaring && canAct && !BonusRound.isBonus)
        {
            random = Random.Range(1, 100);
            // Check for shake
            if (random <= 25)
            {
                animator.SetBool("isShaking", true);
                // check for asthma
                if (random <= 5)
                {
                    audioSource.PlayOneShot(asthma);
                }
            }
            // check for creep up
            if (random > 25 && random <= 45)
            {
                animator.SetBool("isCreepUp", true);
                isStaring = true;
                timeBetweenActions = 10;
                stareTimer = Random.Range(8, 16);
            }
            // check for false fake out
            if (random > 45 && random <= 60)
            {
                animator.SetBool("isFalseFakeOut", true);
            }
            // check for pop up slow
            if (random > 60 && random <= 80)
            {   
                // an animation event calls ActivateTrashTime()
                audioSource.PlayOneShot(slowEmerge);
                animator.SetBool("isPopUpSlow", true);
            }
            // check for pop up fast
            if (random > 80 && random <= 95)
            {   
                // an animation event calls ActivateTrashTime()
                audioSource.PlayOneShot(fastEmerge);
                animator.SetBool("isPopOutFast", true);
            }
            // check for fake out
            if (random > 95)
            {   
                // an animation event calls ActivateTrashTime()
                // an animation event plays the audio clip for this one
                animator.SetBool("isFakeOut", true);
            }
            
            // Reset the action timer
            timeBetweenActions = 15;

        }

        // update staring state if staring
        if (isStaring)
        {   
            stareTimer -= Time.deltaTime;
            if (stareTimer <= 0)
            {
                isStaring = false;
                animator.SetBool("isLookLeft", false);
                animator.SetBool("isLookRight", false);
                animator.SetBool("isSmallRetreat", true);
                return;
            }
            if (player.transform.position.x < 1)
            {
                animator.SetBool("isLookLeft", true);
                animator.SetBool("isLookRight", false);
            }
            if (player.transform.position.x > 1)
            {
                animator.SetBool("isLookLeft", false);
                animator.SetBool("isLookRight", true);
            }

        }

    }

    private void UpdateTrashTime()
    {
        trashTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        if (trashTimer <= 0)
        {
            isTrashTime = false;
            hasLaughed = false;
            trashTimeSign.SetActive(false);
            animator.SetBool("isRetreat", true);
            audioSource.PlayOneShot(retreat);
            spawnTimer = 0.5f;
            timeBetweenActions = 20;
            
            // check if the bonus round should be triggered
            if (timesHit < 10)
            {   
                Reset();
                BonusRound.ActivateBonusRoundAnimation();
            }
            else
            {
                Spawner.canSpawn = true;
            }
            timesHit = 0;

        }
        else if (spawnTimer <= 0 && trashCanSpawn)
        {
            int index = Random.Range(0, trash.Length);
            GameObject enemy = trash[index];
            float x = Random.Range(-9.0f, 9.0f);
            Vector3 pos = new Vector3(x, 10, 0);
            // Quaternion for gimble lock prevention, spawn with Instantiate
            Instantiate(enemy, pos, Quaternion.identity);
            if (index == 1 && !hasLaughed && trashTimer > 3)
            {
                PlayLaughClip();
                hasLaughed = true;
            }

            // reset timer
            spawnTimer = 0.5f;

        }
        
    }

    public static void Reset()
    {
        if (isTrashTime)
        {
            animator.SetBool("isRetreat", true);
        }
        else
        {
            animator.SetBool("isSmallRetreat", true);
        }
        isTrashTime = false;
        trashTimer = 0;
        spawnTimer = 0.5f;
        timeBetweenActions = 20;
        timesHit = 0;
        
    }

    public void ActivateTrashTime()
    {   
        trashTimeSign.SetActive(true);

        // Trash time duration depends on the cookie count
        if (Player.cookieCount < 10)
        {
            trashTimer = 10.5f;
        }
        else if (Player.cookieCount < 20)
        {
            trashTimer = 12.5f;
        }
        else if (Player.cookieCount < 30)
        {
            trashTimer = 14.5f;
        }
        else
        {
            trashTimer = 17.5f;
        }

        isTrashTime = true;
        Spawner.canSpawn = false;
        timeBetweenActions = 25;

        // start the spawn timer higher than normal so trash won't immediatly spawn when trash time starts
        spawnTimer = 2.5f;

    }

    // Animation events
    public void SetAnimationFalse(string animation)
    {
        animator.SetBool(animation, false);

    }

    public void PlayClip(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);

    }

    public void PlayLaughClip()
    {   
        // hasLaughed assures avoidance of voice clip overload
        if (!hasLaughed && isTrashTime && trashTimer > 3)
        {   
            if (Random.Range(1, 100) <= 50)
            {
                audioSource.PlayOneShot(largeLaugh);
            }
            else
            {
                audioSource.PlayOneShot(raspyLaugh);
            }
            hasLaughed = true;

        }

    }

    public void GotHit()
    {   
        /*
        times hit is only kept track of during trash time 
        to determine if the bonus round should start. Times hit
        is set back to 0 at the end of every trash time. 
        */
        if (isTrashTime)
        {
            timesHit++;
            PlayLaughClip();
        }

    }

}
