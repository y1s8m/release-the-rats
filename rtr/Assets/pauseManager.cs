using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                pausePanel.SetActive(true);
                isPaused = false;

            }
            else {
                pausePanel.SetActive(false);
                isPaused = true;
            }
        }
    }
}
