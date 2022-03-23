using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsValidDir : MonoBehaviour
{
    bool valid;
    
    // Start is called before the first frame update
    void Start()
    {
        valid = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "wall"){
            valid = false;
        }
        if (this.gameObject.name == "Up" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidUp(valid);
        }
        if (this.gameObject.name == "Down" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidDown(valid);
        }
        if (this.gameObject.name == "Left" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidLeft(valid);
        }
        if (this.gameObject.name == "Right" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidRight(valid);
        }
    }
}
