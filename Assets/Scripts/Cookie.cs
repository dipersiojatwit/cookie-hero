using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    public float minSpeed = 2.5f;
    public float maxSpeed = 4.5f;
    public int cookieValue;
    private float speed;
    public GameObject hitEffect;

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
    /// Called when the cookie colides with another collider
    /// </summary>
    /// <param name="other">The collider of what the cookie collided with</param>
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Check if Cookie Hero eats the cookie
        if(other.CompareTag("Player"))
        {
            // other's gameObject, rather than making a reference to player
            other.gameObject.GetComponent<Player>().GetCookie(cookieValue);
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;
            Instantiate(hitEffect, pos, Quaternion.identity);
            BonusRound.bonusRoundCookies++;

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
