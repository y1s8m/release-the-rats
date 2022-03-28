using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (PlayerController.instance.dead) return;
		if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MoveableObject" || collision.gameObject.tag == "NoWaterGround")
		{
			Debug.Log("out of my skin");
			PlayerController.instance.EnterGroundBodyCollision();
		}
		else if (collision.gameObject.tag == "Pipe")
		{
			PlayerController.instance.EnterPipeCollision(collision.GetContact(0).point, collision.GetContact(0).normal.normalized);
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (PlayerController.instance.dead) return;
		
		if (collision.gameObject.tag == "Pipe")
		{
			PlayerController.instance.StayPipeCollision();
		}
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "NoWaterGround") {
			Debug.Log("freaking out");
			PlayerController.instance.ExitGroundCollision();
		}
		else if (collision.gameObject.tag == "Pipe")
			PlayerController.instance.ExitPipeCollision();
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "DeadZone")
		{
			StartCoroutine(PlayerController.instance.Die());
		}
		else if (collision.gameObject.tag == "Checkpoint")
		{
			PlayerController.instance.EnterTriggerCheckpoint(collision.gameObject.transform);
		}
		else if (collision.gameObject.tag == "NextLevelPipe")
		{
			PlayerController.instance.LoadNextLevel();
			GameManager.instance.LoadNextLevel();
		}
	}
}
