using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehaviour : MonoBehaviour
{
    public bool on = false;
    private bool leverHit = false;
    private GameObject leverStick;

    public float leverRotation = -90;

    // Start is called before the first frame update
    void Start()
    {
        leverStick = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (leverHit)
        {
            leverHit = false;
            on = !on;

            if (on)
            {
                print("lever on");
                leverStick.transform.rotation = Quaternion.Euler(
                    leverStick.transform.eulerAngles.x + leverRotation,
                    leverStick.transform.eulerAngles.y,
                    leverStick.transform.eulerAngles.z);
            }
            else
            {
                print("lever off");
                leverStick.transform.rotation = Quaternion.Euler(
                    leverStick.transform.eulerAngles.x - leverRotation,
                    leverStick.transform.eulerAngles.y,
                    leverStick.transform.eulerAngles.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            leverHit = true;
        }
    }
}
