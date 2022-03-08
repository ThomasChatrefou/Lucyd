using UnityEngine;


[RequireComponent(typeof(ISelector))]
public class PickableShadow : MonoBehaviour
{

    private ISelector _raycastSelector;

    void Start()
    {
        _raycastSelector = GetComponent<ISelector>();

    }

    void Update()
    {
        _raycastSelector.OnSelect();
        _raycastSelector.GetSelectedPosition();
        
        transform.position = _raycastSelector.GetSelectedPosition();
    }
}
