using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanSound : MonoBehaviour
{
    public float adjustment = 0f;
    
    public Transform player;

    private float curDist;
    private float startingDist;
    private float pan;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        startingDist = Dist();
    }

    // Update is called once per frame
    void Update()
    {
        curDist = Dist();
        pan = Mathf.Abs(curDist / startingDist);
        // if player is on the right
        if (curDist < 0) {
            pan *= -1;
        }
        audio.panStereo = pan - adjustment;
    }

    private float Dist() {
        return transform.position.x - player.position.x;
    }
}
