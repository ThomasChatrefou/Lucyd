using UnityEngine;


[RequireComponent(typeof(SpotInteractor))]
public class PickableShadow : MonoBehaviour
{
    private bool _fixed;
    private GameObject _character;
    private PlayerController _characterController;
    private PickableController _characterPickableController;
    private SpotInteractor _spotInteractor;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _characterPickableController = _character.GetComponent<PickableController>();
        _spotInteractor = GetComponent<SpotInteractor>();
    }

    private void Start()
    {
        _fixed = false;
        _characterController.EnableTargetMove(_spotInteractor.GetSelector());
    }

    void Update()
    {
        if (_fixed) return;
        transform.position = _characterPickableController.GetDropPosition();
    }

    public void Fix()
    {
        _fixed = true;
    }

    public SpotInteractor GetShadowSpotInteractor()
    {
        return _spotInteractor;
    }
}
