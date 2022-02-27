using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// player variables
	private float stamina = 100f;
	private float maxStamina = 100f;
	private float speed = 10f;
	private float moveSmoothing = 0.05f;
	private float jumpForce = 300f;
	private float horizontalMove = 0.0f;

	private Vector3 zeroVel = Vector3.zero;

	private bool jumped = false;
	private bool onGround = false;
	private bool canJump = false;
	private bool lookingRight = true;

	private Rigidbody2D playerRigidbody;
	private Animator playerAnimator;
	private SpriteRenderer playerRenderer;

	private void Start()
	{
		playerRigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		playerRenderer = GetComponent<SpriteRenderer>();
	}

    private void Update()
    {
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

		if (Input.GetButtonDown("Jump") && jumped == false)
		{
			jumped = true;
			//playerAnimator.SetBool("isOnGround", false);
			//AudioManager.instance.PlayOneShot(jumpSound);
		}
	}

    private void FixedUpdate()
	{
		Move(horizontalMove * Time.fixedDeltaTime, false, jumped);
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, playerRigidbody.velocity.y);
		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);

		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !lookingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && lookingRight)
		{
			// ... flip the player.
			Flip();
		}

		// If the player should jump...
		if (canJump && jump)
		{
			// Add a vertical force to the player.
			jumped = false;
			onGround = false;
			canJump = false;

			stamina -= 50;

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

			// reset max stamina
			stamina = maxStamina;
        }
    }
}
