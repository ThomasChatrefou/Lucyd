using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    
    public GameObject TerreMan;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Instantiate(TerreMan, new Vector3(0.5f,0.8f, 15), new Quaternion(0, 0, 0, 0));
               
            print("you finished it wp  bruhhh");
        }
    }
}
