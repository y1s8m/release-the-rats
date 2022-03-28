using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbienceManager : MonoBehaviour
{
    public AudioClip sewerClip;
    public AudioClip kitchenClip;
    
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 1) audio.clip = sewerClip;
        else if (SceneManager.GetActiveScene().buildIndex == 3) audio.clip = kitchenClip;

        audio.loop = true;
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
