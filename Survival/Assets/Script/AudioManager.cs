using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip PlayBGM;

    void Start()
    {
        this.GetComponent<AudioSource>().clip = PlayBGM;    
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindWithTag("Player") != null)
        {
            if(!this.GetComponent<AudioSource>().isPlaying)
            {
                this.GetComponent<AudioSource>().Play();
                this.GetComponent<AudioSource>().loop = true;
            }
        }
    }
}
