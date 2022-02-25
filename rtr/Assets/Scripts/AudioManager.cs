using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance
    {
        get
        {
            if (a_instance == null)
            {
                a_instance = FindObjectOfType<AudioManager>();
            }

            return a_instance;
        }
    }

    private static AudioManager a_instance;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
