using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    

public class Portal : MonoBehaviour
{
    public GameObject TerreMan;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            SceneManager.LoadScene(2);
            print("you finished it wp  bruhhh");

        }
    }
}
