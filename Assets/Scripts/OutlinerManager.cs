using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlinerManager : MonoBehaviour
{

    private LayerMask mask;
    private Camera cam;



    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("DarkWorldCam").GetComponent<Camera>();
        //GetComponent<Outline>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        CustomMouseOver();
    }

    void CustomMouseOver()
    {
        if (GameManager.instance.darkWorld)
            mask = LayerMask.GetMask("DarkWorld");
        else
            mask = LayerMask.GetMask("LightWorld");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.GetComponent<Outline>())
                hit.collider.gameObject.GetComponent<Outline>().enabled = true;
            else if (hit.collider.gameObject != this)
                GetComponent<Outline>().enabled = false;
        }
        
        mask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.gameObject.name == "DarkFeu")
                    hit.collider.gameObject.GetComponentInChildren<Outline>().enabled = true;
               
            //else if (hit.collider.gameObject != this)
                //GetComponent<Outline>().enabled = false;
        }
        
    }
}