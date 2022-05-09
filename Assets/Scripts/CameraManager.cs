using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform CamAnchor;
    private GameObject Cam;
    private GameObject Player;
    private Vector3 Direction;
    public float CamSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        GameObject cameraAnchor = GameObject.Find("CameraAnchor");
        if (cameraAnchor)
        {
            CamAnchor = cameraAnchor.transform;
        }

        Cam = GameObject.Find("Cams");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public IEnumerator CamSlide()
    {
        float PauseTimer = 2;
        Player.GetComponent<PlayerController>().enabled = false;
        Cam.GetComponent<RECamBehaviour>().enabled = false;
        Direction = (CamAnchor.position - Cam.transform.position).normalized;
        Vector3 OldCamPos = Cam.transform.position;

        while (((Cam.transform.position + Direction * CamSpeed) - CamAnchor.position).magnitude > 0.5f)
        {
            Cam.transform.position += Direction * CamSpeed;
            yield return 0;
        }
        while (PauseTimer > 0)
        {
            PauseTimer -= Time.deltaTime;
            yield return 0;
        }

        Player.GetComponent<PlayerController>().enabled = true;
        Cam.GetComponent<RECamBehaviour>().enabled = true;
        Cam.transform.position = OldCamPos;
    }
}
