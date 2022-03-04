using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour, IController
{
    // number of new destinations every second when holding click
    [SerializeField] private float moveRateOnDrag = 12f;
    // number of interactions every second when holding click
    [SerializeField] private float interactionRateOnDrag = 2f;

    private float _lastMoveTime;
    private float _lastInteractionTime;
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

    public void OnInteract()
    {
        if (Time.time - _lastInteractionTime < 1f / interactionRateOnDrag) return;
        if (Time.time - _lastMoveTime < 1f / moveRateOnDrag) return;

        if (CheckInteraction())
        {
            _lastInteractionTime = Time.time;
            _interactable.OnInteract();
        }
        else
        {
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
        _newDestination = _selector.GetSelectedPosition();

        _interactable = _newInteraction.GetComponent<IInteractable>();
        if (_interactable != null) return true;
        return false;
    }

    public void OnMove()
    {
        _lastMoveTime = Time.time;
        _agent.SetDestination(_newDestination);
    }

    public void OnMoveToMousePosition()
    {
        _selector.Check(_rayProvider.CreateRay());
        _newDestination = _selector.GetSelectedPosition();
        OnMove();
    }
    
    public void OnMoveToDestination(Vector3 destination)
    {
        _newDestination = destination;
        OnMove();
    }

    public IEnumerator CheckDestinationReached()
    {
        while(_agent.remainingDistance >= 0.1f)
        {

            yield return null;
        }
    }
}