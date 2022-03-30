using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject NewGameBtn;
    public GameObject SettingsBtn;
    public GameObject ExitBtn;
    public Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        SettingsPanel.SetActive(false);
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
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
        NewGameBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        ExitBtn.SetActive(false);

    }

    public void OnMainMenuSettingsExitClick()
    {
        SettingsPanel.SetActive(false);
        NewGameBtn.SetActive(true);
        SettingsBtn.SetActive(true);
        ExitBtn.SetActive(true);
    }

    public void OnMainMenuExitClick()
    {
        Application.Quit();
    }

    IEnumerator WaitLoadSceneCoroutine(float waitTime, int sceneNum)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneNum);
    }
}
