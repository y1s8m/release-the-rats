using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPipes : MonoBehaviour
{
    public AudioClip[] creaks;

    private float timePassed = 0f;
    private float waitTime;
    private int index = -1;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        Creak();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > waitTime) {
            Creak();
        }
    }

    private void Creak() {
        timePassed = 0f;
        waitTime = Random.Range(0f, 7f) * 100f;
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
