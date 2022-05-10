using UnityEngine;

public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
{
    private Camera cam;

    void Start()
    {
        GameObject darkWorldCam = GameObject.Find("DarkWorldCam");
        if (darkWorldCam)
        {
            cam = darkWorldCam.GetComponent<Camera>();
        }
    }

    public Ray CreateRay()
    {
        return cam.ScreenPointToRay(Input.mousePosition);
    }
}