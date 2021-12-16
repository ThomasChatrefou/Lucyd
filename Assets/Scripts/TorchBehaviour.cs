using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TorchBehaviour : MonoBehaviour
{
    private GameManager gameManager;
    private ButtonBehaviour Button;
    public bool lighted;
    public bool CanBeLighted = false;
    public bool DoOnce = false;
    
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Button = GetComponent<ButtonBehaviour>();
        
    }

    void Update()
    {
        if (gameManager.NbrMana == 3 )
        {
            CanBeLighted = true;
           
         }
        else
        {
            Button.on = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        
        if (lighted == false)
            {
            if (CanBeLighted)
            {
                if (Button.on)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    lighted = true;
                    if (DoOnce == false)
                    {
                        gameManager.CountTorch();
                        DoOnce = true;
                    }

                }
            }
               
              
            }
            
        
    }
   
    void EndStep() 
        {
        CanBeLighted = true;
    }
}
