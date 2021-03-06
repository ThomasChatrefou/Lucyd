using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TorchBehaviour : MonoBehaviour
{
    private Button Button;
    public bool lighted;
    public bool canBeLighted;
    
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        Button = GetComponent<Button>();
        if(!lighted)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if (lighted == false && canBeLighted)
        {
            if (Button.On)
            {
                GameManager.instance.CountTorch();
                transform.GetChild(0).gameObject.SetActive(true);
                lighted = true;
            }
        }
    }
}
