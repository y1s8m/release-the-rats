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

    public GameObject deadPanel;
    private Image img;
    private bool maxOpac = false;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        deadPanel.SetActive(true);
        if (!maxOpac)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + 0.01f);

            if (img.color.a >= 1.0f) maxOpac = true;
        }
    }
}
