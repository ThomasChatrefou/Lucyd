using UnityEngine;

internal interface ISelector
{
    public void OnSelect();

    public Transform GetSelectedObject();

    public Vector3 GetSelectedPosition();
}