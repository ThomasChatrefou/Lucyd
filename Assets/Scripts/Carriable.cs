using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carriable : MonoBehaviour
{
    private bool Carried  ;
    private bool IsClicked ;
    private float Dist = 0;
    private GameObject Player;
    private GameObject Shadow;
    public GameObject ShadowPrefab;
    public Camera Cam;
    RaycastHit Hit;
    
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Carried = false;
        IsClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(Dist);
        if (IsClicked == true )
        {
           
            if (Carried == false)
            {
                Dist = Vector3.Distance(transform.position, Player.transform.position);
                if (Dist <= 1.5f)
                {
                    Carried = true;
                    GetComponent<NavMeshObstacle>().enabled = false;
                }
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = Player.transform;
                transform.position += Vector3.up * 0.5f;
                IsClicked = false;
                Shadow = Instantiate(ShadowPrefab);
            }
        }
        else
        {
            if (Carried == true)
            {
                if ( Input.GetMouseButtonDown(1))
                {
                    Carried = false;
                    Player.GetComponent<NavMeshAgent>().SetDestination(Shadow.transform.position);
                }
                Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out Hit);


                Shadow.transform.position = Hit.point + new Vector3(0, 0.5f, 0);

            }
            else if (Shadow)
            {
                Dist = Vector3.Distance(Player.transform.position, Shadow.transform.position);
                if (Dist <= 1.5f)
                {
                    Player.GetComponent<NavMeshAgent>().isStopped = true;
                    transform.position = Shadow.transform.position;
                    GetComponent<Rigidbody>().isKinematic = false;
                    transform.parent = null;
                    Carried = false;
                    GetComponent<NavMeshObstacle>().enabled = true;
                    Destroy(Shadow);

                }

            }
        }
    }
    void OnMouseDown()
    {
        IsClicked = true;
        Player.GetComponent<NavMeshAgent>().SetDestination(transform.position);
    }
}
