using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IRayProvider _rayProvider;
    private IAgentMover _agentMover;

    private void Awake()
    {
        _agentMover = GetComponent<IAgentMover>();
        _rayProvider = GetComponent<IRayProvider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick();
        }
    }

    private void OnLeftClick()
    {

    }

    private void OnRightClick()
    {
        _agentMover.SetDestination(_rayProvider.CreateRay());
    }
}