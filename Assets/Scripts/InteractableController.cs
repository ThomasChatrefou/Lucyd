using UnityEngine;


[RequireComponent(typeof(ISelector))]
class InteractableController : MonoBehaviour
{
    // number of interactions every second when holding click
    [SerializeField] private float interactionRateOnDrag = 2f;

    private float _lastInteractionTime;
    private IInteractable _interactable;
    private ISelector _raycastSelector;
    private OneButtonInputHandler _input;

    private void Awake()
    {
        _raycastSelector = GetComponent<ISelector>();
        _input = GetComponent<OneButtonInputHandler>();
    }

    private void Start()
    {
        if (_input == null) return;
        _input.ButtonDown += OnBeginInteract;
        _input.Button += OnInteract;
        _input.Button += OnEndInteract;
    }

    private void OnDestroy()
    {
        if (_input == null) return;
        _input.ButtonDown -= OnBeginInteract;
        _input.Button -= OnInteract;
        _input.Button -= OnEndInteract;
    }

    public void OnBeginInteract()
    {
        if (CheckInteraction() == false) return;
        _interactable.OnBeginInteract(gameObject);
    }

    public void OnInteract()
    {
        if (Time.time - _lastInteractionTime < 1f / interactionRateOnDrag) return;
        if (CheckInteraction() == false) return;
        _lastInteractionTime = Time.time;
        _interactable.OnInteract();
    }

    public void OnEndInteract()
    {
        if (_interactable == null) return;
        _interactable.OnEndInteract();
    }

    public bool CheckInteraction()
    {
        Transform newInteraction = _raycastSelector.GetSelectedObject();
        _interactable = newInteraction.GetComponent<IInteractable>();
        if (_interactable == null) return false;
        return true;
    }
}