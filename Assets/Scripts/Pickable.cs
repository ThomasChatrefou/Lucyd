using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(ISelector))]
public class Pickable : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private string pickupTrigger = "Pickup";
    [SerializeField] private string dropTrigger = "Drop";

    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private Transform _defaultParent;
    private GameObject _character;
    private PickableComponent _characterPickableComponent;
    private IController _characterController;
    private ISelector _nearestSpotSelector;


    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterPickableComponent = _character.GetComponent<PickableComponent>();
        _characterController = _character.GetComponent<IController>();

        _nearestSpotSelector = GetComponent<ISelector>();
        _obstacle = GetComponentInChildren<NavMeshObstacle>();
        _animator = GetComponentInChildren<Animator>();
        _defaultParent = transform.parent;
    }

    public void OnInteract() { }

    public void OnBeginInteract()
    {
        if (_characterPickableComponent == null) return;

        if (_characterPickableComponent.HasPickable())
        {
            _characterController.Stop();
            if (_characterPickableComponent.GetPickableObject() != gameObject) return;
            Drop();
        }
        else
        {
            _nearestSpotSelector.OnSelect();
            _characterController.DestinationReached += Pickup;
            _characterController.MoveToDestinationWithOrientation(_nearestSpotSelector.GetSelectedObject());
        }
    }

    public void OnEndInteract() { }

    public void Pickup()
    {
        transform.SetParent(_character.transform, true);

        _characterPickableComponent.SetPickableObject(gameObject, this);
        _characterController.DestinationReached -= Pickup;
        _obstacle.enabled = false;
        _animator.SetTrigger(pickupTrigger);
    }

    public void Drop()
    {
        transform.SetParent(_defaultParent, true);

        _characterPickableComponent.SetPickableObject(null, null);
        _obstacle.enabled = true;
        _animator.SetTrigger(dropTrigger);
    }
}