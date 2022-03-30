using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{

    public static MazeMovement instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MazeMovement>();
            }

            return m_instance;
        }
    }

    private static MazeMovement m_instance;

    [SerializeField] private GameObject ratSprite;

    public Transform startPos;

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    bool validUp = true;
    bool validDown = true;
    bool validLeft = true;
    bool validRight = true;

    public bool deciding = false;

    public bool cutScene = false;

    public bool isChild;

    public float speed = 500f;

    private Vector3 vel = Vector3.zero;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

       
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos.position;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        cutScene = true;
    }

    void Update()
    {
        if ((!cutScene) && (transform.position == startPos.position)) {

            playerRigidbody.velocity = new Vector3(-speed,0f);
        }
        else{
            if (deciding){
                if (validUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))){
                    MoveInDirect(1);
                }
                if (validDown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))){
                    MoveInDirect(2);
                }
                if (validRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))){
                    MoveInDirect(3);
                }
                if (validLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))){
                    MoveInDirect(4);
                }
            }
        }
    }

    private void MoveInDirect(int dir) {
        if (dir == 1) {
            playerRigidbody.velocity = new Vector3(0f, speed);
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            deciding = false;
        }
        else if (dir == 2) {
            playerRigidbody.velocity = new Vector3(0f, -speed);
            this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            deciding = false;
        }
        else if (dir == 3) {
            playerRigidbody.velocity = new Vector3(speed, 0f);
            this.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            deciding = false;
        }
        else if (dir == 4) {
            playerRigidbody.velocity = new Vector3(-speed, 0f);
            this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            deciding = false;
        }
    }

    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "move"){
            playerRigidbody.velocity = Vector3.zero;
        } else if (col.gameObject.tag == "NextLevelPipe") GameManager.instance.LoadNextLevel();
        else if (col.gameObject.tag == "CurveTrigger"){
            playerRigidbody.velocity = Vector3.zero;
            Vector3 target;
            if(col.gameObject.GetComponent<MazeCurve>()){
                if (col.gameObject.transform.parent.GetComponent<MazeCurveParent>().collided){
                    //collided with parent then child
                    target = col.gameObject.GetComponent<MazeCurve>().nextNode.position;
                    if (Mathf.Abs(target.y - transform.position.y) > Mathf.Abs(target.x - transform.position.x)) {
                        if (target.y > transform.position.y) MoveInDirect(1);
                        if (target.y < transform.position.y) MoveInDirect(2);
                    }
                    else {
                        if (target.x > transform.position.x) MoveInDirect(3);
                        if (target.x < transform.position.x) MoveInDirect(4);
                    }
                }
                else{
                    //collided with child first
                    target = col.gameObject.transform.parent.transform.position;
                    transform.up = target - transform.position;
                    playerRigidbody.velocity = transform.up.normalized * speed * Time.deltaTime;
                    /*while (Mathf.Abs(transform.position.x - target.x) > 0.1f
                         && Mathf.Abs(transform.position.y - target.y) > 0.1f) {
                        transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.01f);
                    }*/
                }
            }
            else{
                if (col.gameObject.GetComponent<MazeCurveParent>().chcollided){
                    //collided with child then parent
                    target = col.gameObject.GetComponent<MazeCurveParent>().nextNode.position;
                    if (Mathf.Abs(target.y - transform.position.y) > Mathf.Abs(target.x - transform.position.x)) {
                        if (target.y > transform.position.y) MoveInDirect(1);
                        if (target.y < transform.position.y) MoveInDirect(2);
                    }
                    else {
                        if (target.x > transform.position.x) MoveInDirect(3);
                        if (target.x < transform.position.x) MoveInDirect(4);
                    }
                }
                else{
                    //collided with parent first
                    target = col.gameObject.transform.GetChild(0).gameObject.transform.position;
                    transform.up = target - transform.position;
                    playerRigidbody.velocity = transform.up.normalized * speed * Time.deltaTime;
                    /*while (Mathf.Abs(transform.position.x - target.x) > 0.1f
                         && Mathf.Abs(transform.position.y - target.y) > 0.1f) {
                        transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.01f);
                    }*/
                }
            }
            
        }

    }

    public void SetUp(bool v) {
        validUp = v;
        deciding = true;
    }
    public void SetDown(bool v) {
        validDown = v;
        deciding = true;
    }
    public void SetLeft(bool v) {
        validLeft = v;
        deciding = true;
    }
    public void SetRight(bool v) {
        validRight = v;
        deciding = true;
    }
}
