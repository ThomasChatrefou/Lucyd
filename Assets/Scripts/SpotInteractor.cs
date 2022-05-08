using UnityEngine;


public class SpotInteractor : MonoBehaviour
{
    public delegate void DestinationReachedHandler();
    public event DestinationReachedHandler SpotReached;

    private GameObject _character;
    private PlayerController _characterController;
    private ISelector _nearestSpotSelector;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _nearestSpotSelector = GetComponentInChildren<ISelector>();
    }

    public void GoToNearestSpot()
    {
        _nearestSpotSelector.OnSelect();
        _characterController.DestinationReached += OnDestinationReached;
        _characterController.MoveToDestinationWithOrientation(_nearestSpotSelector.GetSelectedObject());
        //_characterController.DestinationAborted += OnDestinationAborted;
    }

    private void OnDestinationReached()
    {
        _characterController.DestinationReached -= OnDestinationReached;
        //_characterController.DestinationAborted -= OnDestinationAborted;
        SpotReached?.Invoke();
    }
    
    private void OnDestinationAborted()
    {
        print("aborted");
        _characterController.DestinationReached -= OnDestinationReached;
        _characterController.DestinationAborted -= OnDestinationAborted;
        SpotReached?.Invoke();
    }

    public ISelector GetSelector()
    {
        return _nearestSpotSelector;
    }
}
