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
    public float speed = 1.0f;

    private float eps = 0.01f;

    private Vector3 targetPos;
    private Vector3 originalPos;
    private Vector3 translation;


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
        targetPos = originalPos + gate.output * dist * translation;

        Vector3 pos = transform.position;

        float sgn;

        if (Vector3.Distance(pos, targetPos) > eps)
        {
            if (Vector3.Distance(pos, originalPos) < eps)
                sgn = 1.0f;
            else
                sgn = -Mathf.Sign(Vector3.Dot(originalPos - pos, targetPos - pos));

            transform.Translate(sgn * Time.deltaTime * speed * translation, Space.World);
        }
    }
}
