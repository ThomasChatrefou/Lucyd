using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour, IController
{
    private IRayProvider _rayProvider;
    private ISelector _selector;

    private Transform _newInteraction;

    private NavMeshAgent _agent;
    private Vector3 _newDestination;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _selector = GetComponent<ISelector>();
        _rayProvider = GetComponent<IRayProvider>();
    }

    public void OnMove()
    {
        _selector.Check(_rayProvider.CreateRay());
        _newDestination = _selector.GetSelectedPosition();
        _agent.SetDestination(_newDestination);
    }

    public void OnInteract()
    {
        _selector.Check(_rayProvider.CreateRay());
        _newInteraction = _selector.GetSelectedObject();

        if (_newInteraction) TryToInteract(_newInteraction);
    }

    public void TryToInteract(Transform objectToInteract)
    {
        var interactable = objectToInteract.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.OnInteract();
    }
}