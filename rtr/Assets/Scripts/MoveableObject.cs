using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Transform player;

    private bool held = false;

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
            }
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
        if (collision.gameObject.tag == "DeadZone") {
            Hold();
            transform.position = ogPos;
            transform.rotation = ogRot;
            Reset();
        }
    }
}
