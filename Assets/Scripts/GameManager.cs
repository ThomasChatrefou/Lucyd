using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject renderScreen;
    private bool MainCam = false;
    public float CD;
    private float timer;
    public GameObject[] OnelyLightWDObject;
    public GameObject[] OnelyDarkWDObject;
    public bool DarkWorld = false;

    private void Start()
    {
        timer = 0;
        renderScreen = GameObject.Find("Canvas");
        foreach(GameObject obj in OnelyDarkWDObject)
            obj.GetComponent<Collider>().enabled = false;
    }

    public void ScreenFade()
    {
        if (MainCam)
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFade");
            MainCam = false;
            foreach (GameObject obj in OnelyDarkWDObject)
                obj.GetComponent<Collider>().enabled = false;
            foreach (GameObject obj in OnelyLightWDObject)
                obj.GetComponent<Collider>().enabled = true;
            DarkWorld = false;
        }
        else
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");
            MainCam = true;
            foreach (GameObject obj in OnelyDarkWDObject)
                obj.GetComponent<Collider>().enabled = true;
            foreach (GameObject obj in OnelyLightWDObject)
                obj.GetComponent<Collider>().enabled = false;
            DarkWorld = true;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Jump") > 0 && timer < 0)
        {
            ScreenFade();
            timer = CD;
        }
        timer -= Time.deltaTime;
    }
}
