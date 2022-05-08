using UnityEngine;


[RequireComponent(typeof(SpotInteractor))]
public class LeverBehaviour : MonoBehaviour, IInteractable
{
    [HideInInspector] public float Percent = 0;

    [SerializeField] private Transform leverStick;
    [SerializeField] private float leverSpeed = 70;
    [SerializeField] private float maxRotation = 90;

    private bool _onSpot = false;
    private bool _interacting = false;
    private float _currentRotation = 0;
    private float _stepRotation;
    private GameObject _character;
    private PlayerController _characterController;
    private SpotInteractor _spotInteractor;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _spotInteractor = GetComponent<SpotInteractor>();

        _stepRotation = Time.deltaTime * leverSpeed;
    }

    private void Start()
    {
        _spotInteractor.SpotReached += OnSpot;
    }

    private void OnDestroy()
    {
        _spotInteractor.SpotReached -= OnSpot;
    }

    private void OnSpot()
    {
        _onSpot = _interacting;
    }

    public void OnBeginInteract()
    {
        _interacting = true;
        _characterController.DisableButtonMove();
        _spotInteractor.GoToNearestSpot();
    }

    public void OnInteract()
    {
    }
    
    public void OnEndInteract()
    {
        _characterController.EnableButtonMove();
        _interacting = false;
        _onSpot = false;
    }

    void FixedUpdate()
    {
        if (_onSpot)
        {
            if (_currentRotation > maxRotation) return;
            RotateOneStep(false);
        }
        else
        {
            if (_currentRotation < 0) return;
            RotateOneStep(true);
        }
    }

    private void RotateOneStep(bool inverseRotation)
    {
        int sign = inverseRotation ? -1 : 1;
        leverStick.Rotate(sign * _stepRotation, 0, 0, Space.Self);
        _currentRotation += sign * _stepRotation;
        Percent = _currentRotation / maxRotation;
        Mathf.Clamp(Percent, 0, 1);
    }
}
