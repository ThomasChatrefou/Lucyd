using UnityEngine;

public class RayCastBasedLayerSelector : MonoBehaviour, ISelector
{
    private IWorldManager _worldManager;

    private Transform _selection;
    private Vector3 _position;

    private void Awake()
    {
        _worldManager = GameObject.Find("GameManager").GetComponent<IWorldManager>();
    }

    public void Check(Ray ray)
    {
        LayerMask currentLayerMask;

        currentLayerMask = _worldManager.GetCurrentLayerMask();

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, currentLayerMask))
        {
            _selection = hit.transform;
            _position = hit.point;
        }
    }
    
    public Transform GetSelectedObject()
    {
        return _selection;
    }

    public Vector3 GetSelectedPosition()
    {
        return _position;
    }
}