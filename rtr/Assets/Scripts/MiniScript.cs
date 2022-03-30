using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScript : MonoBehaviour
{
    
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 newPosition = Player.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
