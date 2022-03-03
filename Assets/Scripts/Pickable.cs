using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public interface IPickable
{
    public void Pickup();
    public void Drop();
}

public class Pickable : MonoBehaviour, IInteractable, IPickable
{
    [SerializeField] private string pickupTrigger = "Pickup";
    [SerializeField] private string dropTrigger = "Drop";

    private bool _inRange;
    private bool _hasInteracted;
    private NavMeshObstacle _obstacle;
    private Animator _animator;
    private Transform _defaultParent;
    private GameObject _character;
    private IController _characterController;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<IController>();

        _obstacle = GetComponentInChildren<NavMeshObstacle>();
        _animator = GetComponentInChildren<Animator>();

        _defaultParent = transform.parent;
    }

    public void OnInteract()
    {
    }

    public void OnBeginInteract()
    {
        if (_inRange)
        {
            if (transform.parent.CompareTag(GameManager.TAG_PLAYER))
                Drop();
            else
                Pickup();
        }
        else
        {
            _characterController.OnMove();
            _hasInteracted = true;
        }
    }

    public void OnEndInteract()
    {

    }

    public void Pickup()
    {
        _animator.SetTrigger(pickupTrigger);
        _hasInteracted = false;
    }

    public void Drop()
    {
        _animator.SetTrigger(dropTrigger);
    }

    // called from animation event
    public void ParentToCharacter()
    {
        transform.SetParent(_character.transform, true);
        _obstacle.enabled = false;
    }

    // called from animation event
    public void UnparentFromCharacter()
    {
        transform.SetParent(_defaultParent, true);
        _obstacle.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PLAYER))
        {
            _inRange = true;
            if (_hasInteracted) Pickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PLAYER))
        {
            _inRange = false;
        }
    }
}