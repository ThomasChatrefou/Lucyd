using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehaviour : MonoBehaviour
{
    public bool on = false;
    public float leverSpeed;

    private bool pullable = false;

    private GameObject leverStick;

    private float currentRotation = 0;
    private float stepRotation;
    private float maxRotation = 90;

    // Start is called before the first frame update
    void Start()
    {
        leverStick = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        stepRotation = Time.deltaTime * leverSpeed;

        if (pullable)
        {
            if (Input.GetMouseButton(1))
            {
                if(currentRotation < maxRotation)
                {
                    leverStick.transform.Rotate(stepRotation, 0, 0,Space.Self);
                    currentRotation += stepRotation;
                }
                else
                {
                    on = !on;
                }
            }
            else
            {
                if (currentRotation > 0)
                {
                    leverStick.transform.Rotate(-stepRotation, 0, 0, Space.Self);
                    currentRotation -= stepRotation;
                }
            }
        }
        else
        {
            if (currentRotation > 0)
            {
                leverStick.transform.Rotate(-stepRotation, 0, 0, Space.Self);
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
