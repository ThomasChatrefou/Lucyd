using UnityEngine;


public class PickableManager : MonoBehaviour
{
    private IPickable _pickable;

    public void SetPickable(IPickable pickable)
    {
        _pickable = pickable;
    }
}
