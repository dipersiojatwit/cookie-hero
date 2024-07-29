using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadedCookie : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public static bool isFadeIn;
    public static bool isFadeOut;
    private float currentFade;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentFade = 0;

    }

    // Update is called once per frame
    void Update()
    {   
        // Gives color to the faded cookie UI element depending on the bonus round cookie score
        if (BonusRound.isBonus)
        {
            if (BonusRound.bonusRoundCookies == 10)
            {
                animator.SetBool("isRed", true);
            }
            else if (BonusRound.bonusRoundCookies == 20)
            {
                animator.SetBool("isBlue", true);
            }
            else if (BonusRound.bonusRoundCookies == 30)
            {
                animator.SetBool("isGreen", true);
            }
            else if (BonusRound.bonusRoundCookies == 30)
            {
                animator.SetBool("isGreen", true);
            }
            else if (BonusRound.bonusRoundCookies == 40)
            {
                animator.SetBool("isYellow", true);
            }
            else if (BonusRound.bonusRoundCookies == 50)
            {
                animator.SetBool("isOrange", true);

            }

        }

        // Update fading in/ out
        if (isFadeIn)
        {
            FadeCookieIn();

        }

        if (isFadeOut)
        {
            FadeCookieOut();

        }
        
    }

    /// <summary>
    /// Slowly fades the cookie UI in by increasing alpha each frame
    /// </summary>
    public void FadeCookieIn()
    {
        spriteRenderer.color = new Color(255, 255, 255, currentFade);
        currentFade += 0.01f;

        // Check if the UI is completly faded in
        if (spriteRenderer.color.a > 250)
        {   
            Debug.Log("Faded in");
            isFadeIn = false;
            isFadeOut = false;
            currentFade = 255f;

        }

    }

    /// <summary>
    /// Slowly fades the cookie UI out by decresing alpha each frame
    /// </summary>
    public void FadeCookieOut()
    {
        spriteRenderer.color = new Color(255, 255, 255, 0);
        //currentFade -= 0.1f;
        Debug.Log("Fade out " + currentFade);
        Debug.Log("Current fade: " + spriteRenderer.color.a);
        // Check if the cookie UI is completly faded out
        if (spriteRenderer.color.a < 2)
        {
            isFadeOut = false;
            currentFade = 0.00f;
            Debug.Log("Should be faded out");
            Debug.Log(spriteRenderer.color.a);
            isFadeIn = false;

        }

        // Reset the colors after fade out
        animator.SetBool("isRed", false);
        animator.SetBool("isBlue", false);
        animator.SetBool("isGreen", false);
        animator.SetBool("isYellow", false);
        animator.SetBool("isOrange", false);

    }

}
