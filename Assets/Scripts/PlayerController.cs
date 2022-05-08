using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(OneButtonInputHandler))]
[RequireComponent(typeof(ISelector))]
public class PlayerController : MonoBehaviour
{
    //public event IController.DestinationReachedHandler DestinationReached;

    public delegate void DestinationReachedHandler();
    public event DestinationReachedHandler DestinationReached;
    
    public delegate void DestinationAbortedHandler();
    public event DestinationAbortedHandler DestinationAborted;

    // number of new destinations every second when holding click
    [SerializeField] private float moveRateOnDrag = 12f;

    private int nCheckDestRoutine;
    private int nRotationRoutine;


    private float _lastMoveTime;
    private Vector3 _newDestination;
    private Vector3 _currentDestination;
    private NavMeshAgent _agent;
    private OneButtonInputHandler _input;
    private ISelector _defaultSelector;
    private ISelector _currentSelector;
    private IEnumerator _rotationCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _defaultSelector = GetComponent<ISelector>();
        _currentSelector = _defaultSelector;
        _input = GetComponent<OneButtonInputHandler>();
    }

    private void Start()
    {
        nCheckDestRoutine = 0;
        nRotationRoutine = 0;

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

    public void DisableButtonMove()
    {
        if (_input == null) return;
        _input.ButtonDown -= OnMove;
        _input.Button -= OnMove;
    }

    public void EnableButtonMove()
    {
        if (_input == null) return;
        _input.ButtonDown += OnMove;
        _input.Button += OnMove;
    }

    public void EnableTargetMove(ISelector newSelector)
    {
        if (_input == null) return;
        _currentSelector = newSelector;
        _input.Button += OnTargetMove;
    }
    
    public void DisableTargetMove()
    {
        if (_input == null) return;
        _currentSelector = _defaultSelector;
        _input.Button -= OnTargetMove;
    }

    public void OnMove()
    {
        if (Time.time - _lastMoveTime < 1f / moveRateOnDrag) return;
        _newDestination = _currentSelector.GetSelectedPosition();
        print("move");
        Move();
    }

    public void OnTargetMove()
    {
        if (Time.time - _lastMoveTime < 1f / moveRateOnDrag) return;
        if (_currentSelector == null) return;

        _currentSelector.OnSelect();
        MoveToDestinationWithOrientation(_currentSelector.GetSelectedObject());
    }

    public void Move()
    {
        _lastMoveTime = Time.time;
        _agent.SetDestination(_newDestination);
        _currentDestination = _newDestination;
    }

    public void MoveToDestination(Vector3 target)
    {
        SetupDestination(target);
        Move();
        StartCoroutine(CheckDestinationReached());
    }

    private void SetupDestination(Vector3 target)
    {
        _newDestination = target;
        _rotationCoroutine = null;
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
        nCheckDestRoutine++;
        Vector3 destinationBuffer = _newDestination;

        while(_agent.remainingDistance >= _agent.stoppingDistance || _agent.pathPending)
        {
            if ((_currentDestination - destinationBuffer).magnitude > _agent.stoppingDistance)
            {
                nCheckDestRoutine--;
                DestinationAborted?.Invoke();
                yield break;
            }

            yield return null;
        }

        if (_rotationCoroutine != null)
        {
            if (nRotationRoutine > 0)
            {
                nCheckDestRoutine--;
                yield break;
            }
            StartCoroutine(_rotationCoroutine);
        }
        else
        {
            DestinationReached?.Invoke();
        }

        nCheckDestRoutine--;
    }

    private IEnumerator RotateCoroutine(Transform target)
    {
        if(target == null) yield break;

        nRotationRoutine++;
        _agent.isStopped = true;
        float t = 0f;   // interpolation parameter, belongs to [0,1]

        while(target != null)
        {
            if (1f - Vector3.Dot(transform.forward, target.forward) < 1E-10f) break;
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t);
            t += Time.deltaTime;
            yield return null;
        }

        nRotationRoutine--;
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