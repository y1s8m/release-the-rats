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
	private bool beatLevel3 = false;

	// animation
	private bool running = false;
	private int numSteps = 0;

	// player movement variables
	public bool dead = false;
	private float airTime = 0f;
	public float bigJump = 1f;
	private bool reset = true;

	private int numGroundObjects = 0;

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

	// grabbing items
	private bool grabbing = false;
	private bool touchingMoveable = false;
	private bool scaleChanged = false;

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
		audio = GetComponent<AudioSource>();
	}

    private void Start()
	{
		transform.position = startPos.position;
		playerRigidbody.gravityScale = gravityScale;
		playerRigidbody.constraints = RigidbodyConstraints2D.None;

		cutScene = true;
	}

    private async void Update()
    {
		if (!scaleChanged && SceneManager.GetActiveScene().buildIndex == 5) {
			transform.localScale = new Vector3(.8f, .8f, .8f);
			scaleChanged = true;
		}

		if (!isPaused){
			if (cutScene || dead) return;

			if (running) ratSprite.GetComponent<SpriteRenderer>().flipX = false;
			else ratSprite.GetComponent<SpriteRenderer>().flipX = true;

			horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
			
			if (Input.GetButtonDown("Jump") && !jumped && !grabbing) jumped = true;
			else if (Input.GetButtonDown("Jump")) ratSprite.GetComponent<AudioSource>().PlayOneShot(denied);

			if (Input.GetKeyDown("q")) grabbing = !grabbing;
			if (touchingMoveable || SceneManager.GetActiveScene().buildIndex == 1) {
				grabbing = false;
			}

			if (!onGround && !onPipe) airTime += Time.deltaTime;
			else airTime = 0f;

			if (grabbing) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

    private void FixedUpdate()
	{
		if (!isPaused){
			if (cutScene || dead) return;
			Move(horizontalMove * Time.fixedDeltaTime, jumped);
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

		
		if (SceneManager.GetActiveScene().buildIndex != 5 && numGroundObjects > 0) {
			float angle = transform.rotation.z;
			if (targetVelocity.x != 0) {
				if (targetVelocity.x > 0) targetVelocity = new Vector2((Mathf.Cos(angle * targetVelocity.x) - Mathf.Sin(angle * targetVelocity.y)) * 10f, 0f);
				else targetVelocity = new Vector2((Mathf.Cos(angle * targetVelocity.x) - Mathf.Sin(angle * targetVelocity.y)) * 10f * (-1f), 0f);
			}
		}

		if (SceneManager.GetActiveScene().buildIndex != 5 && onGround) targetVelocity = new Vector2(targetVelocity[0], 0f);

		// And then smoothing it out and applying it to the character
		playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, targetVelocity, ref zeroVel, moveSmoothing);
		if (!grabbing && Mathf.Abs(playerRigidbody.velocity.x) > 1f) {
			playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidbody.velocity.x));
			running = true;
		} else if (Mathf.Abs(playerRigidbody.velocity.x) > 5f) {
			playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidbody.velocity.x));
			running = true;
		} else if (!grabbing && Mathf.Abs(playerRigidbody.velocity.y) > 1f) {
			playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidbody.velocity.y));
			running = true;
		} else if (Mathf.Abs(playerRigidbody.velocity.y) > 5f) {
			playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidbody.velocity.y));
			running = true;
		} else {
			playerAnimator.SetFloat("speed", 0f);
			running = false;
		}
		//if (true) playerAnimator.SetFloat("speed", diff);
		/*else {
			if (lookingRight) playerAnimator.SetFloat("speed", playerRigidbody.velocity.x);
			else playerAnimator.SetFloat("speed", -playerRigidbody.velocity.x);
		}*/
		
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
		if (!dead)
		{
			dead = true;

			playerRigidbody.velocity = Vector2.zero;
			playerRigidbody.gravityScale = gravityScale;
			ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
			if (UIManager.instance) UIManager.instance.Die();

			yield return new WaitForSeconds(2f);

			if (SceneManager.GetActiveScene().buildIndex == 1)
			{
				UIManager.instance.DisableDeadPanel();
				Reset();
			}
			else
			{
				GameManager.instance.LoadThisLevel();
			}
		}
    }

	public void ProjectileHit()
    {
		if (!beatLevel3) StartCoroutine(Die());
    }

	public void FallInCauldron()
    {
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.gravityScale = 0;
		UIManager.instance.DarkerAnim();
	}

	public void EnterGroundBodyCollision() {
		EnterGroundCollision();
	}

	public void EnterGroundFeetCollision() {
		EnterGroundCollision();
		numGroundObjects++;
		onGround = true;
		canJump = true;
	}

	private void EnterGroundCollision()
	{
		if (!reset && !canJump && airTime > bigJump) {
			ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
		} else if (!reset && !canJump && !grabbing) LandStep();
		reset = false;
		
		if (!onPipe && !grabbing)
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
		numGroundObjects--;
		if (numGroundObjects <= 0) {
			onGround = false;
			canJump = false;
		}
	}

	public void EnterMoveableObjectCollision() {
		grabbing = false;
		numGroundObjects++;
		canJump = true;
	}

	public void ExitMoveableObjectCollision() {
		numGroundObjects--;
		canJump = false;
	}

	public void EnterPipeCollision(Vector3 contactPos, Vector3 normal)
    {
		if (onPipe) return;
		if (!canJump && airTime > bigJump) {
			ratSprite.GetComponent<AudioSource>().PlayOneShot(sewerLand);
			airTime = 0f;
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
		transform.position = contactPos + normal * 0.6f; //verticalScale;

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
		canJump = true;
	}

	public void ExitPipeCollision()
    {
		onPipe = false;
		canJump = false;

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
		if (level == 1 && Mathf.Abs(horizontalMove) > .1f && (onGround || onPipe)) {
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
			numSteps++;
			StartCoroutine(StepFade(ratSteps[index].length));
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
			StartCoroutine(StepFade(ratSteps[index].length));
		}
	}

	public void HoldMoveable() {
		touchingMoveable = true;
	}

	public void ReleaseMoveable() {
		touchingMoveable = false;
	}

	public void SetJump() {
		canJump = true;
		onGround = true;
	}

	public void StandUpright() {
		transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	private IEnumerator StepFade(float length) {
		yield return new WaitForSeconds(length);
		numSteps--;
	}

	public void Level3End() {
		beatLevel3 = true;
	}
}
