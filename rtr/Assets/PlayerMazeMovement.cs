using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMazeMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playerAnimator;
    string inputMove;

    bool validUp = true;
    bool validDown = true;
    bool validLeft = true;
    bool validRight = true;

    bool deciding = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        rb.velocity = new Vector3(-1000f * Time.deltaTime,0f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (deciding){
            if (validUp && (Input.GetKeyDown(KeyCode.UpArrow))){
                rb.velocity = new Vector3(0f * Time.deltaTime,1000f * Time.deltaTime);
                deciding = false;
            }
            if (validDown && (Input.GetKeyDown(KeyCode.DownArrow))){
                rb.velocity = new Vector3(0f * Time.deltaTime,-1000f * Time.deltaTime);
                deciding = false;
            }
            if ((validRight && Input.GetKeyDown(KeyCode.RightArrow))){
                rb.velocity = new Vector3(1000f * Time.deltaTime,0f * Time.deltaTime);
                deciding = false;
            }
            if ((validLeft && Input.GetKeyDown(KeyCode.LeftArrow))){
                rb.velocity = new Vector3(-1000f * Time.deltaTime,0f * Time.deltaTime);
                deciding = false;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "move"){
            rb.velocity = Vector3.zero;
            validUp = col.gameObject.GetComponent<ValidMoves>().GetValidUp();
            validDown = col.gameObject.GetComponent<ValidMoves>().GetValidDown();
            validLeft = col.gameObject.GetComponent<ValidMoves>().GetValidLeft();
            validRight = col.gameObject.GetComponent<ValidMoves>().GetValidRight();
            deciding = true;
        }
    }
}