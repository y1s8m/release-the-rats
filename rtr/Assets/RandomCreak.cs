using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCreak : MonoBehaviour
{
    public AudioClip creakSound;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Creak() {
        float chance = Random.Range(0f, 1f);
        if (chance > .997f)  {
            audio.PlayOneShot(creakSound);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") Creak();
    }
}
