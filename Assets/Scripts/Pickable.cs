using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(ISelector))]
public class Pickable : MonoBehaviour, IInteractable, IPickable
{
    //[SerializeField] private GameObject shadowPrefab;
    [SerializeField] private string pickupTrigger = "Pickup";
    [SerializeField] private string dropTrigger = "Drop";

    //private GameObject _shadow;
    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private Transform _defaultParent;
    private GameObject _character;
    private PickableController _characterPickableController;
    private PlayerController _characterController;
    private ISelector _nearestSpotSelector;


    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _characterPickableController = _character.GetComponent<PickableController>();

        _nearestSpotSelector = GetComponent<ISelector>();
        _obstacle = GetComponentInChildren<NavMeshObstacle>();
        _animator = GetComponentInChildren<Animator>();
        _defaultParent = transform.parent;
    }

    public void OnBeginInteract()
    {
        if (_characterPickableController == null) return;

        if (_characterPickableController.HasPickable())
        {
            _characterController.Stop();
            if (_characterPickableController.GetPickableObject() != gameObject) return;
            Drop();
        }
        else
        {
            _nearestSpotSelector.OnSelect();
            _characterController.DestinationReached += Pickup;
            _characterController.MoveToDestinationWithOrientation(_nearestSpotSelector.GetSelectedObject());
        }
    }

    public void OnInteract() { }

    public void OnEndInteract() { }

    public void Pickup()
    {
        transform.SetParent(_character.transform, true);

        _characterPickableController.SetPickableObject(gameObject, this);
        _characterController.DestinationReached -= Pickup;
        _obstacle.enabled = false;
        _animator.SetTrigger(pickupTrigger);

        //_shadow = Instantiate(shadowPrefab);
    }

    public void Drop()
    {
        transform.SetParent(_defaultParent, true);

        _characterPickableController.SetPickableObject(null, null);
        _obstacle.enabled = true;
        _animator.SetTrigger(dropTrigger);

        //Destroy(_shadow);
    }
}