using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressBehaviour : MonoBehaviour
{
    public float pressSpeed = 1.0f;

    private int nObjectsOnPress = 0;

    private Transform plate;

    private float plateDownY;
    private float plateOriginalY;
    private float plateDownDistance;


    // Start is called before the first frame update
    void Start()
    {
        Transform pressBase = transform.GetChild(0);
        plate = transform.GetChild(1);

        plateDownDistance = 0.8f * (plate.lossyScale.y - pressBase.lossyScale.y) / 2;
        plateOriginalY = plate.position.y;
        plateDownY = plateOriginalY - plateDownDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nObjectsOnPress > 0)
        {
            if (plate.position.y > plateDownY)
            {
                plate.Translate(0, -Time.deltaTime * pressSpeed, 0);
            }
        }
        else
        {
            if (plate.position.y < plateOriginalY)
            {
                plate.Translate(0, Time.deltaTime * pressSpeed, 0);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovableObject"))
        {
            nObjectsOnPress++;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MovableObject"))
        {
            if (nObjectsOnPress > 0) nObjectsOnPress--;
        }
    }
}
