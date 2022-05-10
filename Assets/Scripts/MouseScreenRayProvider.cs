using UnityEngine;

public class MouseScreenRayProvider : MonoBehaviour, IRayProvider
{
    public Camera cam;
    //private Camera cam;
    /*
    void Awake()
    {
        GameObject darkWorldCam = GameObject.Find("DarkWorldCam");
        if (darkWorldCam)
        {
            cam = darkWorldCam.GetComponent<Camera>();
        }
    }
    */
    public Ray CreateRay()
    {
        return cam.ScreenPointToRay(Input.mousePosition);
    }
}