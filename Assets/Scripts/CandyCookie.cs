using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CandyCookie : MonoBehaviour
{
    public float minSpeed = 2.5f;
    public float maxSpeed = 4.5f;
    public int cookieValue;
    private float speed;
    public ParticleSystem candyHitEffect;
    public ParticleSystem hitEffect;
    public ParticleSystem candyOne;
    public ParticleSystem candyTwo;
    public ParticleSystem candyThree;
    public ParticleSystem candyFour;
    public ParticleSystem candyFive;
    public ParticleSystem candySix;
    public ParticleSystem candySeven;
    public ParticleSystem candyEight;
    public ParticleSystem candyNine;
    public ParticleSystem candyTen;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.down * speed * Time.deltaTime);
        
    }

    /// <summary>
    /// Called when a collider enters the candy cookie's collider
    /// </summary>
    /// <param name="other">The collider of the object that triggers the method</param>
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Check if Cookie Hero eats the candy cookie
        if(other.CompareTag("Player"))
        {
            // other's gameObject, rather than making a reference to player
            other.gameObject.GetComponent<Player>().GetCookie(cookieValue);

            // Eating a candy cookie will allow Cookie Hero to continue dashing
            Player.ResetDashDuration();
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;

            // The ammount of particles to emit depends on the candy cookie score
            var emission = candyHitEffect.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);          
            bursts[0].count = BonusRound.bonusRoundCookies + 1;
            emission.SetBursts(bursts);

            // Instantiate the candy hit effect now that the particle count is determined
            Instantiate(candyHitEffect, pos, Quaternion.identity);
    
            // Instantiate the correct candy number effect if the cookie eaten is not the bonus cookie
            if (!BonusRound.isEnd)
            {
                Instantiate(GetCandyNumberEffect(), pos, Quaternion.identity);
                BonusRound.bonusRoundCookies++;
                BonusRound.cookiesInBatch++;

            }

            // Marks the current wheel position as the starting position for this candy cookie
            BonusRound.isInitialWheelUpdate = true;
            BonusRound.isTick = true;

            // destroy the cookie after it hits the player
            Destroy(this.gameObject);

        }

        // Destroy the candy cookie if it hits the ground
        if (other.CompareTag("Ground"))
        {   
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;

            // A different particle effect plays if the candy cookie hits the ground
            Instantiate(hitEffect, pos, Quaternion.identity);
            Destroy(this.gameObject);

        }

    }

    /// <summary>
    /// Gets the correct candy number particle system depending on cookieInBatch
    /// </summary>
    /// <returns>A ParticleSystem</returns>
    private ParticleSystem GetCandyNumberEffect()
    {
        return BonusRound.cookiesInBatch switch
        {
            0 => candyOne,
            1 => candyTwo,
            2 => candyThree,
            3 => candyFour,
            4 => candyFive,
            5 => candySix,
            6 => candySeven,
            7 => candyEight,
            8 => candyNine,
            9 => candyTen,
            _ => candyOne,

        };

    }

}