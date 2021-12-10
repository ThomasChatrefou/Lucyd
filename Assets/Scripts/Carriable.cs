using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carriable : MonoBehaviour
{
    private bool carried;
    private bool isClicked;
    private float dist = 0;
    private GameObject player;
    private GameObject shadow;
    public GameObject shadowPrefab;
    public Camera cam;
    RaycastHit hit;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        carried = false;
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked == true)
        {
           
            if (carried == false)
            {
                dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist <= 1.5f)
                {
                    carried = true;
                    GetComponent<NavMeshObstacle>().enabled = false;
                }
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = player.transform;
                transform.position += Vector3.up * 0.5f;
                isClicked = false;
                shadow = Instantiate(shadowPrefab);
            }
        }
        else
        {
            if (carried == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    carried = false;
                    player.GetComponent<NavMeshAgent>().SetDestination(shadow.transform.position);
                }
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit);

                shadow.transform.position = hit.point + new Vector3(0, 0.5f, 0);
            }
            else if (shadow)
            {
                dist = Vector3.Distance(player.transform.position, shadow.transform.position);
                if (dist <= 1.5f)
                {
                    player.GetComponent<NavMeshAgent>().isStopped = true;
                    transform.position = shadow.transform.position;
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    carried = false;
                    GetComponent<NavMeshObstacle>().enabled = true;
                    Destroy(shadow);
                }
            }
        }
    }


    void OnMouseDown()
    {
        isClicked = true;
        player.GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
}
