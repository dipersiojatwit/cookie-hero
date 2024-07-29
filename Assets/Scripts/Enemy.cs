using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public float minSpeed = 2.5f;
    public float maxSpeed = 4.5f;
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
    /// Called when the trash hits another collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Cookie Hero will take damage if hit
        if(other.CompareTag("Player"))
        {
            // other's gameObject, rather than making a reference to player
            other.gameObject.GetComponent<Player>().TakeDamage(1);
            Vector3 pos = this.transform.position;
            pos.y -= 0.45f;
            Instantiate(hitEffect, pos, Quaternion.identity);
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
