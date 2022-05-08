using UnityEngine;


public interface ISpot
{
    public Vector3 GetSocketPosition();
}

[RequireComponent(typeof(Animator))]
public class PressPlate : MonoBehaviour, IInteractable, ISpot
{
    public bool On { get => _nObjectsOnPress > 0; }

    [SerializeField] private Transform socket;
    [SerializeField] private string pressedTrigger = "Pressed";
    [SerializeField] private string releasedTrigger = "Released";

    private int _nObjectsOnPress = 0;
    private Animator _animator;
    private GameObject _character;
    private PickableController _characterPickableController;
    private PlayerController _characterController;

    private void Start()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _characterPickableController = _character.GetComponent<PickableController>();
        
        _animator = GetComponent<Animator>();
    }

    public void OnBeginInteract()
    {
        if (_nObjectsOnPress > 0) return;

        if (_characterPickableController == null)
        {
            _characterController.DisableButtonMove();
            _characterController.MoveToDestinationWithOrientation(socket);
            return;
        }

        if (!_characterPickableController.HasPickable())
        {
            _characterController.DisableButtonMove();
            _characterController.MoveToDestinationWithOrientation(socket);
            return;
        }

    }

    public void OnInteract() { }

    public void OnEndInteract()
    {
        if (_nObjectsOnPress > 0) return;

        if (_characterPickableController == null)
        {
            _characterController.EnableButtonMove();
        }

        if (!_characterPickableController.HasPickable())
        {
            _characterController.EnableButtonMove();
        }
    }

    public Vector3 GetSocketPosition()
    {
        return socket.position;
    }

    public int GetnObjectsOnPress()
    {
        return _nObjectsOnPress;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            _nObjectsOnPress++;
            if (_nObjectsOnPress > 1) return;
            if (_animator) _animator.SetTrigger(pressedTrigger);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            _nObjectsOnPress--;
            if (_nObjectsOnPress > 0) return;
            if (_animator) _animator.SetTrigger(releasedTrigger);
        }

        if (_nObjectsOnPress < 0) _nObjectsOnPress = 0;
    }
}