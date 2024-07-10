using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadedCookie : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {   
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
        
    }

    public void MoveFadedCookieIn()
    {
        //this.transform.Translate(1, 2);
    }

}
