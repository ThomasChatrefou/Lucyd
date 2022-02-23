using UnityEngine;


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

        if (Input.GetMouseButtonDown(0))
        {
            playerController.OnInteract();
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerController.OnMove();
        }
    }
}