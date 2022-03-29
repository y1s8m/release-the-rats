using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        UIManager.instance.BrighterAnim();
        MyVideoPlayer.enabled = false;
        if (CameraMovement.instance) CameraMovement.instance.StartCutscene();
    }
}