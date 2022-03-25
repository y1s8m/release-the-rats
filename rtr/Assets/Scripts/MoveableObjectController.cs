using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "MoveableObject" && PlayerController.instance.GetGrabbing()) {
            collision.gameObject.GetComponent<MoveableObject>().Hold();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "MoveableObject" && PlayerController.instance.GetGrabbing()) {
            collision.gameObject.GetComponent<MoveableObject>().Hold();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "MoveableObject") {
            collision.gameObject.GetComponent<MoveableObject>().Reset();
        }
    }
}