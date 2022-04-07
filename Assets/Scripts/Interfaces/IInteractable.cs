using UnityEngine;


internal interface IInteractable
{
    public void OnBeginInteract();
    public void OnInteract();
    public void OnEndInteract();
}

