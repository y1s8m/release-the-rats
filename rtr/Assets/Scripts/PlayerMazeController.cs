using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMazeController : MonoBehaviour
{
    public static PlayerMazeController instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerMazeController>();
            }

            return m_instance;
        }
    }

    private static PlayerMazeController m_instance;

    public bool cutScene = false;

    private float speed = 50f;
    private float origSpeed = 50f;
    private float moveSmoothing = 0.05f;

    private float horizontalMove = 0.0f;
    private float verticalMove = 0.0f;

    private Vector3 zeroVel = Vector3.zero;

    public Transform startPos;

    [SerializeField] private GameObject ratSprite;

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = startPos.position;

        cutScene = true;
    }

    private void Update()
    {
        if (cutScene) return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        verticalMove = Input.GetAxisRaw("Vertical") * speed;
    }

    private void FixedUpdate()
    {
        Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
    }

    private void Move(float horizMove, float vertMove)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizMove * 10f, vertMove * 10f);

        // And then smoothing it out and applying it to the character
        playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);

        // change rotation
        if (horizMove != 0 || vertMove != 0) transform.right = new Vector3(horizMove * 10f, vertMove * 10f, 0);
    }
}
