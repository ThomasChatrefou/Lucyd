using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    private LayerMask mask;

    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            agent.isStopped = false;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (GameManager.instance.darkWorld)
                mask = LayerMask.GetMask("DarkWorld");
            else
                mask = LayerMask.GetMask("LightWorld");

            if (Physics.Raycast(ray, out hit, 1000f, mask) || 
                Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Default")))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}