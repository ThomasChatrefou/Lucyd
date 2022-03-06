using UnityEngine;


public interface ISpot
{
    public Vector3 GetSocketPosition();
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ISelector))]
public class PressPlate : MonoBehaviour, IInteractable, ISpot
{
    public bool On { get => nObjectsOnPress > 0; }

    [SerializeField] private Transform socket;
    [SerializeField] private string pressedTrigger = "Pressed";
    [SerializeField] private string releasedTrigger = "Released";

    private int nObjectsOnPress = 0;
    private Animator _animator;
    private GameObject _character;
    private PickableComponent _characterPickableComponent;
    private IController _characterController;
    private ISelector _nearestSpotSelector;


    private void Start()
    {
        _character = GameObject.Find("Player");
        _characterPickableComponent = _character.GetComponent<PickableComponent>();
        _characterController = _character.GetComponent<IController>();

        _animator = GetComponent<Animator>();
        _nearestSpotSelector = GetComponent<ISelector>();
    }

    public void OnInteract() { }

    public void OnBeginInteract()
    {
        if (_characterPickableComponent == null)
        {
            _characterController.MoveToDestinationWithOrientation(socket);
            return;
        }

        if (nObjectsOnPress > 0) return;

        if (_characterPickableComponent.HasPickable())
        {
            _nearestSpotSelector.OnSelect();
            _characterController.DestinationReached += DropPickableOnPlate;
            _characterController.MoveToDestinationWithOrientation(_nearestSpotSelector.GetSelectedObject());
        }
        else
        {
            _characterController.MoveToDestinationWithOrientation(socket);
        }
    }

    public void OnEndInteract() { }

    private void DropPickableOnPlate()
    {
        _characterController.DestinationReached -= DropPickableOnPlate;
        _characterPickableComponent.GetPickable().Drop();
    }

    public Vector3 GetSocketPosition()
    {
        return socket.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nObjectsOnPress > 0) return;

        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            nObjectsOnPress++;
            if (_animator) _animator.SetTrigger(pressedTrigger);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nObjectsOnPress < 1) return;

        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            nObjectsOnPress--;
            if (_animator) _animator.SetTrigger(releasedTrigger);
        }

        if (nObjectsOnPress < 0) nObjectsOnPress = 0;
    }
}