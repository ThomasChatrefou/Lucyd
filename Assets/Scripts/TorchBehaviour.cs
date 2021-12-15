using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TorchBehaviour : MonoBehaviour
{
    private ButtonBehaviour Button;
    public bool lighted;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        Button = GetComponent<ButtonBehaviour>();
        transform.GetChild(0).gameObject.SetActive(lighted);
    }

    void Update()
    {
        if (lighted == false)
        {
            if(Button.on)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                lighted = true;
            }
        }
    }
}
