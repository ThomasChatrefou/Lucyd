using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Collider lightChildOfDark;
    public bool worldSwap;

    public bool darkWorld = false;
    public float worldFadeCooldown;
    private float worldFadeTimer;

    public Camera darkWorldCamera;

    public GameObject lightWorldEnvironment;
    public GameObject darkWorldEnvironment;
    public GameObject magicCrate;

    private GameObject renderScreen;
    private NavMeshAgent playerAgent;
    private NavMeshAgent feumanAgent;

    private List<Collider> lwColliders = new List<Collider>();
    private List<Collider> dwColliders = new List<Collider>();

    private List<NavMeshObstacle> lwObstacles = new List<NavMeshObstacle>();
    private List<NavMeshObstacle> dwObstacles = new List<NavMeshObstacle>();
    
    private List<Rigidbody> lwRigidbody = new List<Rigidbody>();
    private List<Rigidbody> dwRigidbody = new List<Rigidbody>();

    private ButtonBehaviour darkFeu;

    public int nbrMana = 0;

    public int nbrTorchLighted = 0;

    public GameObject portal;

    [SerializeField] private float resetLevelCooldown = 60f;
    private float resetLevelTimer;
    public SceneFader sceneFader;
    [SerializeField] private string menuName;

    private void FillWorldLists(GameObject world,
        ref List<Collider> colliders, ref List<NavMeshObstacle> obstacles, ref List<Rigidbody> rigidbody)
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
                rigidbody.Add(rb);
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

    private void ToggleRigidBodies(ref List<Rigidbody> enabling, ref List<Rigidbody> disabling)
    {
        foreach (Rigidbody rb in enabling)
            rb.isKinematic = false;

        foreach (Rigidbody rb in disabling)
            rb.isKinematic = true;
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        resetLevelTimer = resetLevelCooldown;
        worldFadeTimer = 0;
        renderScreen = GameObject.Find("Canvas");
        playerAgent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();
        feumanAgent = GameObject.FindWithTag("Follower").GetComponent<NavMeshAgent>();

        FillWorldLists(lightWorldEnvironment, ref lwColliders, ref lwObstacles, ref lwRigidbody);
        FillWorldLists(darkWorldEnvironment, ref dwColliders, ref dwObstacles, ref dwRigidbody);

        foreach(Collider col in dwColliders)
            col.enabled = false;

        foreach(NavMeshObstacle obs in dwObstacles)
            obs.enabled = false;

        foreach (Rigidbody rb in dwRigidbody)
            rb.isKinematic = true;

        darkFeu = GameObject.Find("DarkFeu").GetComponent<ButtonBehaviour>();

        lightChildOfDark.transform.SetParent(magicCrate.transform);
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
            feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;

        }
        else
        {
            renderScreen.GetComponentInChildren<Animation>().Play("CrossFadeRev");

            ToggleColliders(ref dwColliders, ref lwColliders);
            ToggleObstacles(ref dwObstacles, ref lwObstacles);
            ToggleRigidBodies(ref dwRigidbody, ref lwRigidbody);

            playerAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            feumanAgent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;

        }
        darkWorld = !darkWorld;
        worldSwap = darkWorld;
        worldFadeTimer = worldFadeCooldown;
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

        if (Input.anyKey)
        {
            resetLevelTimer = resetLevelCooldown;
        }

        if (resetLevelTimer < 0)
        {
            sceneFader.FadeTo(menuName);
        }

        resetLevelTimer -= Time.deltaTime;
    }
    public void CountTorch()
    {
        nbrTorchLighted += 1;
        if(nbrTorchLighted == 3)
        {
            Instantiate(portal, new Vector3(0,1,7),new Quaternion(0,90,90,90));
            print("a portal has open");
        }
    }
}
