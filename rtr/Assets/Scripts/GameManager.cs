using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
