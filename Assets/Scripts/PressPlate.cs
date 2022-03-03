using UnityEngine;


public interface ISpot
{
    public Vector3 GetSocketPosition();
}

public class PressPlate : MonoBehaviour, IInteractable, ISpot
{
    [HideInInspector] public bool On = false;

    [SerializeField] private Transform socket;
    [SerializeField] private string pressedTrigger = "Pressed";
    [SerializeField] private string releasedTrigger = "Released";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnInteract()
    {

    }

    public void OnBeginInteract()
    {

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
        if (On) return;


        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            On = true;
            if (_animator) _animator.SetTrigger(pressedTrigger);
        }
        print(On);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!On) return;

        if (other.CompareTag(GameManager.TAG_PLAYER) || other.CompareTag(GameManager.TAG_MOVABLE))
        {
            On = false;
            if (_animator) _animator.SetTrigger(releasedTrigger);
        }
        print(On);
    }
}