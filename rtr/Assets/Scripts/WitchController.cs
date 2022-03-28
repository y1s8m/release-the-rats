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
	private bool temp;

	public Transform player;
	public Transform projectilePos;
	public GameObject projectilePrefab;
	public float projectileSpeed;

	/*private float nextMoveTime;
	private float currMoveTime;*/
	private float nextProjTime;
	private float currProjTime;

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
			//Debug.Log(temp);
		}
	}

    private void FixedUpdate()
    {
		if (!isPaused)
		{
			if (cutScene) return;
			/*currMoveTime += Time.deltaTime;
			if (currMoveTime >= nextMoveTime)
			{
				currMoveTime = 0;
            }*/

			currProjTime += Time.deltaTime;
			if (currProjTime >= nextProjTime)
            {
				ShootProjectile();

				nextProjTime = Random.Range(4f, 8f);
				currProjTime = 0;
            }
		}
	}

	private void ShootProjectile()
    {
		witchAnimator.SetTrigger("attack");
		temp = true;
	}

	public void CreateProjectile()
    {
		GameObject proj = Instantiate(projectilePrefab, projectilePos.position, Quaternion.identity);

		Vector3 target = (player.position - projectilePos.position).normalized;
		proj.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
		proj.transform.up = player.position - proj.transform.position;

		witchAnimator.ResetTrigger("attack");
		temp = false;
	}
}
