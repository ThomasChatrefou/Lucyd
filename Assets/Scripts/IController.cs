using UnityEngine;


public interface IController
{
    public void OnInteract();
    public void OnBeginInteract();
    public void OnEndInteract();
    public void OnMove();
    public void OnMoveToMousePosition();
    public void OnMoveToDestination(Vector3 destination);
}
