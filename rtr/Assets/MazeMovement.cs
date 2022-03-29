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

    bool deciding = false;

    public bool cutScene = false;

    public bool isChild;

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

            playerRigidbody.velocity = new Vector3(-1000f * Time.deltaTime,0f * Time.deltaTime);
        }
        else{
            if (deciding){
                if (validUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))){
                    playerRigidbody.velocity = new Vector3(0f * Time.deltaTime,1000f * Time.deltaTime);
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    deciding = false;
                }
                if (validDown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))){
                    playerRigidbody.velocity = new Vector3(0f * Time.deltaTime,-1000f * Time.deltaTime);
                    this.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    deciding = false;
                }
                if (validRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))){
                    playerRigidbody.velocity = new Vector3(1000f * Time.deltaTime,0f * Time.deltaTime);
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    deciding = false;
                }
                if (validLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))){
                    playerRigidbody.velocity = new Vector3(-1000f * Time.deltaTime,0f * Time.deltaTime);
                    this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    deciding = false;
                }
            }
        }
    }

    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "move"){
            playerRigidbody.velocity = Vector3.zero;
            //transform.position = col.gameObject.transform.position;
        } else if (col.gameObject.tag == "NextLevelPipe") GameManager.instance.LoadNextLevel();
        else if (col.gameObject.tag == "CurveTrigger"){
            playerRigidbody.velocity = Vector3.zero;
            if (isChild){
                if(col.gameObject.GetComponent<MazeCurve>().pcollided){
                    //fix direction
                }
                else{
                    playerAnimator.SetBool("D2UTurn", true);
                }
            }
            else{
                if(col.gameObject.GetComponent<MazeCurveParent>().chcollided){
                    //fix direction
                }
                else{
                    playerAnimator.SetBool("D2UTurn", true);
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
