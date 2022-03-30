using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public float adjust = .1f;

    public Transform player;
    public AudioClip draggingSound;
    public AudioSource firstAudio;
    public AudioSource secondAudio;

    private bool held = false;
    private bool playing = false;
    private bool wentToCorrect = false;

    private bool first = false;
    private bool firstPlaying = false;
    private float firstTime = 0f;
    private bool second = false;
    private bool secondPlaying = false;
    private float secondTime = 0f;

    private Rigidbody2D rb;
    private Transform home;
    private Vector3 ogPos;
    private Quaternion ogRot;
    private Vector3 zeroVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 100f;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
        home = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (held) {
            if (!PlayerController.instance.GetGrabbing()) {
                this.gameObject.transform.parent = home;
                held = false;
                Reset();
            } else {
                this.gameObject.transform.parent = player;
                Hold();
                if (!wentToCorrect) GoToCorrectPosition();
            }

            // dragging sound logic
            if (firstPlaying) firstTime += Time.deltaTime;
            if (secondPlaying) secondTime += Time.deltaTime;
            if (Mathf.Abs(player.gameObject.GetComponent<Rigidbody2D>().velocity.x) > .1f) {
                if (!firstPlaying) {
                    firstPlaying = true;
                    firstAudio.Play();
                } else if (firstTime > draggingSound.length - adjust && !secondPlaying) {
                    secondPlaying = true;
                    secondAudio.Play();
                }
            } else {
                firstAudio.Stop();
                secondAudio.Stop();
            }
            if (firstPlaying && adjust + firstTime >= draggingSound.length) {
                firstPlaying = false;
                firstTime = 0f;
            }
            if (secondPlaying && adjust + secondTime >= draggingSound.length) {
                secondPlaying = false;
                secondTime = 0f;
            }
        } else {
            firstAudio.Stop();
            secondAudio.Stop();
        }
    }

    public void Reset() {
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 100f;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - .1f, transform.localPosition.z);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            wentToCorrect = false;
        }
    }

    public void Hold() {
        this.gameObject.transform.parent = player;
        if (!wentToCorrect) GoToCorrectPosition();
        held = true;
        if (rb != null) {
            Destroy(rb);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Cauldron")
        {
            MutationManager.instance.PotionCountDec();
            Destroy(this.gameObject);
        } 
        else if (collision.gameObject.tag == "DeadZone") {
            Hold();
            transform.position = ogPos;
            transform.rotation = ogRot;
            Reset();
        }
    }

    private void GoToCorrectPosition() {
        wentToCorrect = true;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + .1f, transform.localPosition.z);
    }
}
