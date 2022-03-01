using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehaviour : MonoBehaviour, IInteractable
{
    [HideInInspector] public float Percent;

    [SerializeField] private Transform leverStick;
    [SerializeField] private float leverSpeed = 70;
    [SerializeField] private float maxRotation = 90;
    [SerializeField] private string playerTag = "Player";

    private bool _inRange = false;
    private bool _isPulling = false;
    private float _currentRotation = 0;
    private float _stepRotation;
    private IController _characterController;

    private void Awake()
    {
        _characterController = GameObject.Find("Player").GetComponent<IController>();
        _stepRotation = Time.deltaTime * leverSpeed;
    }

    public void OnInteract()
    {
        if (_inRange)
        {
            _isPulling = true;
        }
        else
        {
            _characterController.OnMove();
        }
    }

    void FixedUpdate()
    {
        if (_isPulling)
        {
            _isPulling = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _inRange = true;
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
