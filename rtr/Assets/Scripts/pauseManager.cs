using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public bool isPaused;
    public PlayerController player;

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
                pausePanel.SetActive(false);
                isPaused = false;
                player.isPaused = false;
                if (WitchController.instance) WitchController.instance.isPaused = false;
                Time.timeScale = 1f;

            }
            else {
                pausePanel.SetActive(true);
                isPaused = true;
                player.isPaused = true;
                if (WitchController.instance) WitchController.instance.isPaused = true;
                Time.timeScale = 0f;
            }
        }
    }
}
