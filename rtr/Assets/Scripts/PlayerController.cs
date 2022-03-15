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

	public bool cutScene = false;

	// player movement variables
	private float hp = 1f;
	private float maxHP = 1f;
	public bool dead = false;
	private bool touchingWater = false;

	private float speed = 50f;
	private float origSpeed = 50f;
	private float moveSmoothing = 0.05f;
	private float jumpForce = 4000f;

	private float horizontalMove = 0.0f;
	private float verticalMove = 0.0f;

	private Vector3 zeroVel = Vector3.zero;

	private bool jumped = false;
	private bool onGround = false;
	private bool onPipe = false;
	private bool canJump = false;
	private bool lookingRight = true;

	// slippery variables
	private bool onSlippery = false;
	private float slipperyTimer = 0f;
	private float maxSlipperyTime = 2f;
	private bool slipping = false;

	// water-related variables
	public float damageRate;

	private float damageTimer = 0f;
	private int numWaterDrops = 0;

	public Transform startPos;

	[SerializeField] private GameObject ratSprite;

	private float gravityScale = 5f;
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
		playerRigidbody.gravityScale = gravityScale;

		cutScene = true;
	}

    private void Update()
    {
		if (cutScene || dead) return;

		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
		verticalMove = Input.GetAxisRaw("Vertical") * speed;

		if (Input.GetButtonDown("Jump") && !jumped)
		{
			jumped = true;

			//playerAnimator.SetBool("isOnGround", false);
			//AudioManager.instance.PlayOneShot(jumpSound);
		}
		/*if (Input.GetKeyDown(KeyCode.Q) && !onPipe)
        {
			onPipe = true;

			playerRigidbody.velocity = Vector2.zero;
			playerRigidbody.gravityScale = 0f;

			speed = origSpeed / 2;
		}
		if (Input.GetKeyDown(KeyCode.E) && onPipe)
		{
			onPipe = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

			speed = origSpeed;
		}*/
	}

    private void FixedUpdate()
	{
		if (dead) return;

		Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, false, jumped);

		if (numWaterDrops > 2) {
			damageTimer += Time.fixedDeltaTime;
			if (damageTimer >= damageRate) {
				damageTimer = 0f;
				UpdateHP(hp - .1f);
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
	}

	private void Move(float horizMove, float vertMove, bool crouch, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity;

		if (onSlippery && !slipping) slipperyTimer += Time.deltaTime;
		if (slipping || slipperyTimer > maxSlipperyTime) 
		{
			slipping = true;
			slipperyTimer -= Time.deltaTime;
			return;
		}

		if (onGround) targetVelocity = new Vector2(horizMove * 10f, 0);
		else targetVelocity = new Vector2(horizMove * 10f, playerRigidbody.velocity.y);

		/*if (onPipe)
		{
			targetVelocity = new Vector2(horizMove * 10f, vertMove * 10f);

			jumped = false;
			canJump = true;
		} else
        {
			targetVelocity = new Vector2(horizMove * 10f, playerRigidbody.velocity.y);
		}*/

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
			onGround = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

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

		onGround = false;
		jumped = false;
		canJump = false;

		lookingRight = true;
		ratSprite.transform.localScale = new Vector3(-1 * Mathf.Abs(ratSprite.transform.localScale.x), ratSprite.transform.localScale.y, ratSprite.transform.localScale.z);

		transform.position = startPos.position;
    }

    public IEnumerator Die()
    {
		dead = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = gravityScale;

		yield return new WaitForSeconds(5f);

		Reset();
    }

	public void EnterGroundCollision()
    {
		onGround = true;
		canJump = true;
		playerRigidbody.constraints = RigidbodyConstraints2D.None;
	}
	
	public void StayGroundCollision()
    {
		// reset max stats
		if (!touchingWater) UpdateHP(hp + 0.05f);
		playerRigidbody.constraints = RigidbodyConstraints2D.None;
	}

	public void ExitGroundCollision()
	{
		onGround = false;
		playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void EnterTriggerCheckpoint(Transform cp)
    {
		startPos = cp;
	}

	public void EnterTriggerSlippery ()
	{
		onSlippery = true;
		speed = origSpeed * 0.7f;
	}

    public void ExitTriggerSlippery()
	{
		onSlippery = false;
		slipping = false;
		slipperyTimer = 0f;
		speed = origSpeed;
	}

	public void AddWaterDrop() {
		numWaterDrops++;
	}

	public void SubtractWaterDrop() {
		numWaterDrops--;
	}

	public void LoadNextLevel()
    {
		cutScene = true;
    }
}
