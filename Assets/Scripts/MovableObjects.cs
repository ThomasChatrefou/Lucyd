using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    public GameObject CubeTarget;
    private GameObject Cube;
    private bool OnRange = false;
    private bool beingCarried = false;
    private bool touched = false;
    private bool getclicked = false;
    private bool dropped = false;

    private float desiredDurationDrop = 10f;
    private float elapsedTime = 0 ;

    private Vector3 dropPositon;
    private Vector3 StartPosition;
    RaycastHit hit;
    

    private void Start()
    {
      
    }

    void Update()
    {
        

        //check the distance between the object and the player
        float dist = Vector3.Distance(gameObject.transform.position, player.position);

       
        
        if (dist <= 1.25f)
        {

            OnRange = true;

            if(getclicked)
            {
                Cube = Instantiate(CubeTarget, hit.transform);
            }
        }

        else
        {
            OnRange = false;
        }

        if (beingCarried)
        {
            
           
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit);
            
            Cube.transform.position = hit.point + new Vector3(0,0.5f,0);

            if (touched)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                touched = false;
                
                getclicked = false;

             }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                transform.parent = null;
                dropped = true;
                StartPosition = transform.position;
                dropPositon = Cube.transform.position;
                GetComponent<Rigidbody>().isKinematic = false;
                
                beingCarried = false;
                getclicked = false;
                

            }
        }
        if (OnRange && getclicked)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            

            transform.parent = player;
            transform.position += new Vector3(1, 1, 0);
            beingCarried = true;

        }

        if (dropped)
        {
            elapsedTime += Time.deltaTime;
            //transform.Translate(Vector3.forward * Time.deltaTime);
            transform.position = Vector3.Lerp(StartPosition, dropPositon, Mathf.SmoothStep(0, 1, elapsedTime / desiredDurationDrop));
            transform.rotation = new Quaternion(0, 0, 0, 0);
           
            if (transform.position ==  dropPositon)
            {
                elapsedTime = 0;
                
                dropped = false;
                Destroy(Cube);
            }
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        if (beingCarried)
        {
            touched = true;
        }
    }
    private void OnMouseDown()
    {
        dropped = false;
        getclicked = true;
    }
}
