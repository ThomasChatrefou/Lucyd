using UnityEngine;


internal interface IInteractable
{
    public void OnBeginInteract(GameObject character);
    public void OnInteract();
    public void OnEndInteract();
}

