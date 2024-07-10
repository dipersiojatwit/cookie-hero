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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // other's gameObject, rather than making a reference to player
            other.gameObject.GetComponent<Player>().GetCookie(cookieValue);
            Player.ResetDashDuration();
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;
            ParticleSystem.EmissionModule emissionVar = candyHitEffect.emission;
            var emission = candyHitEffect.emission;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);          
            bursts[0].count = BonusRound.bonusRoundCookies + 1;
            emission.SetBursts(bursts);
            Instantiate(candyHitEffect, pos, Quaternion.identity);
            BonusRound.bonusRoundCookies++;
            BonusRound.cookiesInBatch++;
            BonusRound.isInitialWheelUpdate = true;
            BonusRound.isTick = true;
            Debug.Log("Tick true");
    
            // destroy the cookie after it hits the player
            Destroy(this.gameObject);

        }

        if (other.CompareTag("Ground"))
        {   
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;
            Instantiate(hitEffect, pos, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }

}

