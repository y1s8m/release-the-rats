using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

	// audio clips
	public AudioClip[] ratSteps;
	public AudioClip sewerJump;
	public AudioClip sewerLand;
	private AudioSource audio;
	
	private bool playingSound = false;
	private float stepTimePassed = 0F;
	private int index = 0;
	private int level = 1;
	private int counter = 0;

	// player movement variables
	[Range(0, 1)]
	public float hp = 1f;
	private float maxHP = 1f;
	public bool dead = false;
	private bool touchingWater = false;
	public VolumeProfile volume;
	private Vignette vignette;
	private float airTime = 0f;
	private bool trackAirTime = false;
	public float bigJump = 1f;

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
	private bool piping = false;

	// slippery variables
	private bool onSlippery = false;
	private float slipperyTimer = 0f;
	private float maxSlipperyTime = 2f;
	private bool slipping = false;

	// grabbing items
	private bool grabbing = false;

	// water-related variables
	public float damageRate;

	private float damageTimer = 0f;
	private int numWaterDrops = 0;

	public Transform startPos;

	[SerializeField] private GameObject ratSprite;
	public GameObject backCollision;

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
		audio = GetComponent<AudioSource>();
	}

    private void Start()
	{
		transform.position = startPos.position;
		playerRigidbody.gravityScale = gravityScale;

		cutScene = true;

		volume.TryGet(out vignette);
	}

    private async void Update()
    {
		vignette.intensity.Override(1f - hp);

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

			if (Input.GetKeyDown("q")) {
				grabbing = !grabbing;
			}
			if (playingSound) {
			stepTimePassed += Time.deltaTime;
			if (counter % 2 == 0 && stepTimePassed >= ratSteps[index].length / 2f) playingSound = false;
			else if (stepTimePassed >= ratSteps[index].length / 4f) playingSound = false;
			}

			if (trackAirTime) {
				airTime += Time.deltaTime;
				Debug.Log(airTime);
			} else {
				airTime = 0f;
			}
			
			// footsteps
			if (level == 1 && Mathf.Abs(horizontalMove) > .1f && !playingSound) {
				int last = index;

				for (int j = 0; j < 100; j++) {
					float next = Random.Range(0f, 1f);
					for (int i = 0; i < ratSteps.Length; i++) {
						if (next < ((i + 1f) / ratSteps.Length)) {
							index = i;
							break;
						}
					}
					if (next < ((index + 1f) / ratSteps.Length)) {
						break;
					}
				}

				stepTimePassed = 0f;
				audio.PlayOneShot(ratSteps[index]);
				playingSound = true;
				counter++;
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
		playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidbody.velocity.x) + Mathf.Abs(playerRigidbody.velocity.y));
		
		// If the input is moving the player right and the player is facing left...
		if (!grabbing && horizMove > 0 && !lookingRight)
		{
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (!grabbing && horizMove < 0 && lookingRight)
		{
			Flip();
		}

		// If the player should jump...
		if (!canJump) jumped = false;
		if (canJump && jump)
		{
			trackAirTime = true;
			audio.PlayOneShot(sewerJump);
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
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
		transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

		transform.position = startPos.position;
    }

    public IEnumerator Die()
    {
		dead = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = gravityScale;

		yield return new WaitForSeconds(1f);

		Reset();
    }

	public void EnterGroundCollision()
	{
		if (!canJump && airTime > bigJump) {
			audio.PlayOneShot(sewerLand);
		}
		
		if (!onPipe)
		{
			this.normal = new Vector3(0, 1, 0);
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		onGround = true;
		canJump = true;
		trackAirTime = false;
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
		if (!canJump && airTime > bigJump) {
			audio.PlayOneShot(sewerLand);
		}
		
		playerRigidbody.constraints = RigidbodyConstraints2D.None;

		this.normal = normal;
		
		// take cross product of contact point's normal and XY plane's normal
		// use Vector3.angle to calculate the angle from this slope to forward
		Vector3 slope = Vector3.Cross(normal, new Vector3(0, 0, 1));
		float zRotAngle = Vector3.Angle(slope, transform.forward) % 90;

		//if (normal.x < 0 && !lookingRight) zRotAngle *= -1;
		/*if (normal.x > 0 && zRotAngle >= 90) zRotAngle = -zRotAngle;
		else if (zRotAngle > 90 || zRotAngle < -90) zRotAngle = zRotAngle % 90;*/
		/*if (normal.x < 0 && zRotAngle < 0) zRotAngle = zRotAngle % 90;
		else if (normal.x > 0 && zRotAngle >= 90) zRotAngle = -zRotAngle;*/

		if (lookingRight && normal == new Vector3(-1, 0, 0)) zRotAngle = 90;
		if (!lookingRight && normal == new Vector3(1, 0, 0)) zRotAngle = -90;

		// set the rotation and position offset
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotAngle));
		transform.position = contactPos + normal * 0.8f; //verticalScale;

		onPipe = true;
		jumped = false;
		canJump = true;
		trackAirTime = false;

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

	public bool GetGrabbing() {
		return grabbing;
	}

	public void FlipToNorm() {
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
