using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// player variables
	private float stamina = 1f;
	private float maxStamina = 1f;
	private bool outOfStamina = false;

	private float speed = 50f;
	private float moveSmoothing = 0.05f;
	private float jumpForce = 500f;
	private float horizontalMove = 0.0f;

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

	private void Start()
	{
		staminaBar.SetSize(stamina);

		playerRigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		playerRenderer = GetComponent<SpriteRenderer>();

		playerRigidbody.gravityScale = gravityScale;
	}

    private void Update()
    {
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

		if (stamina != 0 && Input.GetButtonDown("Jump") && jumped == false)
		{
			jumped = true;
			onWall = false;

			playerRigidbody.gravityScale = gravityScale;
			//playerAnimator.SetBool("isOnGround", false);
			//AudioManager.instance.PlayOneShot(jumpSound);
		}
		if (stamina != 0 && Input.GetKeyDown(KeyCode.Q) && !onWall)
        {
			onWall = true;
			jumped = false;
			canJump = true;

			playerRigidbody.velocity = Vector2.zero;
			playerRigidbody.gravityScale = 0f;
        }
		if (Input.GetKeyDown(KeyCode.E) && onWall)
		{
			onWall = false;

			playerRigidbody.gravityScale = gravityScale;
		}
	}

    private void FixedUpdate()
	{
		if (outOfStamina)
        {
			onWall = false;
			playerRigidbody.gravityScale = gravityScale;
        }

		Move(horizontalMove * Time.fixedDeltaTime, false, jumped);
	}

	private void UpdateStamina(float c)
    {
		stamina = c;
		if (stamina < 0)
        {
			stamina = 0;
			outOfStamina = true;
        }
		staminaBar.SetSize(stamina);
    }


	public void Move(float move, bool crouch, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, playerRigidbody.velocity.y);
		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);

		if (onWall && move > 0) UpdateStamina(stamina - 0.01f);

		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !lookingRight)
		{
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && lookingRight)
		{
			Flip();
		}

		// If the player should jump...
		if (canJump && jump)
		{
			// Add a vertical force to the player.
			jumped = false;
			onGround = false;
			canJump = false;

			UpdateStamina(stamina - 0.4f);

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
}
