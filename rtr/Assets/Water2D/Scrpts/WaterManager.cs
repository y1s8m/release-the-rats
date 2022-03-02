using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water2D;

public class WaterManager : MonoBehaviour
{
    public bool flashing = false;
    public float timeFlowing;
    public float timeNotFlowing;
    
    public GameObject waterSpawner;

    private bool flowing = true;
    private bool playing = true;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        flowing = true;
        timePassed = 0f;
        waterSpawner.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (playing && flashing) {
            timePassed += Time.fixedDeltaTime;
            if (flowing && timePassed >= timeFlowing) {
                flowing = false;
                timePassed = 0f;
                waterSpawner.SetActive(false);
            } else if (!flowing && timePassed >= timeNotFlowing) {
                flowing = true;
                timePassed = 0f;
                waterSpawner.SetActive(true);
                Water2D_Spawner.instance.RestartWaterSpawning();
            }
        }
    }
}
