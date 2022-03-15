using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop2D : MonoBehaviour
{
    public float lifeTime = 5f;
    
    private CircleCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        WaterSystem2D.S.Add(this);
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetRadius() {
        return collider.radius;
    }

    private void OnDestroy() {
        WaterSystem2D.S.Remove(this);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.transform.tag == "DeadZone") {
            Destroy(this.gameObject);
        }
    }
}
