using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    private LayerMask LWall;
    private LayerMask DWall;

    private void Start()
    {
        LWall = 8;
        DWall = 9;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agent.isStopped = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, LWall) || Physics.Raycast(ray, out hit, 1000f, DWall))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}