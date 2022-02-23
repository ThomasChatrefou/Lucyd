using UnityEngine;

internal interface ISelector
{
    public void Check(Ray ray);

    public Transform GetSelectedObject();

    public Vector3 GetSelectedPosition();
}