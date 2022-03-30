using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public static InstructionsManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InstructionsManager>();
            }

            return m_instance;
        }
    }
    public GameObject instrPanel;
    public bool cutscene = true;
    private static InstructionsManager m_instance;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.anyKey){
            instrPanel.SetActive(false);
        }
    }

    public void ShowInstructions() {
        instrPanel.SetActive(true);
    }
}
