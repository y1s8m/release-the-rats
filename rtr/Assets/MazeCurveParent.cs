using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCurveParent : MonoBehaviour
{
    public bool collided = false;
    public bool chcollided = false;
    public bool isChild = false;
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
            col.gameObject.GetComponent<MazeMovement>().isChild = false;
            if (this.gameObject.transform.GetChild(0).gameObject.GetComponent<MazeCurve>().collided){
                collided = false;
                this.gameObject.transform.GetChild(0).gameObject.GetComponent<MazeCurve>().collided = false;
                //move player 
                chcollided = true;
                //move player
            }
            else {
                //col.gameObject.transform.position =  this.gameObject.transform.GetChild(0).gameObject.transform.position;
                chcollided = false;
                //col.gameObject.transform.position =  this.gameObject.transform.GetChild(0).gameObject.transform.position;
            }
        }
    }
}
