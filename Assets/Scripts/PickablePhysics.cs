using System.Collections;
using UnityEngine;


public class PickablePhysics : MonoBehaviour
{
    public float gravity = 100;

    private PanelBehaviour _pushingPanel;
    private PanelBehaviour _groundPanel;

    private bool _pushed = false;
    private bool _grounded = false;
    private bool _movingGrounded = false;

    private Vector3 _velocity = Vector3.zero;


    private void FixedUpdate()
    {
        if (_grounded || _movingGrounded)
        {
            DoStep();
        }
        else
        {
            if (_pushed)
                transform.position += _pushingPanel.GetVelocity() * Time.fixedDeltaTime;

            transform.position -= transform.up * gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        }
    }

    private void DoStep()
    {
        _velocity = Vector3.zero;
        if (_pushingPanel != null)
            _velocity += _pushingPanel.GetVelocity();

        if (_groundPanel != null)
            _velocity += _groundPanel.GetVelocity();

        transform.position += _velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            _pushed = true;
            _pushingPanel = other.GetComponent<PanelBehaviour>();
            DoStep();
            DoStep();
        }

        if (other.CompareTag("Ground"))
        {
            _grounded = true;
            if (_groundPanel == null) return;
            if (Vector3.Dot(_groundPanel.GetVelocity(), transform.up) >= 0) return;
            _movingGrounded = false;
            _groundPanel = null;
        }

        if (other.CompareTag("MovingGround"))
        {
            _movingGrounded = true;
            _groundPanel = other.GetComponent<PanelBehaviour>();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            _pushed = false;
            _pushingPanel = null;
        }

        if (other.CompareTag("Ground"))
        {
            _grounded = false;
        }

        if (other.CompareTag("MovingGround"))
        {
            _movingGrounded = false;
            _groundPanel = null;
        }
    }
}
