using UnityEngine;


public class PickableComponent : MonoBehaviour
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
    
    public IPickable GetPickable()
    {
        return _pickable;
    }

    public void SetPickableObject(GameObject pickableObject, IPickable pickable)
    {
        _pickableObject = pickableObject;
        _pickable = pickable;
    }
}
