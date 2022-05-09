using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviour : MonoBehaviour
{
    public LogicGate gate;

    public bool moveRight;
    public bool moveLeft;
    public bool moveUp;
    public bool moveDown;
    public bool moveForward;
    public bool moveBackward;

    public float dist;

    private Vector3 targetPos;
    private Vector3 originalPos;
    private Vector3 translation;
    private Vector3 velocity;

    public float smoothness = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (moveRight)
            translation += transform.right;

        if (moveLeft) 
            translation -= transform.right;

        if (moveUp) 
            translation += transform.up;

        if (moveDown) 
            translation -= transform.up;

        if (moveForward)
            translation += transform.forward;

        if (moveBackward)
            translation -= transform.forward;

        translation.Normalize();
        originalPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPos = originalPos + dist * translation;
        transform.position = Vector3.SmoothDamp(transform.position, Vector3.Lerp(originalPos, targetPos, gate.output), ref velocity, smoothness);
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
}
