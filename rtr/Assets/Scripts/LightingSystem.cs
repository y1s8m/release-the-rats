using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightingSystem : MonoBehaviour
{
    public float flickerLength = .5f;
    public float flickerThreshold = .8f;
    
    public Light2D light;

    private bool flickering = false;
    private float timePassed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // chance of flickering if not already flickering
        if (!flickering) {
            // determine chance
            float chance = Random.Range(0f, 1f);
            if (chance > flickerThreshold) {
                timePassed = 0f;
                flickering = true;
                light.intensity = .75f;
            }
        } else {
            // determine if light should be back on
            if (timePassed > flickerLength) {
                flickering = false;
                light.intensity = 1f;
            } else {
                timePassed += Time.deltaTime;
            }
        }
    }

    private void Flicker() {
        
    }
}
