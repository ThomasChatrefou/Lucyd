using UnityEngine;

public class OutlinerManager : MonoBehaviour
{

    private LayerMask mask;
    private Camera cam;
    private Outline[] OutlineArray;
    private Outline PrevOutline = null;


    void Start()
    {
        GameObject darkWorldCam = GameObject.Find("DarkWorldCam");
        if (darkWorldCam)
        {
            cam = darkWorldCam.GetComponent<Camera>();
        }

        OutlineArray = Object.FindObjectsOfType<Outline>();
        foreach (Outline outline in OutlineArray)
        {
            outline.enabled = false;
        }
    }

    void Update()
    {
        if(GameManager.instance)
            CustomMouseOver();
    }

    void CustomMouseOver()
    {
        if (WorldTransitionManager.darkWorld)
            mask = LayerMask.GetMask("DarkWorld");
        else
            mask = LayerMask.GetMask("LightWorld");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.GetComponent<Outline>())
            {
                hit.collider.GetComponent<Outline>().enabled = true;
                if (!PrevOutline)
                    PrevOutline = hit.collider.gameObject.GetComponent<Outline>();
                else if(PrevOutline.gameObject != hit.collider.gameObject)
                {
                    PrevOutline.enabled = false;
                    PrevOutline = hit.collider.gameObject.GetComponent<Outline>();
                }
            }
            else if (hit.collider.GetComponentInChildren<Outline>())
            {
                hit.collider.GetComponentInChildren<Outline>().enabled = true;
                if (!PrevOutline)
                    PrevOutline = hit.collider.gameObject.GetComponentInChildren<Outline>();
                else if (PrevOutline.gameObject != hit.collider.gameObject)
                {
                    PrevOutline.enabled = false;
                    PrevOutline = hit.collider.gameObject.GetComponentInChildren<Outline>();
                    PrevOutline.enabled = true;
                }
            }
            else if(PrevOutline)
                PrevOutline.enabled = false;
        }
        else if(PrevOutline)
            PrevOutline.enabled = false;

        mask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.gameObject.name == "DarkFeu")
            {
                if (PrevOutline)
                    PrevOutline.enabled = false;
                PrevOutline = hit.collider.gameObject.GetComponentInChildren<Outline>();
                PrevOutline.enabled = true;
            }
        }
    }
}