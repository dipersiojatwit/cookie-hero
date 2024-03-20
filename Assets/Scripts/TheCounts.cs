using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor.Animations;
using UnityEditor.U2D;
using UnityEngine;

public class count : MonoBehaviour
{
    public AudioClip one;
    public AudioClip two;
    public AudioClip three;
    private AudioSource audioSource;
    private Animator animator;

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

    // animation events
    public void onOne()
    {   
        audioSource.PlayOneShot(one);
        GameManager.instance().numberOne();

    }

    public void onTwo()
    {   
        audioSource.PlayOneShot(two);
        GameManager.instance().numberTwo();

    }

    public void onThree()
    {
        audioSource.PlayOneShot(three);
        GameManager.instance().numberThree();

    }

    public void onCountEnd()
    {
        GameManager.instance().endCount();
        animator.SetBool("isReset", false);

    }
    
}
