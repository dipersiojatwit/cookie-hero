using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{   
    private float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.6f, 0.75f);
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime);
        
        if (this.transform.position.x > 12)
        {
            Destroy(this.gameObject);

        }
    }
}
