using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool darkWorld = false;
    public float cooldown;

    public GameObject lightWorldEnvironment;
    public GameObject darkWorldEnvironment;

    public Camera darkWorldCamera;


    private bool mainCam = false;
    private float timer;

    private GameObject renderScreen;

    private List<Collider> lwColliders = new List<Collider>();
    private List<Collider> dwColliders = new List<Collider>();

    private List<NavMeshObstacle> lwObstacles = new List<NavMeshObstacle>();
    private List<NavMeshObstacle> dwObstacles = new List<NavMeshObstacle>();


    private void FillWorldLists(GameObject world, 
        ref List<Collider> colliders, ref List<NavMeshObstacle> obstacles)
    {
        for (int i = 0; i < world.transform.childCount; ++i)
        {
            Collider col = world.transform.GetChild(i).GetComponent<Collider>();
            if (col)
                colliders.Add(col);

            NavMeshObstacle nmo = world.transform.GetChild(i).GetComponent<NavMeshObstacle>();
            if (nmo)
                obstacles.Add(nmo);
        }
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        timer = 0;
        renderScreen = GameObject.Find("Canvas");

        FillWorldLists(lightWorldEnvironment, ref lwColliders, ref lwObstacles);
        FillWorldLists(darkWorldEnvironment, ref dwColliders, ref dwObstacles);

        foreach(Collider col in dwColliders)
            col.enabled = false;

        foreach(NavMeshObstacle obs in dwObstacles)
            obs.enabled = false;
    }


    public void ScreenFade()
    {
        if (mainCam)
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFade");
        else
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");

        mainCam = !mainCam;

        foreach (Collider col in dwColliders)
            col.enabled = !col.enabled;

        foreach (NavMeshObstacle obs in dwObstacles)
            obs.enabled = !obs.enabled;

        foreach (Collider col in lwColliders)
            col.enabled = !col.enabled;

        foreach (NavMeshObstacle obs in lwObstacles)
            obs.enabled = !obs.enabled;

        darkWorld = !darkWorld;
    }


    private void Update()
    {
        if (Input.GetAxis("Jump") > 0 && timer < 0)
        {
            ScreenFade();
            timer = cooldown;
        }
        timer -= Time.deltaTime;
    }
}
