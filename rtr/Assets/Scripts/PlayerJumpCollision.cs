using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpCollision : MonoBehaviour
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
        if (collision.gameObject.tag == "Ground") {
            PlayerController.instance.EnterGroundFeetCollision();
        } else if (collision.gameObject.tag == "MoveableObject") {
            PlayerController.instance.EnterMoveableObjectCollision();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            PlayerController.instance.ExitGroundCollision();
        } else if (collision.gameObject.tag == "MoveableObject") {
            PlayerController.instance.ExitMoveableObjectCollision();
        }
    }
}