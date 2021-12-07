using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    private GameObject renderScreen;
    private bool MainCam = false;
    public float CD;
    private float timer;
    public GameObject[] OnelyLightWDObject;
    public GameObject[] OnelyDarkWDObject;

    private void Start()
    {
        timer = 0;
        renderScreen = GameObject.Find("Canvas");

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
        }
        else
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");
            MainCam = true;
            foreach (GameObject obj in OnelyDarkWDObject)
                obj.GetComponent<Collider>().enabled = true;
            foreach (GameObject obj in OnelyLightWDObject)
                obj.GetComponent<Collider>().enabled = false;
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
