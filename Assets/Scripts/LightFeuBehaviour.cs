using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFeuBehaviour : MonoBehaviour
{
    private ButtonBehaviour Button;
    private Vector3 HidingSpot;
    private Vector3 GoodSpot;
    private void Start()
    {
        GoodSpot = transform.position;
        HidingSpot = transform.position + Vector3.down * 5f;
    }

    void Update()
    {
        if (GameManager.instance.darkWorld == true)
        {
            transform.position = HidingSpot;
        }
        else if (GameManager.instance.darkWorld == false)
        {
            transform.position = GoodSpot;
        }
    }
}