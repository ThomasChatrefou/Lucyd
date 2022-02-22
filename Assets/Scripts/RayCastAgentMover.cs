using UnityEngine;
using UnityEngine.AI;

public class RayCastAgentMover : MonoBehaviour, IAgentMover
{
    private IWorldManager _worldManager;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _worldManager = GameObject.Find("GameManager").GetComponent<IWorldManager>();
    }

    public void SetDestination(Ray ray)
    {
        _agent.isStopped = false;
        LayerMask currentLayerMask;

        currentLayerMask = _worldManager.GetCurrentLayerMask();

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, currentLayerMask))
        {
            _agent.SetDestination(hit.point);
        }
    }
}