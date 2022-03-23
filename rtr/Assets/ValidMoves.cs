using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMoves : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject right;
    public GameObject left;

    bool validUp = true;
    bool validDown = true;
    bool validLeft = true;
    bool validRight = true;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValidUp(bool valid){
        validUp = valid;
    }
    public bool GetValidUp(){
        return validDown;
    }

    public void SetValidDown(bool valid){
        validDown = valid;
    }
    public bool GetValidDown(){
        return validDown;
    }

    public void SetValidLeft(bool valid){
        validLeft = valid;
    }
    public bool GetValidLeft(){
        return validLeft;
    }

    public void SetValidRight(bool valid){
        validRight = valid;
    }
    public bool GetValidRight(){
        return validRight;
    }
}
