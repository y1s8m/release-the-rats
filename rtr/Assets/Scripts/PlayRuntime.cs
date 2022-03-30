using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
 
public class PlayRuntime : MonoBehaviour
{
    private VideoPlayer MyVideoPlayer;
 
    private void Start()
    {
        MyVideoPlayer = GetComponent<VideoPlayer>();
        // play video player
        MyVideoPlayer.Play();
        StartCoroutine(disableVideo((float)MyVideoPlayer.length));
    }

    private IEnumerator disableVideo(float len)
    {
        yield return new WaitForSeconds(len);

        MyVideoPlayer.enabled = false;
        AmbienceManager.S.StartMusic();
        if (SceneManager.GetActiveScene().buildIndex != 1 &&  InstructionsManager.instance) InstructionsManager.instance.ShowInstructions();
        if (CameraMovement.instance) CameraMovement.instance.StartCutscene();
    }
}