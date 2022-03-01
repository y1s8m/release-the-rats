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
	private float hp = 1f;
	private float maxHP = 1f;
	private bool dead = false;
	private bool touchingWater = false;

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
	private bool onWall = false;
	private bool canJump = false;
	private bool lookingRight = true;

	// water-related variables
	public float damageRate;

	private float damageTimer = 0f;
	private int numWaterDrops = 0;

	public Transform startPos;

	[SerializeField] private GameObject ratSprite;
	[SerializeField] private PlayerStatBar hpBar;
	[SerializeField] private PlayerStatBar staminaBar;

	private float gravityScale = 1f;
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
		playerRigidbody.gravityScale = gravityScale;
	}

    private void Update()
    {
		if (dead) return;

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
		if (dead) return;

		Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, false, jumped);

		if (numWaterDrops > 2) {
			damageTimer += Time.fixedDeltaTime;
			if (damageTimer >= damageRate) {
				damageTimer = 0f;
				UpdateHP(hp - .03f);
			}
		}
	}

	private void UpdateHP(float c)
	{
		hp = c;
		hp = Mathf.Clamp(hp, 0, 1);
		if (hp == 0)
		{
			if (!dead) StartCoroutine(Die());

			playerRigidbody.gravityScale = gravityScale;

			speed = origSpeed;
		}
		hpBar.SetSize(hp);
	}

	private void UpdateStamina(float c)
    {
		stamina = c;
		stamina = Mathf.Clamp(stamina, 0, 1);
		if (stamina == 0)
		{
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
		if (!canJump) jumped = false;
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
		ratSprite.transform.localScale = new Vector3(-ratSprite.transform.localScale.x, ratSprite.transform.localScale.y, ratSprite.transform.localScale.z);
	}

    private void Reset()
	{
		dead = false;

		// reset max stamina
		UpdateHP(maxHP);
		UpdateStamina(maxStamina);

		onGround = false;
		jumped = false;
		canJump = false;
		onWall = false;

		lookingRight = true;
		ratSprite.transform.localScale = new Vector3(Mathf.Abs(ratSprite.transform.localScale.x), ratSprite.transform.localScale.y, ratSprite.transform.localScale.z);

		transform.position = startPos.position;
    }

    private IEnumerator Die()
    {
		dead = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = gravityScale;

		yield return new WaitForSeconds(5f);

		Reset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (dead) return;

		if (collision.gameObject.tag == "Ground")
		{
			onGround = true;
			canJump = true;
			onWall = false;
			outOfStamina = false;
		}
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
		if (dead) return;

		// "Ground" tagged objects are level grounds, platforms, and pipes
		if (collision.gameObject.tag == "Ground")
		{
			// reset max stats
			// healing at ground while touching water kinda buggy
			if (!touchingWater) UpdateHP(hp + 0.05f);
			UpdateStamina(stamina + 0.05f);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		// "Slippery" tagged regions are not meant to be traversed via wall
        if (collision.gameObject.tag == "Slippery")
        {
			if (onWall) wallStamCost *= 2;
        }
		else if (collision.gameObject.tag == "Checkpoint")
        {
			startPos = collision.gameObject.transform;
		}
		else if (collision.gameObject.tag == "Metaball_liquid")
		{
			//touchingWater = true;
			UpdateHP(hp - 0.03f);
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		// "Slippery" tagged regions are not meant to be traversed via wall
		if (collision.gameObject.tag == "Slippery")
		{
			if (onWall) wallStamCost /= 2;
		}
		else if (collision.gameObject.tag == "Metaball_liquid")
		{
			//touchingWater = false;
		}
	}

	public void AddWaterDrop() {
		numWaterDrops++;
	}

	public void SubtractWaterDrop() {
		numWaterDrops--;
	}
}
