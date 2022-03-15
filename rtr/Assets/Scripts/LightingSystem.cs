using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightingSystem : MonoBehaviour
{
    public bool flicker = true;
    public float differenceCutoff = .04f;
    public float flickerRate = 2f;
    public float t = .1f;

    public Light2D light;

    private float timePassed = 0f;
    private float waitTime;
    
    // Start is called before the first frame update
    void Start()
    {
        waitTime = flickerRate;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Time Passed: " + timePassed.ToString() + " Wait Time: " + waitTime.ToString());
        if (flicker && timePassed > waitTime) {
            timePassed = 0f;
            Flicker();
            waitTime = Random.Range(flickerRate / 2, flickerRate);
        }
        timePassed += Time.deltaTime;
    }

    private void Flicker() {
        light.intensity = Mathf.Lerp(light.intensity, Mathf.Clamp(Random.Range(light.intensity - differenceCutoff, light.intensity + differenceCutoff), .5f, 1f), t);
    }
}
