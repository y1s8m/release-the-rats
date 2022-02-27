using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject SettingsPanel;
    public Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        SettingsPanel.SetActive(false);
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void OnMainMenuContinueClick()
    {
         PlayerPrefs.SetFloat("Volume", VolumeSlider.value);
        //load from last save
        StartCoroutine(WaitLoadSceneCoroutine(0.3f, 1));
    }

    public void OnMainMenuStartClick()
    {   
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);
        //reload game
        StartCoroutine(WaitLoadSceneCoroutine(0.3f, 1));
    }

    public void OnMainMenuSettingsClick()
    {
        SettingsPanel.SetActive(true);
    }

    public void OnMainMenuExitClick()
    {
        Application.Quit();

        //autosave routine
    }

    IEnumerator WaitLoadSceneCoroutine(float waitTime, int sceneNum)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneNum);
    }
}
