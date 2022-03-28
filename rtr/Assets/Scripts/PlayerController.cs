using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public AudioClip sewerLand;
	public AudioClip denied;
	private AudioSource audio;
	
	private int index = 0;
	private int level = 1;

	// animation
	private bool running = false;

	// player movement variables
	[Range(0, 1)]
	public float hp = 1f;
	private float maxHP = 1f;
	public bool dead = false;
	public VolumeProfile volume;
	private Vignette vignette;
	private float airTime = 0f;
	public float bigJump = 1f;
	private bool reset = true;

	private float speed = 50f;
	private float origSpeed = 50f;
	private float moveSmoothing = 0.05f;
	public float jumpForce = 4000f;

	private float horizontalMove = 0.0f;

	private Vector3 zeroVel = Vector3.zero;
	private Vector3 normal = new Vector3(0, 1, 0);

	private bool jumped = false;
	private bool onGround = false;
	public bool onPipe = false;
	public bool canJump = false;
	private bool lookingRight = true;
	//private bool piping = false;

	// slippery variables
	private bool onSlippery = false;
	private float slipperyTimer = 0f;
	private float maxSlipperyTime = 2f;
	private bool slipping = false;

	// grabbing items
	private bool grabbing = false;

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
		playerRigidbody.constraints = RigidbodyConstraints2D.None;

		cutScene = true;

		volume.TryGet(out vignette);
	}

    private async void Update()
    {
		vignette.intensity.Override(1f - hp);

		if (!isPaused){
			if (cutScene || dead) return;

			if (running) ratSprite.GetComponent<SpriteRenderer>().flipX = false;
			else ratSprite.GetComponent<SpriteRenderer>().flipX = true;

			horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
			
			if (Input.GetButtonDown("Jump") && !jumped && !grabbing) jumped = true;
			else if (Input.GetButtonDown("Jump")) ratSprite.GetComponent<AudioSource>().PlayOneShot(denied);

			if (Input.GetKeyDown("q")) grabbing = !grabbing;

			if (!onGround) airTime += Time.deltaTime;
			else airTime = 0f;
		}
	}

    private void FixedUpdate()
	{
		if (!isPaused){
			if (cutScene || dead) return;
			Move(horizontalMove * Time.fixedDeltaTime, jumped);
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

	private void Move(float horizMove, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity;

		if (onPipe)
		{
			targetVelocity = new Vector2(0, (lookingRight ? 1f : -1f) * horizMove * 10f);

			jumped = false;
			canJump = true;
		} else
        {
			targetVelocity = new Vector2(horizMove * 10f, playerRigidbody.velocity.y);
		}

		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);
		float diff = Mathf.Abs(playerRigidbody.velocity.x) + Mathf.Abs(playerRigidbody.velocity.y);
		running = diff > .1f;
		if (!grabbing) playerAnimator.SetFloat("speed", diff);
		else {
			if (lookingRight) playerAnimator.SetFloat("speed", playerRigidbody.velocity.x);
			else playerAnimator.SetFloat("speed", -playerRigidbody.velocity.x);
		}
		
		// If the input is moving the player right and the player is facing left...
		if (!grabbing && horizMove > 0 && !lookingRight && !onPipe)
		{
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (!grabbing && horizMove < 0 && lookingRight && !onPipe)
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

			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
			Vector3 jumpDir = (normal + new Vector3(0, 1, 0)) / 2f;
			playerRigidbody.AddForce(jumpDir * jumpForce);
		}

		if (grabbing) {
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
		reset = true;

		this.normal = new Vector3(0, 1, 0);
		transform.rotation = Quaternion.Euler(Vector3.zero);

		// reset max HP
		UpdateHP(maxHP);

		onGround = false;
		jumped = false;
		canJump = false;

		lookingRight = true;
		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		transform.position = startPos.position;
		playerRigidbody.velocity = Vector2.zero;
    }

    public IEnumerator Die()
    {
		dead = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = gravityScale;
		ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
		UIManager.instance.Die();

		yield return new WaitForSeconds(2f);

		if (SceneManager.GetActiveScene().buildIndex == 1)
		{
			UIManager.instance.DisableDeadPanel();
			Reset();
        } else
        {
			GameManager.instance.LoadThisLevel();
		}

    }

	public void ProjectileHit()
    {
		StartCoroutine(Die());
    }

	public void EnterGroundBodyCollision() {
		EnterGroundCollision();
	}

	public void EnterGroundFeetCollision() {
		EnterGroundCollision();
		onGround = true;
		canJump = true;
	}

	private void EnterGroundCollision()
	{
		if (!reset && !canJump && airTime > bigJump) {
			ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
		} else if (!reset && !canJump) LandStep();
		reset = false;
		
		if (!onPipe)
		{
			this.normal = new Vector3(0, 1, 0);
			transform.rotation = Quaternion.Euler(Vector3.zero);
		}
	}

	public void StayGroundCollision() {
		canJump = true;
	}

	public void ExitGroundCollision()
	{
		Debug.Log("super average");
		onGround = false;
		canJump = false;
	}

	public void EnterPipeCollision(Vector3 contactPos, Vector3 normal)
    {
		if (onPipe) return;
		if (!canJump && airTime > bigJump) {
			ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
		}

		this.normal = normal;
		
		// take cross product of contact point's normal and XY plane's normal
		// use Vector3.angle to calculate the angle from this slope to forward
		Vector3 slope = Vector3.Cross(normal, new Vector3(0, 0, 1));
		float zRotAngle = Vector3.Angle(slope, transform.forward) % 90;

		if (lookingRight && normal == new Vector3(-1, 0, 0)) zRotAngle = 90;
		if (!lookingRight && normal == new Vector3(1, 0, 0)) zRotAngle = -90;

		// set the rotation and position offset
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRotAngle));
		transform.position = contactPos + normal * 0.8f; //verticalScale;

		onPipe = true;
		jumped = false;
		canJump = true;

		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = 0f;

		speed = origSpeed / 2;
	}

	public void StayPipeCollision()
    {
		playerRigidbody.gravityScale = 0f;
		onPipe = true;
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

	public void PlayFootstep() {
		// footsteps
		if (level == 1 && Mathf.Abs(horizontalMove) > .1f && onGround) {
			int last = index;

			while (index == last) {
				float next = Random.Range(0f, 1f);
				for (int i = 0; i < ratSteps.Length; i++) {
					if (next < ((i + 1f) / ratSteps.Length)) {
						index = i;
						break;
					}
				}
			}
			audio.PlayOneShot(ratSteps[index]);
		}
	}

	private void LandStep() {
		int last = index;
		while (index == last) {
			float next = Random.Range(0f, 1f);
			for (int i = 0; i < ratSteps.Length; i++) {
				if (next < ((i + 1f) / ratSteps.Length)) {
					index = i;
					break;
				}
			}
			audio.PlayOneShot(ratSteps[index]);
		}
	}
}
