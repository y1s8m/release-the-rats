using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPipes : MonoBehaviour
{
    public AudioClip[] creaks;
    private int index = -1;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Creak() {
        float chance = Random.Range(0f, 1f);
        if (chance > .997f)  {
            int last = index;
            while (index == last) {
                float next = Random.Range(0f, 1f);
                for (int i = 0; i < creaks.Length; i++) {
                    if (next < ((i + 1f) / creaks.Length)) {
                        index = i;
                        break;
                    }
                }
            }
            audio.PlayOneShot(creaks[index]);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") Creak();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cauldron") Destroy(this.gameObject);
    }
}
