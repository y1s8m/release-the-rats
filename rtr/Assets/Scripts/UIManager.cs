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

    public GameObject[] jumpTexts;

    public GameObject deadPanel;
    private Image deadPanelImg;
    public GameObject deadText;
    private bool dead = false;
    private bool maxOpac = false;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        if (deadPanel) deadPanelImg = deadPanel.GetComponent<Image>();
        if (deadPanel) deadPanel.SetActive(false);

        foreach (GameObject go in jumpTexts) {
            go.SetActive(false);
        }
    }

    private void Update()
    {
        if (dead && !maxOpac)
        {
            deadPanelImg.color = new Color(deadPanelImg.color.r, deadPanelImg.color.g, deadPanelImg.color.b, deadPanelImg.color.a + 0.01f);

            if (deadPanelImg.color.a >= 1.0f) maxOpac = true;
        }
    }

    public void Die()
    {
        if (dead) return;

        dead = true;
        maxOpac = false;

        deadPanelImg.color = new Color(deadPanelImg.color.r, deadPanelImg.color.g, deadPanelImg.color.b, 0);
        JumpTextOff();
        deadPanel.SetActive(true);
    }

    public void DisableDeadPanel()
    {
        dead = false;
        deadPanel.SetActive(false);
    }

    public void JumpTextOn()
    {
        foreach (GameObject go in jumpTexts)
        {
            go.SetActive(true);
        }
    }

    public void JumpTextOff()
    {
        foreach (GameObject go in jumpTexts)
        {
            go.SetActive(false);
        }
    }
}
