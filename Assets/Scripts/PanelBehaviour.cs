using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviour : MonoBehaviour
{
    public bool moveRight;
    public bool moveLeft;
    public bool moveUp;
    public bool moveDown;
    public bool moveForward;
    public bool moveBackward;

    public float dist;
    public float speed = 1.0f;

    public GameObject input;

    public ButtonBehaviour button; 

    private bool inputIsButton = false;
    private bool inputIsPress = false;
    private bool inputIsLever = false;

    private bool on;

    private float percent;

    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private Vector3 translation;


    // Start is called before the first frame update
    void Start()
    {
        if (moveRight)
            translation += transform.right;

        if (moveLeft) 
            translation -= transform.right;

        if (moveUp) 
            translation += transform.up;

        if (moveDown) 
            translation -= transform.up;

        if (moveForward)
            translation += transform.forward;

        if (moveBackward)
            translation -= transform.forward;

        translation.Normalize();

        if (input.CompareTag("Button"))
        {
            inputIsButton = true;
            on = input.GetComponent<ButtonBehaviour>().on;
        }
        if (input.CompareTag("Press"))
        {
            inputIsPress = true;
            on = input.GetComponent<PressBehaviour>().on;
        }
        if (input.CompareTag("Lever"))
        {
            inputIsLever = true;
            percent = input.GetComponent<LeverBehaviour>().percent;
        }
        
        originalPosition = transform.position;
        targetPosition = originalPosition + translation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputIsButton || inputIsPress)
        {
            if (button.on)
            {
                if (Vector3.Distance(transform.position, targetPosition) > 0.01)
                    transform.Translate(Time.deltaTime * speed * translation);
            }
            else
            {
                if (Vector3.Distance(transform.position, originalPosition) > 0.01)
                    transform.Translate(-Time.deltaTime * speed * translation);
            }
        }

        if (inputIsLever)
        {

        }
    }
}
