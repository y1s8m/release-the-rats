using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public float timeFlowing;
    public float timeNotFlowing;
    
    public GameObject waterSpawner;

    private bool flowing = false;
    private bool playing = true;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        waterSpawner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (playing) {
            timePassed += Time.fixedDeltaTime;
            if (flowing && timePassed >= timeFlowing) {
                flowing = false;
                timePassed = 0f;
                waterSpawner.SetActive(false);
            } else if (!flowing && timePassed >= timeNotFlowing) {
                flowing = true;
                timePassed = 0f;
                waterSpawner.SetActive(true);
            }
        }
    }
}
