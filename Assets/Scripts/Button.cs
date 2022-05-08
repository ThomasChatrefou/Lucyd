using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpotInteractor))]
public class Button : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool On = false;

    [SerializeField] private bool canBeDisabledByHitAgain = true;
    [SerializeField] private bool hasTimer;
    [SerializeField] private float switchOnDuration = 3f;
    [SerializeField] private string animatorTrigger = "Pressed";

    private bool _onSpot = false;
    private bool _inRange;
    private bool _hasInteracted;
    private Animator _animator;
    private TimerHandler _timerBeforeSwitchOff;
    private IEnumerator _timerRoutine;
    private GameObject _character;
    private PlayerController _characterController;
    private SpotInteractor _spotInteractor;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>(); 
        _spotInteractor = GetComponent<SpotInteractor>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _spotInteractor.SpotReached += Toggle;
        if (!hasTimer) return;
        _timerBeforeSwitchOff = new TimerHandler();
        _timerBeforeSwitchOff.Ended += SwitchOff;
    }

    private void OnDestroy()
    {
        _spotInteractor.SpotReached -= Toggle;
        if (hasTimer) 
            _timerBeforeSwitchOff.Ended -= SwitchOff;
    }

    public void OnBeginInteract()
    {
        _spotInteractor.GoToNearestSpot();
    }
    
    public void OnInteract()
    {
    }

    public void OnEndInteract()
    {

    }

    private void Toggle()
    {
        if (_animator) _animator.SetTrigger(animatorTrigger);
        _hasInteracted = false;

        if (On)
        {
            if (canBeDisabledByHitAgain)
            {
                SwitchOff();
                if (hasTimer) 
                    StopCoroutine(_timerRoutine);
                return;
            }

            if (hasTimer) 
                ResetTimer();
        }
        else
        {
            SwitchOn();
        }
    }

    private void ResetTimer()
    {
        _timerBeforeSwitchOff.Countdown = switchOnDuration;
    }

    public void SwitchOn()
    {
        On = true;

        if (hasTimer)
        {
            _timerRoutine = _timerBeforeSwitchOff.Timer(switchOnDuration);
            StartCoroutine(_timerRoutine);
        }
    }

    public void SwitchOff()
    {
        On = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PLAYER))
        {
            _inRange = true;
            if (_hasInteracted) Toggle();
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