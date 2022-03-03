using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour, IController
{
    // number of new destinations computed every second when moving by holding click
    [SerializeField] private float dragMoveRate = 6f;

    private float _lastMoveTime;
    private Vector3 _newDestination;
    private Transform _newInteraction;
    private NavMeshAgent _agent;
    private IInteractable _interactable;
    private IRayProvider _rayProvider;
    private ISelector _selector;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _selector = GetComponent<ISelector>();
        _rayProvider = GetComponent<IRayProvider>();
    }

    public void OnMove()
    {
        _lastMoveTime = Time.time;
        _selector.Check(_rayProvider.CreateRay());
        _newDestination = _selector.GetSelectedPosition();
        _agent.SetDestination(_newDestination);
    }

    public void OnInteract()
    {
        if (CheckInteraction())
        {
            _interactable.OnInteract();
        }
        else
        {
            if (Time.time - _lastMoveTime < 1f / dragMoveRate) return;
            OnMove();
        }
    }

    public void OnBeginInteract()
    {
        if (CheckInteraction())
        {
            _interactable.OnBeginInteract();
        }
        else
        {
            OnMove();
        }
    }
    
    public void OnEndInteract()
    {
        if (CheckInteraction())
            _interactable.OnEndInteract();
    }

    public bool CheckInteraction()
    {
        _selector.Check(_rayProvider.CreateRay());
        _newInteraction = _selector.GetSelectedObject();

        _interactable = _newInteraction.GetComponent<IInteractable>();
        if (_interactable != null) return true;
        return false;
    }
}