using UnityEngine;


public class SelectorDebuger : MonoBehaviour 
{ 
    private RayCastBasedLayerSelector _selector;

    private void Awake()
    {
        _selector = GetComponent<RayCastBasedLayerSelector>();
        _selector.Checked += Debug;
    }

    private void OnDestroy()
    {
        _selector.Checked -= Debug;
    }

    public void Debug()
    {
        print(_selector.GetSelectedObject().name);
        print(_selector.GetSelectedPosition());
    }
}