using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit: https://danielilett.com/2020-03-28-tut5-2-urp-metaballs/

public class WaterSystem2D : MonoBehaviour
{
    public static WaterSystem2D S;
    
    public int numDrops = 100;
    public float timeBetweenDrops = .25f;

    public GameObject waterDropPrefab;

    private List<WaterDrop2D> waterdrops;

    private void Awake() {
        if (WaterSystem2D.S) {
            Destroy(this.gameObject);
        } else {
            S = this;
        }

        waterdrops = new List<WaterDrop2D>();
        Debug.Log(waterdrops);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnWater();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(WaterDrop2D waterdrop) {
        waterdrops.Add(waterdrop);
    }

    public List<WaterDrop2D> GetDropList() {
        return waterdrops;
    }

    public void Remove(WaterDrop2D waterdrop) {
        waterdrops.Remove(waterdrop);
    }

    public void SpawnWater() {
        StartCoroutine(SpawnDrops());
    }

    public IEnumerator SpawnDrops() {
        if (numDrops != -1) {
            for (int i = 0; i < numDrops; i++) {
                Instantiate(waterDropPrefab, transform);
                yield return new WaitForSeconds(timeBetweenDrops);
            }
        } else {
            while (true) {
                Instantiate(waterDropPrefab, transform);
                yield return new WaitForSeconds(timeBetweenDrops);
            }
        }
    }
}
