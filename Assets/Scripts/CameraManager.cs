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
        CamAnchor = GameObject.Find("CameraAnchor").transform;
        Cam = GameObject.Find("Cams");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public IEnumerator CamSlide()
    {
        float PauseTimer = 2;
        Player.GetComponent<CharacterMovement>().enabled = false;
        Cam.GetComponent<CamsBehaviour>().enabled = false;
        Direction = (CamAnchor.position - Cam.transform.position).normalized;

        while (((Cam.transform.position + Direction * CamSpeed) - CamAnchor.position).magnitude > 0.5f)
        {
            Cam.transform.position += Direction * CamSpeed;
            yield return null;
        }
        while (PauseTimer > 0)
        {
            PauseTimer -= Time.deltaTime;
            yield return null;
        }

        Player.GetComponent<CharacterMovement>().enabled = true;
        Cam.GetComponent<CamsBehaviour>().enabled = true;
    }
}
