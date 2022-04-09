using UnityEngine;


public class PickableShadow : MonoBehaviour
{

    private PickableController _characterPickableController;

    private void Awake()
    {
        _characterPickableController = GameObject.Find("Player").GetComponent<PickableController>();
    }

    void Update()
    {
        transform.position = _characterPickableController.GetDropPosition();
    }
}
