using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceToFind : MonoBehaviour
{
    public GameManager GameManager;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            GameManager.CountMana();
            Destroy(gameObject);
            if (GameManager.NbrMana == 3)
            {
                print("Les torches peuvent etre allumées ");
            }
        }
    }

}
