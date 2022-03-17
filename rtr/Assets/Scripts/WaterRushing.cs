using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRushing : MonoBehaviour
{
    public float adjustment = .1f;

    public AudioClip rush1;
    public AudioClip rush2;
    public AudioClip rush3;

    private bool playing = false;
    private float timePassed = 0f;
    
    private AudioClip sound;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        PlayRush();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;
        if (timePassed >= sound.length - adjustment) {
            playing = false;
            PlayRush();
            timePassed = 0f;
        }
    }

    private void PlayRush() {
        float chance = Random.Range(0f, 1f);
        if (chance <= .33f) {
            sound = rush1;
        } else if (chance <= .66f) {
            sound = rush2;
        } else {
            sound = rush3;
        }
        audio.PlayOneShot(sound);
    }
}
