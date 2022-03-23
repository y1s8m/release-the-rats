using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float dampTime = 0.1f;
    public float repeats = 275;

    public Transform goal;

    private bool cutScene = false;
    private bool notMaze = true;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        GetComponent<Camera>().backgroundColor = Color.black;
        StartCoroutine(DoCutscene());

        if (PlayerController.instance) notMaze = true;
        if (PlayerMazeController.instance) notMaze = false;
    }

    private void Update()
    {
        if (!cutScene) {
            Vector3 delta = new Vector3(playerTransform.position.x, playerTransform.position.y, -10) - GetComponent<Camera>().ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
            Vector3 destination = transform.position + delta;
            //if (destination.y < -6 && notMaze) destination = new Vector3(destination.x, -6, destination.z); //needs to be modified on a level basis
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    private IEnumerator DoCutscene() {
        yield return new WaitForSeconds(3f);
        cutScene = true;

        float xDist = ((goal.position.x - playerTransform.position.x) / repeats);
        float yDist = ((goal.position.y - playerTransform.position.y) / repeats);
        Vector3 tempGoal = transform.position;

        float isRight = (transform.position.x < goal.position.x) ? 1f : -1f;
        float isAbove = (transform.position.y < goal.position.y) ? 1f : -1f;

        // Note: this assumes that the goal is to the RIGHT and ABOVE the initial player transform
        while (transform.position.x < isRight * goal.position.x && transform.position.y < isAbove * goal.position.y)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(goal.position.x + 0.2f, goal.position.y + 0.2f, -10f), ref velocity, 1f);
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        while (transform.position.x > isRight * playerTransform.position.x && transform.position.y > isAbove * playerTransform.position.y && cutScene)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(playerTransform.position.x - 0.2f, playerTransform.position.y - 0.2f, -10f), ref velocity, 1f);
            yield return null;
        }

        cutScene = false;
        if (notMaze) PlayerController.instance.cutScene = false;
        else PlayerMazeController.instance.cutScene = false;
        if ((SceneManager.GetActiveScene().buildIndex == 1) && (notMaze)) InstructionsManager.instance.instrPanel.SetActive(true);
        if ((SceneManager.GetActiveScene().buildIndex == 3) && (notMaze)) InstructionsManager.instance.instrPanel.SetActive(true);
    }
}
