using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float dampTime = 0.1f;
    public int repeats = 275;

    public GameObject goal;

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

        float xDist = ((goal.transform.position.x - playerTransform.position.x) / repeats);
        float yDist = ((goal.transform.position.y - playerTransform.position.y) / repeats);
        Vector3 tempGoal = transform.position;

        for (int i = 0; i < repeats; i++) {
            tempGoal.x += xDist;
            tempGoal.y += yDist;
            transform.position = Vector3.SmoothDamp(transform.position, tempGoal, ref velocity, dampTime);
            yield return new WaitForSeconds(.001f);
        }
        yield return new WaitForSeconds(1f);
        cutScene = false;
        if (notMaze) PlayerController.instance.cutScene = false;
        else PlayerMazeController.instance.cutScene = false;
    }
}
