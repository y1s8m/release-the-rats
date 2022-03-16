using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Slider VolumeSlider;

    public AudioClip pourSound;
    public AudioClip[] waterSounds;

    private AudioSource audio;

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
        audio = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("musicVolume")){
            Load();
        }
        else{
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }

    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Save();
    }

    private void Load()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
    }

    public void PlayPourSound() {
        audio.PlayOneShot(pourSound);
    }
}
