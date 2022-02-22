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

    void Start()
    {
        cam = GameObject.Find("DarkWorldCam").GetComponent<Camera>();

        player = GameObject.FindWithTag("Player");

        clickableCollider = transform.GetComponent<Collider>();
        if(transform.childCount > 1)
            button = transform.GetChild(1);
        if(button)
        {
            buttonDownDistance = button.lossyScale.y;
            buttonOriginalY = button.position.y;
            buttonReturnSpeed = buttonDownDistance / antiSpamDelay;
        }

        
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

            if (Physics.Raycast(ray, out hit) || Physics.Raycast(ray, out hit, 1000f, 0))
            {
                if (hit.collider == clickableCollider)
                {
                    isClicked = true;
                    NavMeshAgent agent = player.GetComponent<NavMeshAgent>();

                    Vector3 buttonAvoidance = agent.radius * Vector3.Normalize(player.transform.position - clickableCollider.transform.position);
                    Vector3 dest = clickableCollider.transform.position + buttonAvoidance;

                    agent.SetDestination(dest);
                }
            }
        }
    }

    void Update()
    {
        if (gameObject.layer == 0)
            inDarkWorld = WorldTransitionManager.darkWorld;
        if (inDarkWorld == WorldTransitionManager.darkWorld)
        {
            CustomMouseDown();
            if (pushable && isClicked && canHitAgain < Time.time)
            {
                isClicked = false;

                if (canBeDisabledByHitAgain)
                {
                    if (hasTimer && !on)
                        disablingCountDown = Time.time + enabledDuration;
                    if (button)
                        button.position -= new Vector3(0, buttonDownDistance, 0);
                    on = !on;
                }
                else
                {
                    if (hasTimer)
                    {
                        disablingCountDown = Time.time + enabledDuration;
                        if (button)
                            button.position -= new Vector3(0, buttonDownDistance, 0);
                    }
                    else
                    {
                        if (!on && button)
                        {
                            button.position -= new Vector3(0, buttonDownDistance, 0);
                        }
                    }
                    on = true;
                }

                canHitAgain = Time.time + antiSpamDelay;
            }
        }

        if (hasTimer && disablingCountDown < Time.time)
            on = false;
        
        if(button)
        {
            if (button.position.y < buttonOriginalY)
                button.position += new Vector3(0, Time.deltaTime * buttonReturnSpeed, 0);
        }
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
