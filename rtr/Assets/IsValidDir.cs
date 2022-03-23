using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsValidDir : MonoBehaviour
{
    public string name;
    
    bool valid;
    bool going = false;
    float timePassed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        valid = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (going) timePassed += Time.deltaTime;
        if (timePassed > 1f) Destroy(this.gameObject);
    }
    void OnTriggerEnter2D (Collider2D col){
        if (col.gameObject.tag == "wall"){
            valid = true;
        }
        if (name == "Up" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidUp(valid);
        }
        if (name == "Down" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidDown(valid);
        }
        if (name == "Left" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidLeft(valid);
        }
        if (name == "Right" ){
            this.gameObject.transform.parent.GetComponent<ValidMoves>().SetValidRight(valid);
        }
    }

    public void Go() {
        if (!going) going = true;
        if (gameObject.GetComponent<Rigidbody2D>() == null) {
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
        if (name == "Up") {
            if (gameObject.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0)) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 1f, 0f);
        } else if (name == "Down") {
            if (gameObject.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0)) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, -1f, 0f);
        } else if (name == "Left") {
            if (gameObject.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0)) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-1f, 0f, 0f);
        } else {
            if (gameObject.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0)) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1f, 0f, 0f);
        }
    }
}
