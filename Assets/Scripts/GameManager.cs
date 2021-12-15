using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Collider LightChildOfDark;

    public bool darkWorld = false;
    public float cooldown;

    public GameObject lightWorldEnvironment;
    public GameObject darkWorldEnvironment;
    public GameObject MagicCrate;

    public Camera darkWorldCamera;

    private float timer;

    private GameObject renderScreen;
    private NavMeshAgent playerAgent;
    private NavMeshAgent feumanAgent;

    private List<Collider> lwColliders = new List<Collider>();
    private List<Collider> dwColliders = new List<Collider>();

    private List<NavMeshObstacle> lwObstacles = new List<NavMeshObstacle>();
    private List<NavMeshObstacle> dwObstacles = new List<NavMeshObstacle>();

    private ButtonBehaviour DarkFeu;
    private ButtonBehaviour LightFeu;

    private void FillWorldLists(GameObject world,
        ref List<Collider> colliders, ref List<NavMeshObstacle> obstacles)
    {
        Transform child;
        Collider col;
        NavMeshObstacle nmo;

        for (int i = 0; i < world.transform.childCount; ++i)
        {
            child = world.transform.GetChild(i);

            if (child.CompareTag("Door")) {
                colliders.Add(child.transform.GetChild(0).GetComponent<Collider>());
                colliders.Add(child.transform.GetChild(1).GetComponent<Collider>());

                obstacles.Add(child.transform.GetChild(0).GetComponent<NavMeshObstacle>());
                obstacles.Add(child.transform.GetChild(1).GetComponent<NavMeshObstacle>());
            }

            col = child.GetComponent<Collider>();
            if (col && !child.CompareTag("Ground"))
                colliders.Add(col);

            nmo = child.GetComponent<NavMeshObstacle>();
            if (nmo)
                obstacles.Add(nmo);
        }
    }

    private void ToggleColliders(ref List<Collider> enabling, ref List<Collider> disabling)
    {
        foreach (Collider col in disabling)
            col.enabled = false;
        //yield return new WaitForEndOfFrame();
        foreach (Collider col in enabling)
            col.enabled = true;
    }


    private void ToggleObstacles(ref List<NavMeshObstacle> enabling, ref List<NavMeshObstacle> disabling)
    {
        foreach (NavMeshObstacle obs in enabling)
            obs.enabled = true;

        foreach (NavMeshObstacle obs in disabling)
            obs.enabled = false;
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        timer = 0;
        renderScreen = GameObject.Find("Canvas");
        playerAgent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
        feumanAgent = GameObject.FindWithTag("Follower").GetComponent<NavMeshAgent>();

        FillWorldLists(lightWorldEnvironment, ref lwColliders, ref lwObstacles);
        FillWorldLists(darkWorldEnvironment, ref dwColliders, ref dwObstacles);

        foreach(Collider col in dwColliders)
            col.enabled = false;

        foreach(NavMeshObstacle obs in dwObstacles)
            obs.enabled = false;

        DarkFeu = GameObject.Find("DarkFeu").GetComponent<ButtonBehaviour>();
        LightFeu = GameObject.Find("LightFeu").GetComponent<ButtonBehaviour>();

        LightChildOfDark.transform.SetParent(MagicCrate.transform);
    }


    public void ScreenFade()
    {
        if (darkWorld)
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFade");

            ToggleColliders(ref lwColliders, ref dwColliders);
            ToggleObstacles(ref lwObstacles, ref dwObstacles);

            playerAgent.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;
            feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;

            DarkFeu.enabled = false;
            LightFeu.enabled = true;
        }
        else
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");

            ToggleColliders(ref dwColliders, ref lwColliders);
            ToggleObstacles(ref dwObstacles, ref lwObstacles);

            playerAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;

            DarkFeu.enabled = true;
            LightFeu.enabled = false;
        }

        darkWorld = !darkWorld;
        timer = cooldown;
    }


    private void Update()
    {
        if (Input.GetAxis("Jump") > 0 && timer < 0)
        {
            ScreenFade();
            LightFeu.on = !LightFeu.on;
        }
        timer -= Time.deltaTime;

        if (darkWorld && LightFeu.on == DarkFeu.on)
        {
            ScreenFade();
        }
        else if (!darkWorld && LightFeu.on != DarkFeu.on)
        {
            ScreenFade();
        }
    }
}
