using UnityEngine;
using UnityEngine.AI;

public class Carriable : MonoBehaviour
{
    public float antiSpamDelay = 0.1f;

    public GameObject shadowPrefab;
    public Camera cam;

    private bool carried;
    private bool isClicked;

    private float canClickAgain;
    private float carriableRange = 2.0f;
   
    private GameObject player;
    private GameObject shadow;

    private Collider clickableCollider;
    private Rigidbody rg;
    private NavMeshObstacle obstacle;
    private NavMeshAgent playerAgent;

    private LayerMask mask;

    void Start()
    {
        clickableCollider = GetComponent<Collider>();
        rg = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();

        player = GameObject.FindWithTag("Player");
        playerAgent = player.GetComponent<NavMeshAgent>();

        carried = false;
        isClicked = false;
    } 


    void CustomMouseDown()
    {
        if (Input.GetMouseButton(0) && canClickAgain < Time.time)
        {
            if (DualWorldManager.darkWorld)
                mask = LayerMask.GetMask("DarkWorld");
            else
                mask = LayerMask.GetMask("LightWorld");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, mask))
            {
                if (hit.collider == clickableCollider)
                {
                    if (carried == false)
                    {
                        isClicked = true;
                        //on avance jusqu'a la caisse 

                        Vector3 boxAvoidance = playerAgent.radius
                            * Vector3.Normalize(player.transform.position - clickableCollider.transform.position);
                        Vector3 dest = clickableCollider.transform.position + boxAvoidance;

                        playerAgent.SetDestination(dest);
                    }

                    if (shadow)
                    {
                        rg.isKinematic = false;
                        transform.parent = null;
                        carried = false;
                        Destroy(shadow);
                    }
                }
            }

            canClickAgain = Time.time + antiSpamDelay;
        }
    }


    void DropBox(RaycastHit hit, bool playerHasToMove)
    {
        if (playerHasToMove)
            shadow.transform.position = hit.point + Vector3.up * obstacle.size.y;
        else
            shadow.transform.position = transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            carried = false;
            canClickAgain = Time.time + antiSpamDelay;

            if (playerHasToMove)
            {
                Vector3 boxAvoidance = playerAgent.radius
                        * Vector3.Normalize(player.transform.position - shadow.transform.position);
                Vector3 dest = shadow.transform.position + boxAvoidance;

                playerAgent.SetDestination(dest);
            }
        }
    }


    void Update()
    {
        CustomMouseDown();

        playerAgent.isStopped = false;
        float dist;

        if (isClicked == true)
        {
           //si on clique alors qu'on a pas de caisse on porte une caisse ( bonne distance )
            if (carried == false)
            {
                dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist <= carriableRange)
                {
                    carried = true;
                    obstacle.enabled = false;
                }
            }
            else
            {
                rg.isKinematic = true;
                transform.parent = player.transform;
                transform.position += Vector3.up * obstacle.size.y;
                isClicked = false;
                shadow = Instantiate(shadowPrefab);
            }
        }
        else
        {
            //si on porte une caisse alors une ombre apparait pour poser la caisse 
            //si on essaye de viser un point trop haut l'ombre ne s'affiche pas 
            if (carried == true) 
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Physics.Raycast(ray, out hit, 1000f, mask);

                if (hit.collider == clickableCollider)
                    DropBox(hit, false);
                else
                {
                    NavMeshHit navRayHit;
                    bool navRay;
                    navRay = !NavMesh.Raycast(hit.point, hit.point, out navRayHit, NavMesh.AllAreas);

                    if (navRay)
                    {
                        NavMeshHit navEdgeHit;
                        bool navEdge;
                        navEdge = NavMesh.FindClosestEdge(hit.point, out navEdgeHit, NavMesh.AllAreas);

                        if (navEdge)
                            DropBox(hit, true);
                    }
                }

                if(Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Default")))
                {
                    if (hit.transform.CompareTag("Player"))
                        DropBox(hit, false);
                }
            }
            else if (shadow)
            {
                dist = Vector3.Distance(player.transform.position, shadow.transform.position);
                if (dist <= carriableRange)
                {
                    transform.position = shadow.transform.position;
                    rg.isKinematic = false;
                    transform.parent = null;
                    carried = false;
                    obstacle.enabled = true;

                    Destroy(shadow);
                }
            }
        }
    }
}
