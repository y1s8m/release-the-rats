using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedRatController : MonoBehaviour
{
    private Animator anim;
    public Animator witchAnim;
    private int witchHP = 30;

    private bool gameOver = false;

    public AudioClip[] punchSounds;
    private AudioSource audio;
    private int index = -1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Attacking", true);
            witchAnim.SetBool("playerPunching", true);
            witchHP--;
            if (witchHP <= 0)
            {
                anim.SetBool("FinalHit", true);
                witchAnim.SetBool("isDead", true);
                StartCoroutine(GameOver());
            }
        } else {
            witchAnim.SetBool("playerPunching", false);
        }
    }

    public void SetAttackingFalse()
    {
        anim.SetBool("Attacking", false);
    }

    private IEnumerator GameOver()
    {
        gameOver = true;

        yield return new WaitForSeconds(5);

        // play ending animatic
    }

    public void PlayPunch() {
        int last = index;

        while (index == last) {
            float next = Random.Range(0f, 1f);
            for (int i = 0; i < punchSounds.Length; i++) {
                if (next < ((i + 1f) / punchSounds.Length)) {
                    index = i;
                    break;
                }
            }
        }
        audio.PlayOneShot(punchSounds[index]);
    }
}
