using UnityEngine;


public class InteractionSpotSelector : MonoBehaviour, ISelector
{
    private Vector3 _position;
    private Transform _selection;
    private Transform[] _interactionSpots;
    private GameObject _character;
    private PlayerController _characterController;

    private void Awake()
    {
        _character = GameObject.Find("Player");
        _characterController = _character.GetComponent<PlayerController>();
        _interactionSpots = GetComponentsInChildren<Transform>();
    }

    public void OnSelect()
    {
        SelectNearestSpot();
    }

    private void SelectNearestSpot()
    {
        int minIndex = 0;
        float minDistance = Mathf.Infinity;

        for (int i = 1; i < _interactionSpots.Length; i++)
        {
            if (!_characterController.HasValidPathTo(_interactionSpots[i].position)) continue;

            float distance = Vector3.Distance(_interactionSpots[i].position, _character.transform.position);

            if (minDistance <= distance) continue;

            minDistance = distance;
            minIndex = i;
        }

        _selection = _interactionSpots[minIndex];
        _position = _selection.position;
    }

    public Transform GetSelectedObject()
    {
        return _selection;
    }

    public Vector3 GetSelectedPosition()
    {
        return _position;
    }
}