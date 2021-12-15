using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFeuBehaviour : MonoBehaviour
{
    private ButtonBehaviour Button;
    private Vector3 HidingSpot;
    private Vector3 GoodSpot;
    private void Start()
    {
        HidingSpot = transform.position + Vector3.down * 5f;
        GoodSpot = transform.position;
    }

    void Update()
    {
        if (GameManager.instance.darkWorld == false)
        {
            transform.position = HidingSpot;
        }
        else if (GameManager.instance.darkWorld == true)
        {
            transform.position = GoodSpot;
        }
    }
}
