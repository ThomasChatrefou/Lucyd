using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IWorldManager
{
    public LayerMask GetCurrentLayerMask();
}

public class WorldTransitionManager : MonoBehaviour, IWorldManager
{
    [SerializeField] private GameObject lightWorldEnvironment;
    [SerializeField] private GameObject darkWorldEnvironment;
    [SerializeField] private Transform lightChildOfDark;
    [SerializeField] private Transform magicCrate;
    [SerializeField] private float worldFadeCooldown = 1f;

    private List<Collider> lwColliders = new List<Collider>();
    private List<Collider> dwColliders = new List<Collider>();

    private List<NavMeshObstacle> lwObstacles = new List<NavMeshObstacle>();
    private List<NavMeshObstacle> dwObstacles = new List<NavMeshObstacle>();

    private List<Rigidbody> lwRigidbody = new List<Rigidbody>();
    private List<Rigidbody> dwRigidbody = new List<Rigidbody>();

    public static bool darkWorld = false;
    public bool worldSwap;
    private float worldFadeTimer;
    private ButtonBehaviour darkFeu;
    private GameObject renderScreen;

    private NavMeshAgent playerAgent;
    private NavMeshAgent feumanAgent;

    public LayerMask GetCurrentLayerMask()
    {
        if (darkWorld)
        {
            return LayerMask.GetMask("DarkWorld");
        }
        else
        {
            return LayerMask.GetMask("LightWorld");
        }
    }

    private void Awake()
    {
        InitializeVariables();

        FillWorldLists(lightWorldEnvironment, ref lwColliders, ref lwObstacles, ref lwRigidbody);
        FillWorldLists(darkWorldEnvironment, ref dwColliders, ref dwObstacles, ref dwRigidbody);

        HideDarkWorld();

        if (lightChildOfDark)
        {
            LinkObjectsInDifferentWorlds(lightChildOfDark, magicCrate);
        }
    }

    public void InitializeVariables()
    {
        worldFadeTimer = 0;
        renderScreen = GameObject.Find("RenderScreen");
        darkFeu = GameObject.Find("DarkFeu").GetComponent<ButtonBehaviour>();
        playerAgent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();

        GameObject follower = GameObject.FindWithTag("Follower");
        if (follower)
            feumanAgent = follower.GetComponent<NavMeshAgent>();
    }

    private void FillWorldLists(GameObject world,
        ref List<Collider> colliders, ref List<NavMeshObstacle> obstacles, ref List<Rigidbody> rigidbodies)
    {
        Transform child;
        Collider col;
        NavMeshObstacle nmo;
        Rigidbody rb;

        for (int i = 0; i < world.transform.childCount; ++i)
        {
            child = world.transform.GetChild(i);

            if (child.CompareTag("Door"))
            {
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

            rb = child.GetComponent<Rigidbody>();
            if (rb && child.CompareTag("MovableObject"))
                rigidbodies.Add(rb);
        }
    }

    public void HideDarkWorld()
    {
        foreach (Collider col in dwColliders)
            col.enabled = false;

        foreach (NavMeshObstacle obs in dwObstacles)
            obs.enabled = false;

        foreach (Rigidbody rb in dwRigidbody)
            rb.isKinematic = true;
    }

    public void LinkObjectsInDifferentWorlds(Transform child, Transform parent)
    {
        child.SetParent(parent);
    }

    private void Update()
    {
        if (Input.GetAxis("Jump") > 0 && worldFadeTimer < 0)
        {
            darkFeu.on = !darkFeu.on;
        }
        worldSwap = darkFeu.on;
        worldFadeTimer -= Time.deltaTime;

        if (worldSwap != darkWorld)
            ScreenFade();
    }

    public void ScreenFade()
    {
        if (darkWorld)
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFade");

            ToggleColliders(ref lwColliders, ref dwColliders);
            ToggleObstacles(ref lwObstacles, ref dwObstacles);
            ToggleRigidBodies(ref lwRigidbody, ref dwRigidbody);

            playerAgent.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;
            if (feumanAgent)
            {
                feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;
            }

        }
        else
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");

            ToggleColliders(ref dwColliders, ref lwColliders);
            ToggleObstacles(ref dwObstacles, ref lwObstacles);
            ToggleRigidBodies(ref dwRigidbody, ref lwRigidbody);

            playerAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            if (feumanAgent)
            {
                feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            }

        }

        darkWorld = !darkWorld;
        worldSwap = darkWorld;
        worldFadeTimer = worldFadeCooldown;
    }

    private void ToggleColliders(ref List<Collider> enabling, ref List<Collider> disabling)
    {
        foreach (Collider col in disabling)
            col.enabled = false;
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

    private void ToggleRigidBodies(ref List<Rigidbody> enabling, ref List<Rigidbody> disabling)
    {
        foreach (Rigidbody rb in enabling)
            rb.isKinematic = false;

        foreach (Rigidbody rb in disabling)
            rb.isKinematic = true;
    }
}
