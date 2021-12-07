using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamsBehaviour : MonoBehaviour
{
    public float timeOffset;
    public GameObject player;

    private Vector3 constantOffset;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        constantOffset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = constantOffset + 2 * player.transform.forward;

        //transform.position = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position,
            player.transform.position + offset,
            ref velocity,
            timeOffset);
    }
}
