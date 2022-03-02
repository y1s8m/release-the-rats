using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public Transform playerTransform;
    public float dampTime = 0.1f;
    public int repeats = 275;

    public GameObject goal;

    private bool cutSceneDone = false;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        StartCoroutine(DoCutscene());
    }

    private void Update()
    {
        if (cutSceneDone) {
            Vector3 delta = new Vector3(playerTransform.position.x, 0, -10) - cam.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
            Vector3 destination = transform.position + delta;
            if (destination.x < 0) destination = new Vector3(0, destination.y, destination.z);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    private IEnumerator DoCutscene() {
        float xDist = (Mathf.Abs(goal.transform.position.x - playerTransform.position.x) / 10) - .25f;
        float yDist = (Mathf.Abs(goal.transform.position.y - playerTransform.position.y) / 10) + .25f;
        Debug.Log(yDist);
        for (int i = 0; i < repeats; i++) {
            Vector3 goalPos = new Vector3(transform.position.x + xDist, transform.position.y + yDist, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, dampTime);
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        cutSceneDone = true;
    }
}
