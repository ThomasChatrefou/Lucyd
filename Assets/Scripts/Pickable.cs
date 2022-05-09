using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(SpotInteractor))]
public class Pickable : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private string pickupTrigger = "Pickup";
    [SerializeField] private string dropTrigger = "Drop";

    private bool _interacting;
    private GameObject _shadow;
    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private Transform _defaultParent;
    private GameObject _character;
    private PlayerController _characterController;
    private InteractableController _characterInteractableController;
    private PickableController _characterPickableController;
    private SpotInteractor _spotInteractor;
    private PickablePhysics _pickablePhysics;


    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _characterInteractableController = _character.GetComponent<InteractableController>();
        _characterPickableController = _character.GetComponent<PickableController>();

        _spotInteractor = GetComponent<SpotInteractor>();
        _obstacle = GetComponentInChildren<NavMeshObstacle>();
        _animator = GetComponentInChildren<Animator>();
        _pickablePhysics = GetComponent<PickablePhysics>();
        _defaultParent = transform.parent;
    }

    private void Start()
    {
        _spotInteractor.SpotReached += Pickup;
    }

    private void OnDestroy()
    {
        _spotInteractor.SpotReached -= Pickup;
    }

    public void OnBeginInteract()
    {

        if (_interacting) return;

        if (_characterInteractableController.State == InteractionState.Waiting)
        {
            //print("BEGIN");
            _interacting = true;
            _pickablePhysics.enabled = false;
            _characterInteractableController.State = InteractionState.Approaching;
            _characterController.DisableButtonMove();
            _spotInteractor.GoToNearestSpot();
            return;
        }
    }

    public void OnInteract() { }

    public void OnEndInteract()
    {
        if (!_interacting) return;
        _interacting = false;

        if (_characterInteractableController.State == InteractionState.Approaching)
        {
            _characterController.EnableButtonMove();
            _characterInteractableController.State = InteractionState.Waiting;
            _spotInteractor.SpotReached -= Pickup;
            _spotInteractor.SpotReached += OnFailInteract;
            return;
        }

        if (_characterInteractableController.State == InteractionState.InAnimation)
        {
            Drop();
            return;
        }

        if (_characterInteractableController.State == InteractionState.Interacting)
        {
            if (_shadow)
            {
                _characterController.DisableTargetMove();
                PickableShadow pShadow = _shadow.GetComponent<PickableShadow>();
                pShadow.Fix();
                pShadow.GetShadowSpotInteractor().SpotReached += Drop;
                pShadow.GetShadowSpotInteractor().GoToNearestSpot();
            }
            else
            {
                Drop();
            }
            return;
        }
    }

    public void Pickup()
    {
        _characterInteractableController.State = InteractionState.InAnimation;
        transform.SetParent(_character.transform, true);
        _animator.SetTrigger(pickupTrigger);
        _characterPickableController.OnPickup(gameObject, this);
        _obstacle.enabled = false;
    }

    public void OnPickupAnimationEnd()
    {
        _characterInteractableController.State = InteractionState.Interacting;
        if (!_interacting) return;

        _characterPickableController.OnPickupAnimationEnd();
        _characterPickableController.SetDropPosition(transform.position);
        _shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
    }

    public void Drop()
    {
        _characterInteractableController.State = InteractionState.InAnimation;
        transform.SetParent(_defaultParent, true);
        _characterPickableController.OnDrop(); 
        _characterController.DestinationReached -= Drop;
        _obstacle.enabled = true;
        _animator.SetTrigger(dropTrigger);

    }

    public void OnDropAnimationEnd()
    {
        _pickablePhysics.enabled = true;
        _characterInteractableController.State = InteractionState.Waiting;
        _characterController.EnableButtonMove();
        Destroy(_shadow);
    }

    public void OnFailInteract()
    {
        // here : maybe add a trigger for some UI to tell the player to hold the button
        _spotInteractor.SpotReached -= OnFailInteract;
        _spotInteractor.SpotReached += Pickup;
    }
}