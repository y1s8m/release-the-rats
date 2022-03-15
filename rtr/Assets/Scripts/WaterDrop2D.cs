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
        Destroy(gameObject, lifeTime);
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
}
