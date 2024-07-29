using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieUI : MonoBehaviour
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
        // Make the UI slightly transparent if Cookie Hero is behind it
        if(player.transform.position.x > 4.2)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.8f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

        }

    }

    /// <summary>
    /// Triggers an animation for the UI cookie
    /// </summary>
    public static void CookieCrunch()
    {
        animator.SetBool("isCookieCrunch", true);

    }

    /// <summary>
    /// Sets an animation bool false based on a string
    /// </summary>
    /// <param name="animation">The string of the animation bool</param>
    public void SetAnimationFalse(string animation)
    {
        animator.SetBool(animation, false);

    }
    
}

