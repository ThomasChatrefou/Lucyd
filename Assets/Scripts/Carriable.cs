using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carriable : MonoBehaviour
{
    private bool carried;
    private bool isClicked;
    private bool CanPose;
    private float dist = 0;
    private GameObject player;
   
    private GameObject shadow;
    public GameObject shadowPrefab;
    public Camera cam;
    RaycastHit hit;

    //variable de debug 
   /* public float DistBrasx=0;
    public float DistBrasy=0;*/
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        carried = false;
        isClicked = false;
        CanPose = false;
}
    //on clique sur la caisse en question
    void OnMouseDown()
    {
        if (carried == false)
        {
            isClicked = true;
            //on avance jusqu'a la caisse 
            player.GetComponent<NavMeshAgent>().SetDestination(transform.position - new Vector3(0.8f,0.8f,0.8f));
        }
        if (shadow)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = null;
            carried = false;
            Destroy(shadow);
        }

    }
    
    void Update()
    {
        player.GetComponent<NavMeshAgent>().isStopped = false;
        if (isClicked == true)
        {
           //si on clique alors qu'on a pas de caisse on porte une caisse ( bonne distance )
            if (carried == false)
            {
                dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist <= 2f)
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
            //si on porte une caisse alors une ombre apparait pour poser la caisse 
            //si on essaye de viser un point trop haut l'ombre ne s'affiche pas 
            if (carried == true) 
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit);
                
                if ( hit.point.y <= player.transform.position.y -0.55f || (hit.point.y <= player.transform.position.y - 0.55f && hit.point.x >= player.transform.position.x +0.5f))
                {
                    shadow.transform.position = hit.point + new Vector3(0, 0.5f, 0);

                    if (Input.GetMouseButtonDown(0))
                    {
                        carried = false;
                        player.GetComponent<NavMeshAgent>().SetDestination(shadow.transform.position);
                    }
                }
                else
                {
                    //print("c'est hors de port�");
                }

                    
                
            }
            else if (shadow)
            {
                dist = Vector3.Distance(player.transform.position, shadow.transform.position);
                if (dist <= 2f)
                {
                    
                    transform.position = shadow.transform.position;
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    carried = false;
                    GetComponent<NavMeshObstacle>().enabled = true;
                    player.GetComponent<NavMeshAgent>().SetDestination(player.transform.position - new Vector3(0.2f, 0.2f,0.2f));


                    Destroy(shadow);
                }
            }
        }
    }

   
}
