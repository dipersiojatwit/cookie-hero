using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{   
    private static Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Check Cookie Hero's position to update transparency
        if(player.transform.position.x < -7.2)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.8f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

        }

    }

    /// <summary>
    /// Sets the heart hurt animation to play
    /// </summary>
    public void Hurt()
    {
        animator.SetBool("isHeartHurt", true);
        
    }

    /// <summary>
    /// Sets the heart die animation to play
    /// </summary>
    public void HeartDie()
    {
        animator.SetBool("isHeartDie", true);

    }

    /// <summary>
    /// Sets the animation events for the UI heart to false
    /// </summary>
    public static void SetHeartDieFalse()
    {
        animator.SetBool("isHeartDie", false);
        animator.SetBool("isHeartHurt", false);

    }

    /// <summary>
    /// Sets an animation bool to false
    /// </summary>
    /// <param name="animation">A string containing the animation bool</param>
    public void SetAnimationFalse(string animation)
    {
        animator.SetBool(animation, false);

    }

}
