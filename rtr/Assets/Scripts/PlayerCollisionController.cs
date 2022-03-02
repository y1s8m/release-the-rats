using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (PlayerController.instance.dead) return;
		if (collision.gameObject.tag == "Ground") PlayerController.instance.EnterGroundCollision();
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (PlayerController.instance.dead) return;

		if (collision.gameObject.tag == "Ground") PlayerController.instance.StayGroundCollision();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// "Slippery" tagged regions are not meant to be traversed via wall
		if (collision.gameObject.tag == "Slippery")
		{
			PlayerController.instance.EnterTriggerSlippery();
		}
		else if (collision.gameObject.tag == "Checkpoint")
		{
			PlayerController.instance.EnterTriggerCheckpoint(collision.gameObject.transform);
		}
		else if (collision.gameObject.tag == "Cheese")
		{
			GameManager.instance.IncCheeseCounter();
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.tag == "OtherRat")
		{
			GameManager.instance.IncRatSaveCounter();
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.tag == "NextLevelPipe")
		{
			GameManager.instance.LoadNextLevel();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// "Slippery" tagged regions are not meant to be traversed via wall
		if (collision.gameObject.tag == "Slippery")
		{
			PlayerController.instance.ExitTriggerSlippery();
		}
	}
}