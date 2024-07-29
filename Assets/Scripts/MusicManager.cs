using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{   

    // singlton stuff
    private static MusicManager _instance = null;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this; 
        }
        else
        {
            Destroy(this.gameObject);

        }

        DontDestroyOnLoad(this);
    }

    public static MusicManager Instance()
    {
        return _instance; 

    }

    // Other stuff
    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();

        }

    }

}
