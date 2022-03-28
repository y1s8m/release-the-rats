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
        Debug.Log(MyVideoPlayer.isPlaying);
    }
}