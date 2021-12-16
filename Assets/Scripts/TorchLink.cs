using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLink : MonoBehaviour
{
    public TorchBehaviour linkedTorch;
    private TorchBehaviour thisTorch;
    private bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        thisTorch = GetComponent<TorchBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (done == false && linkedTorch.lighted)
        {
            print("link");
            thisTorch.lighted = true;
            transform.GetChild(0).gameObject.SetActive(true);
            done = true;
        }
    }
}
