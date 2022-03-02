using UnityEngine;


public class PressPlate : MonoBehaviour
{
    [HideInInspector] public bool On = false;

    [SerializeField] private string pressedTrigger = "Pressed";
    [SerializeField] private string releasedTrigger = "Released";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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