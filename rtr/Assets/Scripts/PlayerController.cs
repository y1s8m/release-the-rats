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

	public bool isPaused;

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
	private Vector3 normal = new Vector3(0, 1, 0);

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

		//cutScene = true;
	}

    private void Update()
    {
		if (!isPaused){
			if (cutScene || dead) return;

			horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
			verticalMove = Input.GetAxisRaw("Vertical") * speed;
			
			if (Input.GetButtonDown("Jump") && !jumped)
			{
				jumped = true;

				//playerAnimator.SetBool("isOnGround", false);
				//AudioManager.instance.PlayOneShot(jumpSound);
			}
		}
	}

    private void FixedUpdate()
	{
		if (!isPaused){
			if (dead) return;

			Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jumped);

			if (numWaterDrops > 2) {
				damageTimer += Time.fixedDeltaTime;
				if (damageTimer >= damageRate) {
					damageTimer = 0f;
					UpdateHP(hp - .1f);
				}
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

	private void Move(float horizMove, float vertMove, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity;

		if (onSlippery && !slipping) slipperyTimer += Time.deltaTime;
		if (slipping && slipperyTimer < 0)
        {
			slipping = false;
			slipperyTimer = 0;
        }
		if (slipping || slipperyTimer > maxSlipperyTime)
		{
			playerRigidbody.gravityScale = gravityScale;
			slipping = true;
			slipperyTimer -= Time.deltaTime;
			return;
		}

		if (onPipe)
		{
			targetVelocity = new Vector2(horizMove * 10f, vertMove * 10f);

			jumped = false;
			canJump = true;
		} else
        {
			targetVelocity = new Vector2(horizMove * 10f, playerRigidbody.velocity.y);
		}

		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);
		//print(playerRigidbody.velocity);
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
			jumped = true;
			onGround = false;
			onPipe = false;
			canJump = false;

			playerRigidbody.gravityScale = gravityScale;

			playerRigidbody.AddForce(normal * jumpForce);
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

		this.normal = new Vector3(0, 1, 0);
		transform.rotation = Quaternion.Euler(Vector3.zero);

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
		if (!onPipe)
		{
			this.normal = new Vector3(0, 1, 0);
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		onGround = true;
		canJump = true;
	}
	
	public void StayGroundCollision()
	{
		// reset max stats
		if (!touchingWater) UpdateHP(hp + 0.05f);
	}

	public void ExitGroundCollision()
	{
		onGround = false;
	}

	public void EnterPipeCollision(Vector3 contactPos, Vector3 normal)
    {
		playerRigidbody.constraints = RigidbodyConstraints2D.None;
		this.normal = normal;
		Vector3 slope = Vector3.Cross(normal, new Vector3(0, 0, 1));
		float zRotAngle = Vector3.Angle(slope, transform.forward) % 90;

		//if (normal.x < 0 && !lookingRight) zRotAngle *= -1;
		/*if (normal.x > 0 && zRotAngle >= 90) zRotAngle = -zRotAngle;
		else if (zRotAngle > 90 || zRotAngle < -90) zRotAngle = zRotAngle % 90;*/
		/*if (normal.x < 0 && zRotAngle < 0) zRotAngle = zRotAngle % 90;
		else if (normal.x > 0 && zRotAngle >= 90) zRotAngle = -zRotAngle;*/

		if (lookingRight && normal == new Vector3(-1, 0, 0)) zRotAngle = 90;
		if (!lookingRight && normal == new Vector3(1, 0, 0)) zRotAngle = -90;

		transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotAngle));
		transform.position = contactPos + normal * 0.8f; //verticalScale;

		/*print(normal);
		print(slope);
		print(zRotAngle);*/

		onPipe = true;
		jumped = false;
		canJump = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = 0f;

		speed = origSpeed / 2;
	}

	public void StayPipeCollision()
    {
		if (!slipping) playerRigidbody.gravityScale = 0f;
	}

	public void ExitPipeCollision()
    {
		onPipe = false;

		playerRigidbody.gravityScale = gravityScale;

		speed = origSpeed;
	}

	public void EnterTriggerCheckpoint(Transform cp)
    {
		startPos = cp;
	}

	public void EnterTriggerSlippery ()
	{
		print("???");
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
