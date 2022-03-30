using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidMoves : MonoBehaviour
{
    public GameObject up;
    public GameObject down;
    public GameObject right;
    public GameObject left;

    public GameObject player;

    bool validUp = true;
    bool validDown = true;
    bool validLeft = true;
    bool validRight = true;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
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

    private void OnTriggerEnter2D() {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        up.GetComponent<IsValidDir>().Go();
        down.GetComponent<IsValidDir>().Go();
        left.GetComponent<IsValidDir>().Go();
        right.GetComponent<IsValidDir>().Go();
        StartCoroutine(Label());
    }

    private void OnTriggerExit2D() {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    private IEnumerator Label() {
        yield return new WaitForSeconds(.2f);
        player.GetComponent<MazeMovement>().SetUp(validUp);
        player.GetComponent<MazeMovement>().SetDown(validDown);
        player.GetComponent<MazeMovement>().SetRight(validRight);
        player.GetComponent<MazeMovement>().SetLeft(validLeft);
    }
}
