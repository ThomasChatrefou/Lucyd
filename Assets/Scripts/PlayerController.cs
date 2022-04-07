using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(OneButtonInputHandler))]
[RequireComponent(typeof(ISelector))]
public class PlayerController : MonoBehaviour
{
    public event IController.DestinationReachedHandler DestinationReached;

    // number of new destinations every second when holding click
    [SerializeField] private float moveRateOnDrag = 12f;

    private float _lastMoveTime;
    private Vector3 _newDestination;
    private Vector3 _currentDestination;
    private NavMeshAgent _agent;
    private OneButtonInputHandler _input;
    private ISelector _raycastSelector;
    private IEnumerator _rotationCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _raycastSelector = GetComponent<ISelector>();
        _input = GetComponent<OneButtonInputHandler>();
    }

    private void Start()
    {
        if (_input == null) return;
        _input.ButtonDown += OnMove;
        _input.Button += OnMove;
    }

    private void OnDestroy()
    {
        if (_input == null) return;
        _input.ButtonDown -= OnMove;
        _input.Button -= OnMove;
    }

    public void OnMove()
    {
        if (Time.time - _lastMoveTime < 1f / moveRateOnDrag) return;
        _newDestination = _raycastSelector.GetSelectedPosition();
        Move();
    }

    public void Move()
    {
        _lastMoveTime = Time.time;
        _agent.SetDestination(_newDestination);
        _currentDestination = _newDestination;
    }

    public void MoveToDestinationWithOrientation(Transform target)
    {
        SetupDestinationAndRotation(target);
        Move();
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