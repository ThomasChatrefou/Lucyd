using UnityEngine;


public class PickableController : MonoBehaviour
{
    private GameObject _pickableObject;
    private IPickable _pickable;

    public bool HasPickable()
    {
        return _pickableObject != null;
    }

    public GameObject GetPickableObject()
    {
        return _pickableObject;
    }
    
    public IPickable GetPickableComponent()
    {
        return _pickable;
    }

    public void SetPickableObject(GameObject pickableObject, IPickable pickable)
    {
        _pickableObject = pickableObject;
        _pickable = pickable;
    }

    public void OnPickup(GameObject pickableObject)
    {
        pickableObject.transform.SetParent(transform, true);
    }

    public void OnDrop()
    {
        _pickable = null;
        _pickableObject = null;
    }
}
