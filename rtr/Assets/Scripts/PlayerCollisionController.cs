using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (PlayerController.instance.dead) return;

		if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MoveableObject")
		{
			PlayerController.instance.EnterGroundBodyCollision();
			if (collision.gameObject.tag == "MoveableObejct") PlayerController.instance.HoldMoveable();
		}
		else if (collision.gameObject.tag == "Pipe") {
            PlayerController.instance.EnterPipeCollision(collision.GetContact(0).point, collision.GetContact(0).normal.normalized);
        } else if (collision.gameObject.tag == "MandrakeJumpTrig") {
            PlayerController.instance.EnterGroundBodyCollision();
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

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Pipe") {
			PlayerController.instance.ExitPipeCollision();
        } else if (collision.gameObject.tag == "MoveableObject") PlayerController.instance.ReleaseMoveable();
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
		else if (collision.gameObject.tag == "Cauldron")
        {
			// should play animatic
			if (MutationManager.instance.potionCount == 0)
			{
				PlayerController.instance.FallInCauldron();
				GameManager.instance.LoadNextLevel();
			}
        }
	}
}
