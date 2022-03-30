using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CameraMovement>();
            }

            return m_instance;
        }
    }

    public static CameraMovement m_instance;

    public Transform playerTransform;
    public float dampTime = 0.1f;
    public float repeats = 275;

    public Transform goal;

    private bool cutScene = false;
    private bool afterCutScene = false;
    private bool notMaze = true;
    private bool witchExist = false;

    public bool movieNotExists = false;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        GetComponent<Camera>().backgroundColor = Color.black;
        if (movieNotExists) StartCoroutine(DoCutscene());

        if (PlayerController.instance) notMaze = true;
        if (MazeMovement.instance) notMaze = false;
        if (WitchController.instance) witchExist = true;
    }

    private void Update()
    {
        if (afterCutScene && SceneManager.GetActiveScene().buildIndex == 5) return;
        if (!cutScene) {
            Vector3 delta = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    public void StartCutscene()
    {
        StartCoroutine(DoCutscene());
    }

    private IEnumerator DoCutscene()
    {
        UIManager.instance.BrighterAnim();

        yield return new WaitForSeconds(1.5f);
        cutScene = true;

        float origZ = transform.position.z;

        float xDist = ((goal.position.x - playerTransform.position.x) / repeats);
        float yDist = ((goal.position.y - playerTransform.position.y) / repeats);
        float zDist = ((goal.position.z - playerTransform.position.z) / repeats);
        Vector3 tempGoal = transform.position;

        while (Mathf.Abs(goal.position.x - transform.position.x) > 0.2f || Mathf.Abs(goal.position.y - transform.position.y) > 0.2f
                                                                        || Mathf.Abs(goal.position.z - transform.position.z) > 0.2f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                                new Vector3(goal.position.x, goal.position.y, goal.position.z), ref velocity, 1f);
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        if (SceneManager.GetActiveScene().buildIndex != 5)
        {
            while ((Mathf.Abs(playerTransform.position.x - transform.position.x) > 0.2f || Mathf.Abs(playerTransform.position.y - transform.position.y) > 0.2f
                                                                                        || Mathf.Abs(origZ - transform.position.z) > 0.2f) && cutScene)
            {
                transform.position = Vector3.SmoothDamp(transform.position,
                                    new Vector3(playerTransform.position.x, playerTransform.position.y, origZ), ref velocity, 1f);
                yield return null;
            }
        }

        cutScene = false;
        afterCutScene = true;
        if (notMaze) PlayerController.instance.cutScene = false;
        else MazeMovement.instance.cutScene = false;

        if (witchExist) WitchController.instance.cutScene = false;

        if ((SceneManager.GetActiveScene().buildIndex == 1) && (notMaze)) InstructionsManager.instance.instrPanel.SetActive(true);
        if ((SceneManager.GetActiveScene().buildIndex == 3) && (notMaze)) InstructionsManager.instance.instrPanel.SetActive(true);
    }
}
