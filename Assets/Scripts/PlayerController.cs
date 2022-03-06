using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ISelector))]
public class PlayerController : MonoBehaviour, IController
{
    public event IController.DestinationReachedHandler DestinationReached;

    // number of new destinations every second when holding click
    [SerializeField] private float moveRateOnDrag = 12f;
    // number of interactions every second when holding click
    [SerializeField] private float interactionRateOnDrag = 2f;

    private float _lastMoveTime;
    private float _lastInteractionTime;
    private Vector3 _newDestination;
    private Vector3 _currentDestination;
    private Transform _newInteraction;
    private NavMeshAgent _agent;
    private ISelector _raycastSelector;
    private IInteractable _interactable;
    private IEnumerator _rotationCoroutine;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _raycastSelector = GetComponent<ISelector>();
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
        _raycastSelector.OnSelect();
        _newInteraction = _raycastSelector.GetSelectedObject();
        _newDestination = _raycastSelector.GetSelectedPosition();

        _interactable = _newInteraction.GetComponent<IInteractable>();
        if (_interactable != null) return true;
        return false;
    }

    public void OnMove()
    {
        _lastMoveTime = Time.time;
        _agent.SetDestination(_newDestination);
        _currentDestination = _newDestination;
    }

    public void MoveToDestinationWithOrientation(Transform target)
    {
        SetupDestinationAndRotation(target);
        OnMove();
        StartCoroutine(CheckDestinationReached());
    }

    private void SetupDestinationAndRotation(Transform target)
    {
        _newDestination = target.position;
        _rotationCoroutine = RotateCoroutine(target);
    }

    private IEnumerator CheckDestinationReached()
    {
        Vector3 destinationBuffer = _newDestination;

        while(_agent.remainingDistance >= _agent.stoppingDistance || _agent.pathPending)
        {
            if ((_currentDestination - destinationBuffer).magnitude > _agent.stoppingDistance)
                yield break;

            yield return null;
        }

        StartCoroutine(_rotationCoroutine);
    }

    private IEnumerator RotateCoroutine(Transform target)
    {
        _agent.isStopped = true;
        float t = 0f; //interpolation parameter, belongs to [0,1]

        while(1f - Vector3.Dot(transform.forward, target.forward) >= 1E-10f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t);
            t += Time.deltaTime;
            yield return null;
        }

        _agent.isStopped = false;
        DestinationReached?.Invoke();
    }

    public void Stop()
    {
        _agent.ResetPath();
    }

    public bool HasValidPathTo(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }
}