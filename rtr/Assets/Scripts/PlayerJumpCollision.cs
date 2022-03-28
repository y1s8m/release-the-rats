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
        Debug.Log("mood swings");
        if (collision.gameObject.tag == "Ground") {
            Debug.Log("just us two");
            PlayerController.instance.EnterGroundFeetCollision();
        }
    }
}
