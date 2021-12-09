using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehaviour : MonoBehaviour
{
    public float leverSpeed = 40;

    public float percent = 0;

    private bool pullable = false;

    private float currentRotation = 0;
    private float stepRotation;
    private float maxRotation = 90;

    private Transform leverStick;

    // Start is called before the first frame update
    void Start()
    {
        leverStick = transform.GetChild(1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stepRotation = Time.deltaTime * leverSpeed;

        if (pullable)
        {
            if (Input.GetMouseButton(1))
            {
                if(currentRotation < maxRotation)
                {
                    leverStick.Rotate(stepRotation, 0, 0,Space.Self);
                    currentRotation += stepRotation;
                }
            }
            else
            {
                if (currentRotation > 0)
                {
                    leverStick.Rotate(-stepRotation, 0, 0, Space.Self);
                    currentRotation -= stepRotation;
                }
            }
        }
        else
        {
            if (currentRotation > 0)
            {
                leverStick.Rotate(-stepRotation, 0, 0, Space.Self);
                currentRotation -= stepRotation;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pullable = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pullable = false;
        }
    }
}
