using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<PlayerController>();
			}

			return m_instance;
		}
	}

	private static PlayerController m_instance;

	// player movement variables
	public float stamina = 1f;
	private float maxStamina = 1f;
	private bool outOfStamina = false;
	private float wallStamCost = 0.005f;
	private float jumpStamCost = 0.4f;

	private float speed = 50f;
	private float origSpeed = 50f;
	private float moveSmoothing = 0.05f;
	private float jumpForce = 500f;

	private float horizontalMove = 0.0f;
	private float verticalMove = 0.0f;

	private Vector3 zeroVel = Vector3.zero;

	private bool jumped = false;
	private bool onGround = false;
	private bool canJump = false;
	private bool lookingRight = true;

	private bool onWall = false;

	[SerializeField] private StaminaBar staminaBar;

	private float gravityScale = 1f;
	private Rigidbody2D playerRigidbody;
	private Animator playerAnimator;
	private SpriteRenderer playerRenderer;

    private void Awake()
    {
		if (instance != this)
		{
			Destroy(gameObject);
		}

		playerRigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		playerRenderer = GetComponent<SpriteRenderer>();
	}

    private void Start()
	{
		playerRigidbody.gravityScale = gravityScale;
	}

    private void Update()
    {
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
		verticalMove = Input.GetAxisRaw("Vertical") * speed;

		if (Input.GetButtonDown("Jump") && !jumped && !outOfStamina)
		{
			jumped = true;

			//playerAnimator.SetBool("isOnGround", false);
			//AudioManager.instance.PlayOneShot(jumpSound);
		}
		if (Input.GetKeyDown(KeyCode.Q) && !onWall && !outOfStamina)
        {
			onWall = true;

			playerRigidbody.velocity = Vector2.zero;
			playerRigidbody.gravityScale = 0f;

			speed = origSpeed / 2;
		}
		if (Input.GetKeyDown(KeyCode.E) && onWall)
		{
			onWall = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

			speed = origSpeed;
		}
	}

    private void FixedUpdate()
	{
		Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, false, jumped);
	}

	private void UpdateStamina(float c)
    {
		stamina = c;
		if (stamina < 0)
        {
			stamina = 0;
			outOfStamina = true;

			onWall = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

			speed = origSpeed;
		}
		staminaBar.SetSize(stamina);
    }


	private void Move(float horizMove, float vertMove, bool crouch, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity;
		
		if (onWall)
		{
			targetVelocity = new Vector2(horizMove * 10f, vertMove * 10f);

			jumped = false;
			canJump = true;

			UpdateStamina(stamina - wallStamCost);
			if (horizMove > 0 || vertMove > 0) UpdateStamina(stamina - wallStamCost);
		} else
        {
			targetVelocity = new Vector2(horizMove * 10f, playerRigidbody.velocity.y);
		}

		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);

		// If the input is moving the player right and the player is facing left...
		if (horizMove > 0 && !lookingRight)
		{
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (horizMove < 0 && lookingRight)
		{
			Flip();
		}

		// If the player should jump...
		if (canJump && jump)
		{
			// Add a vertical force to the player.
			jumped = false;
			onWall = false;
			onGround = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

			UpdateStamina(stamina - jumpStamCost);

			playerRigidbody.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		lookingRight = !lookingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		// "Ground" tagged objects are level grounds, platforms, and pipes
		if (collision.gameObject.tag == "Ground")
		{
			onGround = true;
			canJump = true;
			onWall = false;
			outOfStamina = false;

			// reset max stamina
			UpdateStamina(maxStamina);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		// "Slippery" tagged regions are not meant to be traversed via wall
        if (collision.gameObject.tag == "Slippery")
        {
			if (onWall) wallStamCost *= 2;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		// "Slippery" tagged regions are not meant to be traversed via wall
		if (collision.gameObject.tag == "Slippery")
		{
			if (onWall) wallStamCost /= 2;
		}
	}
}
