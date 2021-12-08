using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

    public bool on = false;
    private bool buttonHit = false;
    private bool clicked = false;
    private GameObject button;

    private float buttonDownDistance = 0.05f;
    private float buttonReturnSpeed = 0.1f;
    private float buttonOriginalY;

    private float buttonCooldown = 1.0f;
    private float canHitAgain;

    //remplacer par reference vers l'objet relié au bouton
    //public bool ButtonState = false;


    // Start is called before the first frame update
    void Start()
    {
        button = transform.GetChild(1).gameObject;
        buttonOriginalY = button.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonHit)
        {
            buttonHit = false;
            on = !on;


            button.transform.position -= new Vector3(0, buttonDownDistance, 0);


            if (on)
            {
                print("button on");
            }
            else
            {
                print("button off");
            }

        }
        if (button.transform.position.y < buttonOriginalY)
        {
            button.transform.position += new Vector3(0, Time.deltaTime * buttonReturnSpeed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(clicked && other.CompareTag("Player") && canHitAgain < Time.time)
        {
            canHitAgain = Time.time + buttonCooldown;
            buttonHit = true;
            clicked = false;
        }
    }

    private void OnMouseDown()
    {
        clicked = true;
        print(clicked);
    }

}
