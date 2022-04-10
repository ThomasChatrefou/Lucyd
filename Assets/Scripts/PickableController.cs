using UnityEngine;


public class PickableController : MonoBehaviour
{
    private bool _isLiftingPickable;
    private Vector3 _dropPosition;
    private GameObject _pickableObject;
    private IPickable _pickable;
    private ISelector _raycastSelector;
    private OneButtonInputHandler _input;

    private void Awake()
    {
        _isLiftingPickable = false;
        _raycastSelector = GetComponent<ISelector>();
        _input = GetComponent<OneButtonInputHandler>();
    }

    private void Start()
    {
        if (_input == null) return;
        _input.Button += UpdateDropPosition;
        _input.ButtonUp += UpdateDropPosition;
    }

    private void OnDestroy()
    {
        if (_input == null) return;
        _input.Button -= UpdateDropPosition;
        _input.ButtonUp -= UpdateDropPosition;
    }

    public void UpdateDropPosition()
    {
        if (!HasPickable() || _isLiftingPickable) return;

        Transform objectUnderCursor = _raycastSelector.GetSelectedObject();

        IInteractable interactable = objectUnderCursor.GetComponent<IInteractable>();
        if (interactable == null)
        {
            _dropPosition = _raycastSelector.GetSelectedPosition();
            return;
        }

        PressPlate pressPlate = objectUnderCursor.GetComponent<PressPlate>();
        if(pressPlate != null)
        {
            if (pressPlate.GetnObjectsOnPress() > 0) return;
            _dropPosition = pressPlate.GetSocketPosition();
            return;
        }
    }

    public void OnPickup(GameObject pickableObject, IPickable pickable)
    {
        _pickableObject = pickableObject;
        _pickable = pickable;
        _isLiftingPickable = true;
    }

    public void OnPickupAnimationEnd()
    {
        _isLiftingPickable = false;
    }

    public void OnDrop()
    {
        _pickable = null;
        _pickableObject = null;
    }

    public Vector3 GetDropPosition()
    {
        return _dropPosition;
    }

    public void SetDropPosition(Vector3 position)
    {
        _dropPosition = position;
    }

    public bool HasPickable()
    {
        return _pickableObject != null;
    }

    public bool IsLiftingPickable()
    {
        return _isLiftingPickable;
    }

    public GameObject GetPickableObject()
    {
        return _pickableObject;
    }
    
    public IPickable GetPickableComponent()
    {
        return _pickable;
    }
}
