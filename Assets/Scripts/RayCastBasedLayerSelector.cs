using UnityEngine;


[RequireComponent(typeof(IRayProvider))]
public class RayCastBasedLayerSelector : MonoBehaviour, ISelector
{
    private Transform _selection;
    private Vector3 _position;
    private IRayProvider _rayProvider;
    private IWorldManager _worldManager;

    public delegate void SelectorDelegate();
    public event SelectorDelegate Checked;

    private void Start()
    {
        if (_rayProvider == null)
        _rayProvider = GetComponent<IRayProvider>();
        if( GameManager.instance != null )
        _worldManager = GameManager.instance.GetComponent<IWorldManager>();
    }

    public void OnSelect()
    {
        Check(_rayProvider.CreateRay());
    }

    public void Check(Ray ray)
    {
        if (_worldManager == null) return;

        LayerMask currentLayerMask;

        currentLayerMask = _worldManager.GetCurrentLayerMask();

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, currentLayerMask))
        {
            _selection = hit.transform;
            _position = hit.point;
        }

        Checked?.Invoke();
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