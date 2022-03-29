using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
	public static WitchController instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<WitchController>();
			}

			return m_instance;
		}
	}

	private static WitchController m_instance;

	public bool cutScene = false;

	public bool isPaused;
	private bool shooting;

	public Transform player;
	public Transform projectilePos;
	public GameObject projectilePrefab;
	public float projectileSpeed;

	public float moveSpeed = 1f;

	private float directChangeTime = 2f;
	private float currDirectChangeTime;

	private float nextProjTime;
	private float currProjTime;
	private Vector3 velocity = Vector3.zero;

	private Animator witchAnimator;

	private void Awake()
	{
		if (instance != this)
		{
			Destroy(gameObject);
		}

		witchAnimator = GetComponent<Animator>();
		//audio = GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
	{
		cutScene = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isPaused)
		{
			if (cutScene) return;

			if (!shooting)
            {
				Move();
            }
		}
	}

    private void FixedUpdate()
    {
		if (!isPaused)
		{
			if (cutScene) return;

			currDirectChangeTime += Time.deltaTime;
			currProjTime += Time.deltaTime;
			if (currProjTime >= nextProjTime)
            {
				ShootProjectile();

				nextProjTime = Random.Range(4f, 8f);
				currProjTime = 0;
            }
		}
	}

    private void Move()
    {
		float left = (player.transform.position.x <= transform.position.x) ? -1f : 1f;
		transform.position = Vector3.SmoothDamp(transform.position, transform.position +
								new Vector3(left * moveSpeed, 0, 0), ref velocity, 1f);
		if (currDirectChangeTime >= directChangeTime)
		{
			transform.localScale = new Vector3(-left * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			currDirectChangeTime = 0;
		}
	}

    private void ShootProjectile()
	{
		witchAnimator.SetTrigger("attack");
		shooting = true;
	}

	public void CreateProjectile()
    {
		GameObject proj = Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);

		Vector3 target = (player.position - projectilePos.position).normalized;
		proj.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
		proj.transform.up = player.position - proj.transform.position;

		witchAnimator.ResetTrigger("attack");
		shooting = false;
	}
}
