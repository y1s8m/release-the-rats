using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && PlayerController.instance.GetGrabbing()) {
            // grab object
            this.gameObject.transform.parent = player;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!PlayerController.instance.GetGrabbing()) {
            this.gameObject.transform.parent = null;
        } else {
            this.gameObject.transform.parent = player;
        }
    }
}
