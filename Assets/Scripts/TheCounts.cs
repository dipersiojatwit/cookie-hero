using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class TheCounts : MonoBehaviour
{
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    private AudioSource audioSource;
    private static Animator animator;

     // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetBool("isReset", true);

    }

    public void Reset()
    {   
        animator.SetBool("isReset", true);

    }

    public static void RollCough()
    {
        if (UnityEngine.Random.Range(1, 22) == 9)
        {
            animator.SetBool("isCoughing", true);

        }

    }

    // function to set any animation to true or false through animation events
    public void SetAnimation(string animationName)
    {
        animator.SetBool(animationName, !animator.GetBool(animationName));

    }

    public static void HeadScratch()
    {
        animator.SetBool("isScratch", true);

    }

    // animation events
    public void OnOne()
    {   
        audioSource.PlayOneShot(one);
        GameManager.instance().numberOne();

    }

    public void OnTwo()
    {   
        audioSource.PlayOneShot(two);
        GameManager.instance().numberTwo();

    }

    public void OnThree()
    {
        audioSource.PlayOneShot(three);
        GameManager.instance().numberThree();

    }

    public void OnCountEnd()
    {
        GameManager.instance().endCount();
        animator.SetBool("isReset", false);

    }
    
}
