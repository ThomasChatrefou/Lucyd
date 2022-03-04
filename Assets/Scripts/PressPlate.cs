using UnityEngine;


public interface ISpot
{
    public Vector3 GetSocketPosition();
}

public class PressPlate : MonoBehaviour, IInteractable, ISpot
{
    public bool On { get => nObjectsOnPress > 0; }

    [SerializeField] private Transform socket;
    [SerializeField] private string pressedTrigger = "Pressed";
    [SerializeField] private string releasedTrigger = "Released";

    private int nObjectsOnPress = 0;
    private Animator _animator;
    private GameObject _character;
    private IController _characterController;


    private void Start()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<IController>();
        _animator = GetComponent<Animator>();
    }

    public void OnInteract()
    {

    }

    public void OnBeginInteract()
    {
        _characterController.OnMoveToDestination(socket.position);
    }

    public void OnEndInteract()
    {

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