using UnityEngine;


public interface IController
{
    public void OnInteract();
    public void OnBeginInteract();
    public void OnEndInteract();
    public void OnMove();
    public void MoveToDestinationWithOrientation(Transform spot);
    public void Stop();
    public bool HasValidPathTo(Vector3 position);

    public delegate void DestinationReachedHandler();
    public event DestinationReachedHandler DestinationReached;
}
