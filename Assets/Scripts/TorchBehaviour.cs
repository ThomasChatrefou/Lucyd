using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TorchBehaviour : MonoBehaviour
{
    public bool Lighted;
    private bool Clicked;
    public float SafetyDistance;
    private GameObject Player;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.GetChild(0).gameObject.SetActive(Lighted);
    }

    void Update()
    {
        if (Lighted == false)
        {
            if (Clicked == true)
            {
                if(Vector3.Distance(Player.transform.position, transform.position) > SafetyDistance)
                {
                    Player.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                }
                else
                {
                    Clicked = false;
                    Lighted = true;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        Clicked = true;
    }
}
