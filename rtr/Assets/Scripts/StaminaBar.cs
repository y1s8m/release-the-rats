using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");

        SetSize(PlayerController.instance.stamina);
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(1f, sizeNormalized);
    }
}
