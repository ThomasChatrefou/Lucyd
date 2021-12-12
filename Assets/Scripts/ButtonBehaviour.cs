using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButtonBehaviour : MonoBehaviour
{
    public bool canBeDisabledByHitAgain = true;
    public bool hasTimer;

    public float enabledDuration;
    public float antiSpamDelay = 1.0f;

    public bool on = false;

    private bool isClicked = false;
    private bool pushable = false;
    private bool inDarkWorld = false;

    private float disablingCountDown;
    private float canHitAgain;
    private float buttonOriginalY;
    private float buttonDownDistance;
    private float buttonReturnSpeed;

    private Transform button;

    private Collider clickableCollider;

    private GameObject player;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameManager.instance.darkWorldCamera;

        player = GameObject.FindWithTag("Player");

        clickableCollider = transform.GetComponent<Collider>();

        button = transform.GetChild(1);
        buttonDownDistance = button.lossyScale.y;
        buttonOriginalY = button.position.y;
        buttonReturnSpeed = buttonDownDistance / antiSpamDelay;
        
        if (gameObject.layer == 6 || gameObject.layer == 8)
            inDarkWorld = false;
        else if (gameObject.layer == 7 || gameObject.layer == 9)
            inDarkWorld = true;
    }


    void CustomMouseDown()
    {
        if (Input.GetMouseButton(0) && canHitAgain < Time.time)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == clickableCollider)
                {
                    isClicked = true;
                    player.GetComponent<NavMeshAgent>().SetDestination(clickableCollider.transform.position);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (inDarkWorld == GameManager.instance.darkWorld)
        {
            CustomMouseDown();

            if (pushable && isClicked && canHitAgain < Time.time)
            {
                isClicked = false;

                if (canBeDisabledByHitAgain)
                {
                    if (hasTimer && !on)
                        disablingCountDown = Time.time + enabledDuration;

                    button.position -= new Vector3(0, buttonDownDistance, 0);
                    on = !on;
                }
                else
                {
                    if (hasTimer)
                    {
                        disablingCountDown = Time.time + enabledDuration;
                        button.position -= new Vector3(0, buttonDownDistance, 0);
                    }
                    else
                    {
                        if (!on)
                            button.position -= new Vector3(0, buttonDownDistance, 0);
                    }
                    on = true;
                }

                canHitAgain = Time.time + antiSpamDelay;
            }
        }

        if (hasTimer && disablingCountDown < Time.time)
            on = false;
        
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
}
