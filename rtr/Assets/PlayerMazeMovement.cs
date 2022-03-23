using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMazeMovement : MonoBehaviour
{
    public static PlayerMazeMovement S;

    private Rigidbody2D rb;
    private Animator playerAnimator;
    string inputMove;

    bool validUp = true;
    bool validDown = true;
    bool validLeft = true;
    bool validRight = true;

    bool deciding = false;
    
    void Awake() {
        if (PlayerMazeMovement.S) {
            Destroy(this.gameObject);
        } else {
            S = this;
        }
    }

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
            if (validUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))){
                rb.velocity = new Vector3(0f * Time.deltaTime,1000f * Time.deltaTime);
                this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                deciding = false;
            }
            if (validDown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))){
                rb.velocity = new Vector3(0f * Time.deltaTime,-1000f * Time.deltaTime);
                this.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                deciding = false;
            }
            if (validRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))){
                rb.velocity = new Vector3(1000f * Time.deltaTime,0f * Time.deltaTime);
                this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                deciding = false;
            }
            if (validLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))){
                rb.velocity = new Vector3(-1000f * Time.deltaTime,0f * Time.deltaTime);
                this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                deciding = false;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "move"){
            rb.velocity = Vector3.zero;
        } else if (col.gameObject.tag == "NextLevelPipe") GameManager.instance.LoadNextLevel();
    }

    public void SetUp(bool v) {
        validUp = v;
    }
    public void SetDown(bool v) {
        validDown = v;
    }
    public void SetLeft(bool v) {
        validLeft = v;
    }
    public void SetRight(bool v) {
        validRight = v;
        deciding = true;
    }
}