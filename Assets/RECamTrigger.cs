using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RECamTrigger : MonoBehaviour
{
    private RECamBehaviour ReCam;
    // Start is called before the first frame update
    void Start()
    {
        ReCam = GameObject.Find("Cams").GetComponent<RECamBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            ReCam.GoToCamSpot(this.name);
    }
}
