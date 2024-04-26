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

    void Update()
    {
        if(player.transform.position.x > 4.2)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.8f);

        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }

    }

    public static void CookieCrunch()
    {
        animator.SetBool("isCookieCrunch", true);
    }

    // Animation events
    public void SetAnimationFalse(string animation)
    {
        animator.SetBool(animation, false);
    }
}

