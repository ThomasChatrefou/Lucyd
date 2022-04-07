using UnityEngine;


public class OneButtonInputHandler : MonoBehaviour
{
    public delegate void ButtonDownHandler();
    public delegate void ButtonHandler();
    public delegate void ButtonUpHandler();

    public event ButtonDownHandler ButtonDown;
    public event ButtonHandler Button;
    public event ButtonUpHandler ButtonUp;

    private ISelector _raycastSelector;

    private void Awake()
    {
        _raycastSelector = GetComponent<ISelector>();
    }

    private void Update()
    {
        if (_raycastSelector == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            _raycastSelector.OnSelect();
            ButtonDown?.Invoke();
        }

        if (Input.GetButton("Fire1"))
        {
            _raycastSelector.OnSelect();
            Button?.Invoke();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _raycastSelector.OnSelect();
            ButtonUp?.Invoke();
        }
    }
}