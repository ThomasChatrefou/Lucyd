using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

    public bool on = false;
    public float buttonReturnSpeed = 0.1f;
    public float buttonCooldown = 1.0f;

    private bool pushable = false;

    private Transform button;

    private float canHitAgain;
    private float buttonOriginalY;
    private float buttonDownDistance;

    //remplacer par reference vers l'objet relié au bouton
    //public bool ButtonState = false;


    // Start is called before the first frame update
    void Start()
    {
        button = transform.GetChild(1);
        buttonDownDistance = button.lossyScale.y;
        buttonOriginalY = button.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushable && Input.GetMouseButtonDown(1) && canHitAgain < Time.time)
        {
            on = !on;
            button.position -= new Vector3(0, buttonDownDistance, 0);
            
            if (on)
            {
                print("button on");
            }
            else
            {
                print("button off");
            }

            canHitAgain = Time.time + buttonCooldown;
        }

        if (button.position.y < buttonOriginalY)
        {
            button.position += new Vector3(0, Time.deltaTime * buttonReturnSpeed, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pushable = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pushable = false;
        }
    }
}
