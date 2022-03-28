using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool maxOpac = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
    }

    private void Update()
    {
        if (!maxOpac)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + 0.01f);

            if (sr.color.a >= 1.0f) maxOpac = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeleteProjectile")
        {
            Destroy(this.gameObject);
        }
    }
}
