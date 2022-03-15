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
            Debug.Log(chance);
            if (chance > flickerThreshold) {
                timePassed = 0f;
                flickering = true;
                light.enabled = false;
            }
        } else {
            // determine if light should be back on
            if (timePassed > flickerLength) {
                flickering = false;
                light.enabled = true;
            } else {
                timePassed += Time.deltaTime;
            }
        }
    }

    private void Flicker() {
        
    }
}
