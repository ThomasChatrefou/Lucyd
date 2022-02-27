using System.Collections;
using UnityEngine;


public class Button : MonoBehaviour, IInteractable
{
    public bool On { get; set; }

    [SerializeField] private bool canBeDisabledByHitAgain = true;
    [SerializeField] private bool hasTimer;
    [SerializeField] private float switchOnDuration = 3f;
    [SerializeField] private float antiSpamCooldown = 1f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string animatorTrigger = "Pressed";

    private bool _inRange;
    private bool _hasInteracted;
    private float _lastInteractionTime;
    private Animator _animator;
    private TimerHandler _timerBeforeSwitchOff;
    private IEnumerator _timerRoutine;
    private IController _characterController;


    private void Awake()
    {
        _characterController = GameObject.Find("Player").GetComponent<IController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!hasTimer) return;
        _timerBeforeSwitchOff = new TimerHandler();
        _timerBeforeSwitchOff.Ended += SwitchOff;
    }

    private void OnDestroy()
    {
        if (hasTimer) 
            _timerBeforeSwitchOff.Ended -= SwitchOff;
    }
    
    public void OnInteract()
    {
        if(Time.time - _lastInteractionTime < antiSpamCooldown) return;
        _lastInteractionTime = Time.time;

        if (_inRange)
        {
            Toggle();
        }
        else
        {
            _characterController.OnMove();
            _hasInteracted = true;
        }
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
        if (other.CompareTag(playerTag))
        {
            _inRange = true;
            if (_hasInteracted) Toggle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _inRange = false;
        }
    }
}