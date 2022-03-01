using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public Text cheeseCounter;
    public Text ratSaveCounter;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCheeseCounter(int c)
    {
        cheeseCounter.text = c.ToString();
    }

    public void UpdateRatSaveCounter(int c)
    {
        ratSaveCounter.text = c.ToString();
    }
}
