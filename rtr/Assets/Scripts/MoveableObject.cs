using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public float adjust = .1f;

    public Transform player;
    public AudioClip draggingSound;

    private bool held = false;
    private float timePlay = 0f;

    private AudioSource audio;
    private Rigidbody2D rb;
    private Transform home;
    private Vector3 ogPos;
    private Quaternion ogRot;
    private Vector3 zeroVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
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
            }
            
            // dragging sound logic
            timePlay += Time.deltaTime;
            if ((Mathf.Abs(player.gameObject.GetComponent<Rigidbody2D>().velocity.x) > .1f && Mathf.Abs(draggingSound.length - timePlay) < adjust)) audio.PlayOneShot(draggingSound);
            if (Mathf.Abs(draggingSound.length - timePlay) < adjust) timePlay = 0f;
        }
    }

    public void Reset() {
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 100f;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Hold() {
        this.gameObject.transform.parent = player;
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
}
