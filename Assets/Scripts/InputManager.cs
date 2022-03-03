﻿using UnityEngine;


public class InputManager : MonoBehaviour
{
    private IController playerController;

    private void Awake()
    {
        playerController = GetComponent<IController>();
    }

    private void Update()
    {
        if (playerController == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            playerController.OnBeginInteract();
        }

        if (Input.GetButton("Fire1"))
        {
            playerController.OnInteract();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            playerController.OnEndInteract();
        }
    }
}