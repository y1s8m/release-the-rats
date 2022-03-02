using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform startPos;
    public Transform checkpointPos;
    public Transform playerPos;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    public static GameManager m_instance;

    private int cheeseCounter = 0;
    private int ratSaveCounter = 0;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void start(){
        Load();
        Save(startPos);
    }

    void update(){
        if (playerPos.position == checkpointPos.position){
            Save(checkpointPos);
        }
    }

    public void Save(Transform pos){
        //scene number, xsign, x100, x10, x1, xdp1, xdp2, ysign, y100, y10, y1, ydp1, ydp2
        PlayerPrefs.SetFloat("xpos", pos.position.x);
        PlayerPrefs.SetFloat("ypos", pos.position.y);
        PlayerPrefs.SetInt("level",SceneManager.GetActiveScene().buildIndex);
    }

    public void Load(){
        if (PlayerPrefs.HasKey("level")){
            playerPos.position = new Vector3(PlayerPrefs.GetFloat("xpos"), PlayerPrefs.GetFloat("ypos"), 0);
        }
    }

    public void IncCheeseCounter()
    {
        cheeseCounter++;
        UIManager.instance.UpdateCheeseCounter(cheeseCounter);
    }

    public void IncRatSaveCounter()
    {
        ratSaveCounter++;
        UIManager.instance.UpdateRatSaveCounter(ratSaveCounter);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(WaitLoadSceneCoroutine(2));
    }

    IEnumerator WaitLoadSceneCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
