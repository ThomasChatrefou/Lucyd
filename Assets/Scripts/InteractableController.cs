using UnityEngine;


[RequireComponent(typeof(ISelector))]
class InteractableController : MonoBehaviour
{
    // number of interactions every second when holding click
    [SerializeField] private float interactionRateOnDrag = 2f;


    private float _lastInteractionTime;
    private IInteractable _Interactable;
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
        //print("coucou");
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
    }

    public bool CheckInteraction()
    {
        Transform newInteraction = _raycastSelector.GetSelectedObject();
        _Interactable = newInteraction.GetComponent<IInteractable>();
        if (_Interactable == null) return false;
        return true;
    }
}