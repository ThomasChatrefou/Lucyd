using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FeumanBehaviour : MonoBehaviour
{
    Transform Player;
    public float GesteBarriere;
    private NavMeshAgent Feuman;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Feuman = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) > GesteBarriere)
            Feuman.destination = Player.position;
        else
            Feuman.destination = transform.position;
    }
}
