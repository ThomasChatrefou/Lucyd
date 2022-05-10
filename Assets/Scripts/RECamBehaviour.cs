using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RECamBehaviour : MonoBehaviour
{
    public List<Transform> CamSpots;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform Spot in CamSpots)
        {
            if (Spot.name == "Spawn")
                transform.position = Spot.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCamSpot(string SpotName)
    {
        foreach(Transform Spot in CamSpots)
        {
            if (Spot.name == SpotName)
                transform.position = Spot.position;
        }
        
    }
}
