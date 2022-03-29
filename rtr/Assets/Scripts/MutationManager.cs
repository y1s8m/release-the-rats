using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationManager : MonoBehaviour
{
    public static MutationManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MutationManager>();
            }

            return m_instance;
        }
    }

    public static MutationManager m_instance;

    public GameObject[] potions;
    public GameManager mandrake;
    public SpriteRenderer mandrakeSR;
    public Sprite mandrakeHighlight;

    private bool jumpTextOn = false;
    public int potionCount;

    // Start is called before the first frame update
    void Start()
    {
        potionCount = potions.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (potionCount == 0 && !jumpTextOn)
        {
            jumpTextOn = true;
            UIManager.instance.JumpTextOn();
            mandrakeSR.sprite = mandrakeHighlight;
        }
    }

    public void PotionCountDec()
    {
        potionCount--;
        if (potionCount < 0) potionCount = 0;
    }

    public void DropMandrake()
    {
        if (potionCount != 0) return;
        Rigidbody2D mandrakeRB = mandrake.gameObject.AddComponent<Rigidbody2D>();
        mandrakeRB.gravityScale = 3f;
        mandrakeRB.constraints = RigidbodyConstraints2D.None;
    }
}
