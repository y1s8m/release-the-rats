using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Transform player;

    private bool held = false;

    private Rigidbody2D rb;
    private Vector3 ogPos;
    private Quaternion ogRot;
    private Vector3 zeroVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 100f;
        ogPos = gameObject.transform.position;
        ogRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (held) {
            if (!PlayerController.instance.GetGrabbing()) {
                this.gameObject.transform.parent = null;
                held = false;
                Reset();
            } else {
                this.gameObject.transform.parent = player;
                Hold();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && PlayerController.instance.GetGrabbing()) {
            // grab object
            this.gameObject.transform.parent = player;
            held = true;
            Hold();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && PlayerController.instance.GetGrabbing()) {
            // grab object
            this.gameObject.transform.parent = player;
            held = true;
            Hold();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Reset();
        }
    }

    private void Reset() {
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 100f;
        }
    }

    private void Hold() {
        if (rb != null) {
            Destroy(rb);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "DeadZone") {
            Hold();
            transform.position = ogPos;
            transform.rotation = ogRot;
            Reset();
        }
    }
}
