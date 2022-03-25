using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCurve : MonoBehaviour
{
    public bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Player"){
            collided = true;
            if (this.gameObject.transform.parent.GetComponent<MazeCurveParent>().collided){
                collided = false;
                this.gameObject.transform.parent.GetComponent<MazeCurveParent>().collided = false;
                //move player 
            }
            else {
                //start curve animation
            }
        }
    }
}
