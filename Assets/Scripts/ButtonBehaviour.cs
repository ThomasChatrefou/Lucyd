using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButtonBehaviour : MonoBehaviour
{
    public float buttonReturnSpeed = 0.1f;
    public float buttonCooldown = 1.0f;

    public bool on = false;

    private bool isClicked = false;
    private bool pushable = false;

    private float canHitAgain;
    private float buttonOriginalY;
    private float buttonDownDistance;

    private Transform button;

    private GameObject player;

    //remplacer par reference vers l'objet relié au bouton
    //public bool ButtonState = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        button = transform.GetChild(1);
        buttonDownDistance = button.lossyScale.y;
        buttonOriginalY = button.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushable && isClicked && canHitAgain < Time.time)
        {
            on = !on;
            isClicked = false;
            button.position -= new Vector3(0, buttonDownDistance, 0);
            
            if (on)
                print("button on");
            else
                print("button off");

            canHitAgain = Time.time + buttonCooldown;
        }

        if (button.position.y < buttonOriginalY)
            button.position += new Vector3(0, Time.deltaTime * buttonReturnSpeed, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            pushable = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            pushable = false;
    }

    void OnMouseDown()
    {
        if (canHitAgain < Time.time)
            isClicked = true;
        player.GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
}
