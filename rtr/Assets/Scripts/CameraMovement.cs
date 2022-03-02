using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public Transform playerTransform;
    public float dampTime = 0.1f;


    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 delta = new Vector3(playerTransform.position.x, 0, -10) - cam.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        Vector3 destination = transform.position + delta;
        if (destination.x < 0) destination = new Vector3(0, destination.y, destination.z);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
