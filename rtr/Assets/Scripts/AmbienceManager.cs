using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbienceManager : MonoBehaviour
{
    public static AmbienceManager S;

    public AudioClip sewerClip;
    public AudioClip kitchenClip;
    public AudioClip potionClip;
    public AudioClip bossClip;

    private AudioSource audio;

    private void Awake() {
        if (AmbienceManager.S) {
            Destroy(this.gameObject);
        } else {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 1) audio.clip = sewerClip;
        else if (SceneManager.GetActiveScene().buildIndex == 3) audio.clip = kitchenClip;
        else if (SceneManager.GetActiveScene().buildIndex == 5) audio.clip = potionClip;
        else if (SceneManager.GetActiveScene().buildIndex == 6) audio.clip = bossClip;

        audio.loop = true;
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 3
            || SceneManager.GetActiveScene().buildIndex == 5) audio.Play();
    }

    public void StartMusic() {
        audio.Play();
    }

    public void EndMusic() {
        StartCoroutine(StopMusic());
    }

    private IEnumerator StopMusic() {
        yield return new WaitForSeconds(2f);

        audio.volume = 1f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .95f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .9f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .85f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .8f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .75f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .7f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .65f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .6f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .55f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .5f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .45f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .4f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .35f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .3f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .35f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .3f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .25f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .2f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .15f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .1f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .05f;
        yield return new WaitForSeconds(.1f);
        audio.volume = .0f;
        audio.loop = false;
        audio.Stop();
    }
}
