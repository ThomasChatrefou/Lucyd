using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public bool on;

    [SerializeField] private float cooldown = 1f;
    [SerializeField] private string playerTag = "Player";

    private float lastInteractionTime;

    private bool inRange;

    private void Update()
    {
        
    }


    public void OnInteract()
    {
        if(Time.time - lastInteractionTime < cooldown)
        {
            return;
        }

        lastInteractionTime = Time.time;

        if (inRange)
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (on)
        {
            on = false;
        }
        else
        {
            on = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = false;
        }
    }
}
