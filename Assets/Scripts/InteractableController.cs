using UnityEngine;


enum InteractionState
{
    Waiting,
    Approaching,
    InAnimation,
    Interacting
}


[RequireComponent(typeof(ISelector))]
class InteractableController : MonoBehaviour
{
    [HideInInspector] public InteractionState State;

    // number of interactions every second when holding click
    [SerializeField] private float interactionRateOnDrag = 2f;

    private float _lastInteractionTime;
    private IInteractable _Interactable = null;
    private ISelector _raycastSelector;
    private OneButtonInputHandler _input;

    private void Awake()
    {
        _raycastSelector = GetComponent<ISelector>();
        _input = GetComponent<OneButtonInputHandler>();
    }

    private void Start()
    {
        State = InteractionState.Waiting;
        if (_input == null) return;
        _input.ButtonDown += OnBeginInteract;
        _input.Button += OnInteract;
        _input.ButtonUp += OnEndInteract;
    }

    private void OnDestroy()
    {
        if (_input == null) return;
        _input.ButtonDown -= OnBeginInteract;
        _input.Button -= OnInteract;
        _input.ButtonUp -= OnEndInteract;
    }

    public void OnBeginInteract()
    {
        if (_Interactable != null) return;
        if (State != InteractionState.Waiting) return;

        if (CheckInteraction() == false) return;
        _Interactable.OnBeginInteract();
    }

    public void OnInteract()
    {
        if (Time.time - _lastInteractionTime < 1f / interactionRateOnDrag) return;
        if (_Interactable == null) return;
        _lastInteractionTime = Time.time;
        _Interactable.OnInteract();
    }

    public void OnEndInteract()
    {
        if (_Interactable == null) return;

        _Interactable.OnEndInteract();

        _Interactable = null;
    }

    public bool CheckInteraction()
    {
        Transform newInteraction = _raycastSelector.GetSelectedObject();
        if (newInteraction == null) return false;
        _Interactable = newInteraction.GetComponent<IInteractable>();
        if (_Interactable == null) return false;
        return true;
    }

}